using UnityEngine;

namespace LUP.PCR
{
    public class PauseCurrentTask : WorkerBlackboardNode
    {
        public PauseCurrentTask(WorkerBlackboard bb) : base(bb) { }

        public override NodeState Evaluate()
        {
            //RefreshCachedReferences();
            bool isWorking = GetData<bool>(BBKeys.IsWorking);
            if (isWorking)
                return NodeState.SUCCESS;

            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            if (building == null) return NodeState.SUCCESS; // nothing to pause
            
            SetData(BBKeys.TargetBuilding + "_paused", building);
            SetData(BBKeys.HasPausedTask, true);

            SetData(BBKeys.IsWorking, false);
            BB.Remove(BBKeys.TargetPosition);

            return NodeState.SUCCESS;
        }
    }
}

