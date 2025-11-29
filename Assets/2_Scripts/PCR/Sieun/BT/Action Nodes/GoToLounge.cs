using UnityEngine;

namespace LUP.PCR
{
    public class GoToLounge : WorkerBlackboardNode
    {
        public GoToLounge(WorkerBlackboard blackboard) : base(blackboard) { }
        bool started = false;
        Vector2Int loungePos;

        public override NodeState Evaluate()
        {
            if (Mover == null) return NodeState.FAILURE;

            if (!started)
            {
                //@TODO : 구조 확정되면 라운지 위치 지정하기
                //Mover.MoveTo(loungePos);
                Mover.SetDestination(loungePos);
                //SetData(BBKeys.TargetPosition, loungePos);
                started = true;

                if (!Mover.IsArrived())
                {
                    Debug.Log("라운지로 이동 중...");
                }

                return NodeState.RUNNING;
            }

            Debug.Log("라운지 도착. 휴식 중...");
            started = false;
            return NodeState.SUCCESS;
        }
    }
}
