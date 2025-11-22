using UnityEngine;

namespace LUP.PCR
{
    public class GoToEatingPlace : WorkerBlackboardNode
    {
        public GoToEatingPlace(WorkerBlackboard blackboard) : base(blackboard) { }
        bool arrived = false;

        public override NodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("식당으로 이동 중...");
                //worker.MoveTo(worker.eatingSpot);

                //if (!worker.IsAt(worker.eatingSpot))
                    return NodeState.RUNNING;

                //arrived = true;
                //Debug.Log("식당 도착!");
            }
            return NodeState.SUCCESS;
        }
    }

}
