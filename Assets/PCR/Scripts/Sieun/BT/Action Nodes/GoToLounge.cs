using UnityEngine;

namespace LUP.PCR
{
    public class GoToLounge : BTNode
    {
        Worker worker;
        bool arrived = false;

        public GoToLounge(Worker worker) { this.worker = worker; }

        public override WorkerNodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("라운지로 이동 중...");
                worker.MoveTo(worker.loungeSpot);

                if (!worker.IsAt(worker.loungeSpot))
                    return WorkerNodeState.RUNNING;

                arrived = true;
                Debug.Log("라운지 도착. 휴식 중...");
            }

            return WorkerNodeState.RUNNING; // 계속 대기
        }
    }


}
