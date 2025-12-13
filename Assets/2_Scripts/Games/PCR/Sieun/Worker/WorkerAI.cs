using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

namespace LUP.PCR
{
    //[System.Serializable]
    //public class WorkerProfile
    //{
    //}






    [RequireComponent(typeof(Worker))]
    [RequireComponent(typeof(UnitMover))]
    public class WorkerAI : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private float hunger = 0;
        private bool isHunger = false;
        private bool hasTask = false; 

        [Header("Component")]
        private Worker worker;
        private UnitMover mover;
        private BTNode root;

        private BuildingBase currentTaskBuilding = null;

        private void OnEnable()
        {
            WorkerDataCenter dataCenter = this.transform.root.GetComponent<WorkerDataCenter>();

            if(dataCenter != null)
            {
                dataCenter.RegisterWorker(this);
            }
        }


        //@TODO : restaurant든 station이든 건물 위치는 받아와서 처리하기
        public void SetGlobalBuildings(BuildingBase restaurant, BuildingBase station)
        {
            // 건물 생성되는 시점부터 자동으로 초기화될 위치 : 식당, 작업 스테이션
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.Restaurant, restaurant);
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.WorkerStation, station);

            currentTaskBuilding = station;
            LocalBlackboard.SetValue<BuildingBase>(BBKeys.AssignedWorkplace, currentTaskBuilding);
        }

        //@TODO: BuildingSystem에 있는 실제 currBuildings 및 건물타입ID로 건물 조회해서 entrancePos 접근하기.
        // 지금은 임시로 건물 프리팹 자체에서 직접 entrancePos 를 가져온다.

        public WorkerBlackboard LocalBlackboard { get; private set; }
        public float LastWorkEndTime { get; private set; } = 0f;

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

        public bool HasTask
        {
            get => hasTask;
            set
            {
                hasTask = value;
                LocalBlackboard.SetValue(BBKeys.HasTask, hasTask);

                if (value == false)
                {
                    LastWorkEndTime = Time.time;
                }

            }
        }

        public void InitWorkerData(int id, string name)
        {
            //profile = new WorkerProfile { id = id, workerName = name };
            //this.name = $"Worker_{name}"; // 오브젝트 이름 변경
            LastWorkEndTime = Time.time;   // 게임 시작 시점 or 일 끝난 시점 기록
        }
        
        public void InitBTReferences()
        {
            worker = GetComponent<Worker>();
            mover = GetComponent<UnitMover>();
            LocalBlackboard = new WorkerBlackboard();

            InitBlackboard();
            CheckHungerState();
            SettingBT();
            GetComponentInChildren<WorkerOverlayUI>().Setup(this);
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
            LocalBlackboard.SetValue(BBKeys.HasTask, hasTask);
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
            //new GoToWorkerStation(LocalBlackboard),
            new RoamAroundBuilding(LocalBlackboard),
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
        }

        public void AssignTask(BuildingBase building)
        {
            currentTaskBuilding = building;
            LocalBlackboard.SetValue(BBKeys.AssignedWorkplace, currentTaskBuilding);
            
            hasTask = true;
            LocalBlackboard.SetValue(BBKeys.HasTask, hasTask);
        }
    }

}