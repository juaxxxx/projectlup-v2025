using UnityEngine;

namespace LUP.PCR
{
    public class IsHealthLowChecker : BTNode
    {
        public WorkerStatus Status { get; private set; } = new WorkerStatus();
        Worker worker;
        
        int logLoopCount = -1;

        public override WorkerNodeState Evaluate(WorkerAI worker)
        {
            if (worker.hunger >= 0.7f)
            {
                Debug.Log("배고픔 감지됨.");
                return WorkerNodeState.SUCCESS;
            }

            if(logLoopCount == 0)
            {
                Debug.Log("아직 배고프지 않음.");
                logLoopCount += 1;
            }
            return WorkerNodeState.FAILURE;
        }
    }


}

