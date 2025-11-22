using UnityEngine;

namespace LUP.PCR
{

    public class GoToNewTaskLocation : WorkerBlackboardNode
    {
        public GoToNewTaskLocation(WorkerBlackboard blackboard) : base(blackboard) { }
        bool arrived = false;
    
        public override NodeState Evaluate()
        {
            if (!arrived)
            {
                //Debug.Log("새 작업지로 이동 중...");
                //worker.MoveTo(worker.newTaskSpot);
    
                //if (!worker.IsAt(worker.newTaskSpot))
                    return NodeState.RUNNING;
    
                arrived = true;
                Debug.Log("새 작업지 도착.");
            }
            return NodeState.SUCCESS;
        }
    }
}