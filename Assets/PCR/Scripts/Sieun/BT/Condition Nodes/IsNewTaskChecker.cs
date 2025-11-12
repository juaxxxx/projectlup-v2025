using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : BTNode
    {
        Worker worker;
    
        public override WorkerNodeState Evaluate(WorkerAI worker)
        {
            Debug.Log("새 작업 명령 존재 여부 검사...");
            return worker.hasNewTask ? WorkerNodeState.SUCCESS : WorkerNodeState.FAILURE;
        }
    }
}