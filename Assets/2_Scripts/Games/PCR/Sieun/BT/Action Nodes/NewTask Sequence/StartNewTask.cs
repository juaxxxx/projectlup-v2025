using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 3f;

        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            
            if (building == null)
            {
                return NodeState.FAILURE;
            }
            else
            {
                SetData(BBKeys.HasNewTask, false);
                SetData(BBKeys.IsWorking, true);
                return NodeState.SUCCESS;
            }
        }
    }
}

