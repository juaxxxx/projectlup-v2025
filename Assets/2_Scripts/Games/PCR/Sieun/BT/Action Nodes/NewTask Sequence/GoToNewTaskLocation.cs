using UnityEngine;

namespace LUP.PCR
{

    public class GoToNewTaskLocation : MoveActionBase
    {
        public GoToNewTaskLocation(WorkerBlackboard bb) : base(bb) { }
        protected override string GetBuildingKey() => BBKeys.AssignedWorkplace;
    }
}