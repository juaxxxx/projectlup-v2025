using UnityEngine;

namespace LUP.PCR
{
    public class GoToEatingPlace : WorkerBlackboardNode
    {
        public GoToEatingPlace(WorkerBlackboard blackboard) : base(blackboard) { }

        public override NodeState Evaluate()
        {
            UnitMover mover = GetData<UnitMover>(BBKeys.UnitMover);
            Vector3 eatingPos = GetData<Vector3>(BBKeys.TargetPosition);

            if (eatingPos == null || mover == null)
            {
                return NodeState.FAILURE;
            }
  
            bool isArrived = mover.IsArrived();
            if (isArrived)
            {
                return NodeState.SUCCESS;
            }
            
            mover.SetDestination(eatingPos);
            Debug.Log("식당으로 이동 중...");
            return NodeState.RUNNING;
        }
    }

}
