using UnityEngine;

namespace LUP.PCR
{

    public class ReturnToPausedTask : WorkerBlackboardNode
    {
        public ReturnToPausedTask(WorkerBlackboard bb) : base(bb) { }

        public override NodeState Evaluate()
        {
            RefreshCachedReferences();

            // Check paused building
            if (!HasData(BBKeys.TargetBuilding + "_paused")) return NodeState.FAILURE;
            var paused = GetData<ProductableBuilding>(BBKeys.TargetBuilding + "_paused");
            if (paused == null) return NodeState.FAILURE;

            SetData(BBKeys.TargetBuilding, paused);
            SetData(BBKeys.HasPausedTask, false);
            BB.Remove(BBKeys.TargetBuilding + "_paused");

            //@TODO : 구조 확정되면 추가
            //SetData(BBKeys.TargetPosition, paused.GetWorkerEntranceWorldPos(null));
            OwnerAI?.StartWorkingAt(paused);

            return NodeState.SUCCESS;
        }
    }

}

