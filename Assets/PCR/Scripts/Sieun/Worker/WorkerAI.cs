using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    //RequireComponent(typeof(Animator))]
    public class WorkerAI : MonoBehaviour
    {
        BTNode root;
        Worker worker;

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
             new IsHealthLowChecker(worker),
             new GoToEatingPlace(worker),
             new EatFood(worker),
             new GoBackToOriginPosition(worker)
         });

            // Sequence: 하던 일 재개
            BTNode resumeTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsPausedTaskChecker(worker),
            new GoToPausedTaskLocation(worker),
            new ResumePausedTask(worker)
        });

            // Sequence: 새 일 시작
            BTNode newTaskSequence = new SequenceNode(new List<BTNode>
        {
            new IsNewTaskChecker(worker),
            new GoToNewTaskLocation(worker),
            new StartNewTask(worker)
        });

            // Selector: 작업/휴식
            BTNode taskSelector = new SelectorNode(new List<BTNode>
           {
               resumeTaskSequence,
               newTaskSequence,
               new GoToLounge(worker)
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
            root.Evaluate();

        }


    }

}