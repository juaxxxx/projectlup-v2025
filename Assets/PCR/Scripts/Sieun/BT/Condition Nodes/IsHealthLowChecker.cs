using PCR;
using UnityEngine;

namespace PCR
{
    public class IsHealthLowChecker : BTNode
    {
        public WorkerStatus Status { get; private set; } = new WorkerStatus();
        Worker worker;
        public IsHealthLowChecker(Worker worker) { this.worker = worker; }
        public override WorkerNodeState Evaluate()
        {
            if (worker.hunger >= 0.7f)
            {
                Debug.Log("배고픔 감지됨.");
                return WorkerNodeState.SUCCESS;
            }
            Debug.Log("아직 배고프지 않음.");
            return WorkerNodeState.FAILURE;
        }
    }


}

