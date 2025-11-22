using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    [RequireComponent(typeof(Worker))]
    [RequireComponent(typeof(UnitMover))]
    public class WorkerAI : MonoBehaviour
    {
        public event Action<WorkerAI> OnEatCompleted;
        public event Action<WorkerAI> OnTaskStarted;     // [작업 시작]
        public event Action<WorkerAI> OnTaskFinished;    // [작업 완료]

        [Header("State")]
        [SerializeField] private float hunger;
        [SerializeField] private bool  hasNewTask = false;
        [SerializeField] private bool  hasPausedTask = false;
        private bool Ishunger;

        [Header("BT Settings")]
        public float btTickInterval = 0.1f;
        private float btTimer = 0f;

        // 컴포넌트
        private Worker worker;
        private UnitMover mover;
        private IUnitMoveable moverAdapter;
        private BTNode root;

        // 로컬 블랙보드->동적 데이터 동기화
        // WorkerAI의 변수 값이 바뀌면 -> 블랙보드도 즉시 업데이트됨
        // BT 노드들은 변수를 직접 안 보고 블랙보드의 Key만 봄
        public WorkerBlackboard LocalBlackboard { get; private set; }
        public float Hunger
        {
            get => hunger;
            set
            {
                hunger = value;
                LocalBlackboard.SetValue(BBKeys.Hunger, hunger);

                bool hungryFlag = hunger >= HungerRules.Hunger;
                LocalBlackboard.SetValue(BBKeys.IsHungry, hungryFlag);
            }
        }

        public bool HasNewTask
        {
            get => hasNewTask;
            set
            {
                hasNewTask = value;
                LocalBlackboard.SetValue(BBKeys.HasNewTask, hasNewTask);
            }
        }
        public bool HasPausedTask
        {
            get => hasPausedTask;
            set
            {
                hasPausedTask = value;
                LocalBlackboard.SetValue(BBKeys.HasPausedTask, hasPausedTask);
            }
        }

        // 생산 작업 참조
        private ProductableBuilding currentTaskBuilding = null;
        private ProductableBuilding pausedTaskBuilding = null;
        private ProductableBuilding newAssignedBuilding = null;

        private void Awake()
        {
            worker = GetComponent<Worker>();
            mover = GetComponent<UnitMover>();
            moverAdapter = mover as IUnitMoveable;

            LocalBlackboard = new WorkerBlackboard();

        }

        private void Start()
        {


            // BT 트리 구성
            SettingBT();
        }
        private void SyncFieldsToBlackboard()
        {
            //정적 데이터(참조) 등록
            LocalBlackboard.SetValue(BBKeys.OwnerAI, this);
            LocalBlackboard.SetValue(BBKeys.Self, worker);
            LocalBlackboard.SetValue(BBKeys.UnitMover, moverAdapter);

            // BT 상태 초기화
            LocalBlackboard.SetValue(BBKeys.Hunger, hunger);
            bool hungryFlag = hunger >= HungerRules.Hunger;
            LocalBlackboard.SetValue(BBKeys.IsHungry, hungryFlag);

            LocalBlackboard.SetValue(BBKeys.HasNewTask, hasNewTask);
            LocalBlackboard.SetValue(BBKeys.HasPausedTask, hasPausedTask);
        }



        void SettingBT()
        {
            // 모든 Leaf Node 생성자에 LocalBlackboard를 전달 (주입)
            // CompositeNode(Sequence/Selector)는 블랙보드가 필요 없으므로 리스트만 전달

            // Sequence: 배고픔 처리
            BTNode hungerSequence = new SequenceNode(new List<BTNode>
         {
             new IsHealthLowChecker(LocalBlackboard),
             new PauseCurrentTask(LocalBlackboard),
             new GoToEatingPlace(LocalBlackboard),
             new ReturnToPausedTask(LocalBlackboard),
             new EatFood(LocalBlackboard),
         });

            // Sequence: 하던 일 재개
        BTNode resumeTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsPausedTaskChecker(LocalBlackboard),
            new GoToPausedTaskLocation(LocalBlackboard),
            //new ResumePausedTask(LocalBlackboard)
        });

            // Sequence: 새 일 시작
        BTNode newTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsNewTaskChecker(LocalBlackboard),
            new GoToNewTaskLocation(LocalBlackboard),
            //new StartNewTask(LocalBlackboard)
        });

            // Selector: 작업/휴식
        BTNode taskSelector = new SelectorNode(new List<BTNode>
        {
            resumeTaskSequence,
            newTaskSequence,
            new GoToLounge(LocalBlackboard)
        });

            // Root Selector: 배고픔 → 작업/휴식
            root = new SelectorNode(new List<BTNode>
            {
                hungerSequence,
                taskSelector
            });

        }
        

        private void Update()
        {
            if (root == null) return;

           // Hunger = Mathf.Clamp01(hunger - Time.deltaTime * 0.01f);

            // protected, private 보호수준에 막힘.
            // @TODO: ProductableBuilding의 currBuildState 가져오는 방법 고민하기 
            //if (currentTaskBuilding != null && currentTaskBuilding.currBuildState is ProductableState pState && pState != null)
            //{
            //    // @TODO: ProductableState data를 블랙보드에 등록할 수 있는 함수 필요.
            //    // 블랙보드 생산 데이터 업데이트
            //    LocalBlackboard.SetValue(BBKeys.ProductionStateData, pState.data);
            //    LocalBlackboard.SetValue(BBKeys.IsProductionCompleted, pState.data.IsCompleted);
            //    LocalBlackboard.SetValue(BBKeys.ProductionProgress, pState.data.Progress);

               //btTimer += Time.deltaTime;
               //if (btTimer >= btTickInterval)
               //{
               //    btTimer = 0f;
               //}
                   root?.Evaluate();
           //}
        }
        public void AssignTask(ProductableBuilding building)
        {
            CancelOrReplaceCurrentTask();

            newAssignedBuilding = building;
            HasNewTask = true;

            LocalBlackboard.SetValue(BBKeys.TargetBuilding, building);

            //@TODO : 구조 확정되면 추가하기
            //LocalBlackboard.SetValue(BBKeys.TargetPosition, building.GetWorkerEntranceWorldPos(null));
            //if (building.currBuildState is ProductableState ps)
            {
                //LocalBlackboard.SetValue(BBKeys.ProductionStateData, ps.Data);
                //LocalBlackboard.SetValue(BBKeys.IsProductionCompleted, ps.Data.IsCompleted);
                //LocalBlackboard.SetValue(BBKeys.ProductionProgress, ps.Data.Progress);
            }
        }

        private void CancelOrReplaceCurrentTask()
        {
            if (currentTaskBuilding != null)
            {
                // store as paused task only if we will resume it later (hunger case)
                pausedTaskBuilding = currentTaskBuilding;
                HasPausedTask = true;
            }
            currentTaskBuilding = null;
            LocalBlackboard.Remove(BBKeys.TargetBuilding);
            LocalBlackboard.Remove(BBKeys.TargetPosition);
            HasNewTask = false;
        }

        public void ClearPausedTask()
        {
            pausedTaskBuilding = null;
            HasPausedTask = false;
        }

        public void StartWorkingAt(ProductableBuilding building)
        {
            LocalBlackboard.SetValue(BBKeys.TargetBuilding, building);
            //LocalBlackboard.SetValue(BBKeys.TargetPosition, building.GetWorkerEntranceWorldPos(null));
            OnTaskStarted?.Invoke(this);
        }
        public void FinishWorking()
        {
            currentTaskBuilding = null;
            OnTaskFinished?.Invoke(this);
        }

        public void OnAte()
        {
            Hunger = 0f;
            OnEatCompleted?.Invoke(this);
        }

    }

}