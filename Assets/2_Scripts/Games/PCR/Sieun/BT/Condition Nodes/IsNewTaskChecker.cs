using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : WorkerBlackboardNode
    {
        public IsNewTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }
        public override NodeState Evaluate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            return building != null && OwnerAI != null && OwnerAI.HasNewTask ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}