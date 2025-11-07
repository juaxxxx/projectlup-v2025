using UnityEngine;

namespace PCR
{

    public class GoToNewTaskLocation : BTNode
    {
        Worker worker;
        bool arrived = false;
    
        public GoToNewTaskLocation(Worker worker) { this.worker = worker; }
    
        public override WorkerNodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("새 작업지로 이동 중...");
                worker.MoveTo(worker.newTaskSpot);
    
                if (!worker.IsAt(worker.newTaskSpot))
                    return WorkerNodeState.RUNNING;
    
                arrived = true;
                Debug.Log("새 작업지 도착.");
            }
            return WorkerNodeState.SUCCESS;
        }
    }
}