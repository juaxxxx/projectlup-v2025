using UnityEngine;

namespace LUP.PCR
{
    public class GoToLounge : WorkerBlackboardNode
    {
        BuildingBase loungePlace;

        public GoToLounge(WorkerBlackboard blackboard) : base(blackboard) { }

        protected override void OnStart()
        {
            if (HasData(BBKeys.Lounge))
            {
                loungePlace = GetData<BuildingBase>(BBKeys.Lounge);
                
                if(loungePlace == null)
                {
                    Debug.Log("3. 라운지가 없습니다.");
                }
                else if (Mover != null)
                {
                    Mover.SetDestination(loungePlace.entrancePos); 
                    //SetData<Vector2Int>(BBKeys.TargetPosition, LoungeBuilding.entrancePos);
                }
            }
        }


        protected override NodeState OnUpdate()
        {
            if (Mover == null || loungePlace == null) { return NodeState.FAILURE; }
            {
                if (Mover.IsArrived())
                {
                    Debug.Log("3. 라운지 도착. 휴식 중...");

                    return NodeState.SUCCESS;
                }
                else
                {
                    Mover.MoveAlongPath();

                    Debug.Log("3. 라운지로 이동 중...");

                    return NodeState.RUNNING;
                }
            }
        }
    }
}
