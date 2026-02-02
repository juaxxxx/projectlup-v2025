using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard bb) : base(bb) { }
        
        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.AssignedWorkplace);
            
            if (building == null)
            {
                return NodeState.FAILURE;
            }

            building.EnterWorker();

            return NodeState.SUCCESS;
        }
    }
}