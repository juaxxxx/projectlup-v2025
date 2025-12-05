using System;
using System.Collections.Generic;

using UnityEngine;

namespace LUP.PCR
{
    [RequireComponent(typeof(Worker))]
    [RequireComponent(typeof(UnitMover))]
    public class WorkerAI : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private float hunger = 0;
        private bool isHunger = false;

        //@TODO : 일하고 있는 상태를 어떤식으로 정의할지 고민하기.
        private bool isWorking = true; 
        private bool hasNewTask = false;

        [Header("BT Time")]
        private float btTickInterval = 0.1f;
        private float btTimer = 0f;

        [Header("Component")]
        private Worker worker;
        private UnitMover mover;
        private BTNode root;

        //@TODO: BuildingSystem에 있는 실제 currBuildings 및 건물타입ID로 건물 조회해서 entrancePos 접근하기.
        // 지금은 임시로 건물 프리팹 자체에서 직접 entrancePos 를 가져온다.
        [Header("Task")]
        private ProductableBuilding currentTaskBuilding = null;
        [SerializeField] private ProductableBuilding restaurantBuilding = null;
        [SerializeField] private ProductableBuilding loungeBuilding = null;

        // 
        public WorkerBlackboard LocalBlackboard { get; private set; }
        public float Hunger
        {
            get => hunger;
            set
            {
                hunger = value;
                LocalBlackboard.SetValue(BBKeys.Hunger, hunger);

                CheckHungerState();
            }
        }
        private void CheckHungerState()
        {
            bool shouldBeHungry = hunger >= HungerRules.HungryThreshold;

            if (isHunger != shouldBeHungry)
            {
                IsHunger = shouldBeHungry;
            }
        }

        public bool IsHunger
        {
            get => isHunger;
            set
            {
                isHunger = value;
                LocalBlackboard.SetValue(BBKeys.IsHunger, isHunger);
            }
        }

        

        public bool IsWorking
        {
            get => isWorking;
            set
            {
                isWorking = value;
                LocalBlackboard.SetValue(BBKeys.IsWorking, isWorking);
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

        public void InitBTReferences()
        {
            worker = GetComponent<Worker>();
            mover = GetComponent<UnitMover>();
            LocalBlackboard = new WorkerBlackboard();

            InitBlackboard();

            CheckHungerState();
            
            SettingBT();
        }

        private void InitBlackboard()
        {
            //정적 데이터(참조) 등록
            LocalBlackboard.SetValue(BBKeys.OwnerAI, this);
            LocalBlackboard.SetValue(BBKeys.Self, worker);
            LocalBlackboard.SetValue(BBKeys.UnitMover, mover);

            // BT 상태 초기화
            LocalBlackboard.SetValue(BBKeys.Hunger, hunger);
            LocalBlackboard.SetValue(BBKeys.IsHunger, IsHunger);

            // 건물 생성되는 시점부터 자동으로 초기화될 위치 : 식당, 라운지
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.Restaurant, restaurantBuilding);
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.Lounge, loungeBuilding);

            LocalBlackboard.SetValue<Vector2Int>(BBKeys.TargetPosition, restaurantBuilding.entrancePos);
            LocalBlackboard.SetValue<Vector2Int>(BBKeys.TargetPosition, loungeBuilding.entrancePos);

            // @TODO : currentTaskBuilding을 받을 AssignTask()를 어디서 호출하게 할지 생각하기
            // 워커 시작 위치 : 라운지
            currentTaskBuilding = loungeBuilding;
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.TargetBuilding, currentTaskBuilding); 
            LocalBlackboard.SetValue<Vector2Int>(BBKeys.TargetPosition, currentTaskBuilding.entrancePos);

            LocalBlackboard.SetValue(BBKeys.HasNewTask, hasNewTask);
            LocalBlackboard.SetValue(BBKeys.IsWorking, isWorking);

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
             new EatFood(LocalBlackboard),
         });

        // Sequence: 새 일 시작
        BTNode workingSequence = new SequenceNode(new List<BTNode>
        {
            new IsNewTaskChecker(LocalBlackboard),
            new GoToNewTaskLocation(LocalBlackboard),
            new StartNewTask(LocalBlackboard)
        });

        // Root Selector: 배고픔 → 작업/휴식
        root = new SelectorNode(new List<BTNode>
        {
            hungerSequence,
            workingSequence,
            new GoToLounge(LocalBlackboard)
        });
        }

        public void UpdateBT()
        {
            if (root == null) return;
            root?.Evaluate();

            if(!isHunger)
            {
                // 배고프게 만들기
                Hunger = Mathf.Clamp(hunger + Time.deltaTime * 0.1f, 0, 3);
            }

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
            //}
        }

        //@TODO : AssignTask()를 어디서 어떻게 호출하게 할지 생각하기
        // 지금은 임시로 버튼UI OnClick(미리 오브젝트 자체를 지정)으로 건물 위치가 지정되게 했다.
        public void AssignTask(ProductableBuilding building)
        {
           // CancelOrReplaceCurrentTask();

            currentTaskBuilding = building;
            HasNewTask = true;
            LocalBlackboard.SetValue(BBKeys.HasNewTask, true);
            LocalBlackboard.SetValue(BBKeys.TargetBuilding, currentTaskBuilding);
        }
        //private void CancelOrReplaceCurrentTask()
        //{
        //    if (currentTaskBuilding != null)
        //    {
        //        currentTaskBuilding = null;
        //    }
        //    LocalBlackboard.Remove(BBKeys.TargetBuilding);
        //    LocalBlackboard.Remove(BBKeys.TargetPosition);
        //    IsWorking = false;
        //    HasNewTask = false;
        //}
    }

}