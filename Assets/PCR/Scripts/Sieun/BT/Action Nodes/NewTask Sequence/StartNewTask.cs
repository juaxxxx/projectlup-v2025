using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 3f;


        public override NodeState Evaluate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            if (building == null) return NodeState.FAILURE;

            OwnerAI?.StartWorkingAt(building);
            SetData(BBKeys.HasNewTask, false);
            return NodeState.SUCCESS;
        }
    }
}

