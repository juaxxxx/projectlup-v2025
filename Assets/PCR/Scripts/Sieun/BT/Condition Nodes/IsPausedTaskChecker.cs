using UnityEngine;

namespace LUP.PCR
{
    public class IsPausedTaskChecker : BTNode
    {
        Worker worker;
        public IsPausedTaskChecker(Worker worker) { this.worker = worker; }
    
        public override WorkerNodeState Evaluate()
        {
            Debug.Log("중단된 작업 존재 여부 검사...");
            return worker.hasPausedWork ? WorkerNodeState.SUCCESS : WorkerNodeState.FAILURE;
        }
    }
}
