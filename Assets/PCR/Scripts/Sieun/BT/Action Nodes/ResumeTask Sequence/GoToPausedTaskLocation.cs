using UnityEngine;

namespace LUP.PCR
{
    public class GoToPausedTaskLocation : BTNode
    {
        Worker worker;
        bool arrived = false;

        public GoToPausedTaskLocation(Worker worker) { this.worker = worker; }

        public override WorkerNodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("중단된 작업 위치로 이동 중...");
                worker.MoveTo(worker.pausedWorkSpot);


                if (!worker.IsAt(worker.pausedWorkSpot))
                    return WorkerNodeState.RUNNING;

                arrived = true;
                Debug.Log("중단된 작업 위치 도착.");
            }
            return WorkerNodeState.SUCCESS;
        }
    }

}


