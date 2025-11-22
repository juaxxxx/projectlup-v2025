using UnityEngine;

namespace LUP.PCR
{
    public class PauseCurrentTask : WorkerBlackboardNode
    {
        public PauseCurrentTask(WorkerBlackboard bb) : base(bb) { }

        public override NodeState Evaluate()
        {
            //RefreshCachedReferences();
            var building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            if (building == null) return NodeState.SUCCESS; // nothing to pause

            SetData(BBKeys.TargetBuilding + "_paused", building);
            SetData(BBKeys.HasPausedTask, true);

            BB.Remove(BBKeys.TargetPosition);
            return NodeState.SUCCESS;
        }
    }
}

