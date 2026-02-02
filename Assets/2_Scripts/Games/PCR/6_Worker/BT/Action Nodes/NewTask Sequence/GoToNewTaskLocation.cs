using UnityEngine;

namespace LUP.PCR
{
    public class GoToNewTaskLocation : MoveActionBase
    {
        public GoToNewTaskLocation(WorkerBlackboard bb) : base(bb) { }

        // 부모에게 "어디로 갈지" 알려주는 키
        protected override string GetBuildingKey() => BBKeys.AssignedWorkplace;

        protected override NodeState OnUpdate()
        {
            if (!OwnerAI.HasTask)
            {
                OwnerAI.HasTask = true;
            }

            if (targetPlace == null || !targetPlace.IsWorkRequested)
            {
                if (targetPlace != null)
                {
                    targetPlace.SetWorker(null);
                }

                if (Mover != null)
                {
                    Mover.Stop();
                }

                BB.Remove(BBKeys.AssignedWorkplace);
                OwnerAI.HasTask = false;

                return NodeState.FAILURE;
            }

            return base.OnUpdate();
        }
    }
}