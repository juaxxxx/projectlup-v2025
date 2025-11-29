using UnityEngine;

namespace LUP.PCR
{
    public class ResumePausedTask : WorkerBlackboardNode
    {
        public ResumePausedTask(WorkerBlackboard blackboard) : base(blackboard) { }

        float timer = 0f;
        float duration = 2f;

        public override NodeState Evaluate()
        {
            ProductableBuilding paused = GetData<ProductableBuilding>(BBKeys.TargetBuilding + "_paused");
            if (paused == null) return NodeState.FAILURE;

            OwnerAI?.StartWorkingAt(paused);
            
            BB.Remove(BBKeys.TargetBuilding + "_paused");
            SetData(BBKeys.HasPausedTask, false);
            return NodeState.SUCCESS;
        }
    }
}

