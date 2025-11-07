using PCR;
using UnityEngine;
using static PCR.BTNode;

namespace PCR
{ 
    public class IsNewTaskChecker : BTNode
    {
        Worker worker;
        public IsNewTaskChecker(Worker worker) { this.worker = worker; }
    
        public override WorkerNodeState Evaluate()
        {
            Debug.Log("새 작업 명령 존재 여부 검사...");
            return worker.hasNewTask ? WorkerNodeState.SUCCESS : WorkerNodeState.FAILURE;
        }
    }
}