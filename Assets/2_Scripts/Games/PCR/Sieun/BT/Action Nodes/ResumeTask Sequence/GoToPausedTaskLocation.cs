using UnityEngine;

namespace LUP.PCR
{
    public class GoToPausedTaskLocation : WorkerBlackboardNode
    {
        public GoToPausedTaskLocation(WorkerBlackboard blackboard) : base(blackboard) { }
        bool started = false;
        Vector3 dest;

        public override NodeState Evaluate()
        {
            ProductableBuilding paused = GetData<ProductableBuilding>(BBKeys.TargetBuilding + "_paused");
            if (paused == null) return NodeState.FAILURE;
            // @TODO : 구조 확정되면 추가
            // dest = paused.GetWorkerEntranceWorldPos(null);
            if (Mover == null) return NodeState.FAILURE;

            if (!started)
            {
                Mover.SetDestination(dest);
                SetData(BBKeys.TargetPosition, dest);
                started = true;

                if (!Mover.IsArrived())
                {
                    Debug.Log("중단된 작업 위치로 이동 중...");
                }

                return NodeState.RUNNING;
            }

            Debug.Log("중단된 작업 위치 도착.");
            started = false;
            
            return NodeState.SUCCESS;
        }
    }

}


