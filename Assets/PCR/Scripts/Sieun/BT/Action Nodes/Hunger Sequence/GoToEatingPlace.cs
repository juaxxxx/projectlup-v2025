using UnityEngine;

namespace LUP.PCR
{
    public class GoToEatingPlace : BTNode
    {
        Worker worker;
        bool arrived = false;


        public override WorkerNodeState Evaluate(WorkerAI worker)
        {
            if (!arrived)
            {
                Debug.Log("식당으로 이동 중...");
                //worker.MoveTo(worker.eatingSpot);

                //if (!worker.IsAt(worker.eatingSpot))
                    return WorkerNodeState.RUNNING;

                //arrived = true;
                //Debug.Log("식당 도착!");
            }
            return WorkerNodeState.SUCCESS;
        }
    }

}
