using UnityEngine;

namespace LUP.PCR
{
    public class IsPausedTaskChecker : WorkerBlackboardNode
    {
        public IsPausedTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }

        public override NodeState Evaluate()
        {
            ProductableBuilding paused = GetData<ProductableBuilding>(BBKeys.TargetBuilding + "_paused");
            
            Debug.Log("중단된 작업 존재 여부 검사...");
            return paused != null ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
