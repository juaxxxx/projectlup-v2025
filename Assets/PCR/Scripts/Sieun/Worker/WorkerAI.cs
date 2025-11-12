using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    //RequireComponent(typeof(Animator))]
    public class WorkerAI : MonoBehaviour
    {
        public event Action<WorkerAI> OnEatCompleted;    //
                                                         //[밥 다 먹었다!]
        public event Action<WorkerAI> OnTaskStarted;     // [작업 시작]
        public event Action<WorkerAI> OnTaskFinished;    // [작업 완료]
        BTNode root;
        Worker worker;
        
        public float hunger = 0.5f;

        // bool isWorking, bool isEating 등은 State 클래스에서 가져오게 할까 고민중
        // hasNewTask, isTaskPaused 등도 State 클래스에서 가져오게 할까 고민중
        public bool isWorking = false; 
        public bool isEating = false;
        public bool hasNewTask = false;
        public bool hasPausedWork = false;

        public Vector3 originSpot;

        public void StartTask()

        {
            isWorking = true;
            OnTaskStarted?.Invoke(this);

        }



        public void FinishTask()

        {
            isWorking = false;
            OnTaskFinished?.Invoke(this);

        }



        private void Start()
        {
            worker = GetComponent<Worker>();
            SettingBT();
        }
        void SettingBT()
        {
            // Sequence: 배고픔 처리
            BTNode hungerSequence = new SequenceNode(new List<BTNode>
         {
             new IsHealthLowChecker(),
             new GoToEatingPlace(),
             new EatFood(),
            // new GoBackToOriginPosition()
         });

            // Sequence: 하던 일 재개
            BTNode resumeTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsPausedTaskChecker(),
            new GoToPausedTaskLocation(),
            new ResumePausedTask()
        });

            // Sequence: 새 일 시작
            BTNode newTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsNewTaskChecker(),
            new GoToNewTaskLocation(),
            new StartNewTask()
        });

            // Selector: 작업/휴식
            BTNode taskSelector = new SelectorNode(new List<BTNode>
           {
               resumeTaskSequence,
               newTaskSequence,
               new GoToLounge()
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
            root.Evaluate(this);
        }



        //public void MoveTo(TileInfo buiding)
        //{
        //   // transform.position = Vector3.MoveTowards(transform.position, tile.pos, Time.deltaTime * 2f);
        //}

        //public bool IsAt(BuildingData tile)
        //{
        //    //return Vector3.Distance(transform.position, tile.place) < 0.1f;
        //}

    }

}