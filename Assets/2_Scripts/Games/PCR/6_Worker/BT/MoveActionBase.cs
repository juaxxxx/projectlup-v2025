using UnityEngine;

namespace LUP.PCR
{
    public abstract class MoveActionBase : WorkerBlackboardNode
    {
        protected StructureBase targetPlace;
        private Vector2Int lastEntrancePos = new Vector2Int(-999, -999);
        
        public MoveActionBase(WorkerBlackboard bb) : base(bb) { }
        protected abstract string GetBuildingKey();

        protected override void OnStart()
        {
            if(HasData(GetBuildingKey()))
            {
                targetPlace = GetData<BuildingBase>(GetBuildingKey());

                if (targetPlace != null)
                {
                    UpdatePath();
                }
            }
        }

        protected override NodeState OnUpdate()
        {
            if (Mover == null || targetPlace == null)
            {
                return NodeState.FAILURE;
            }

            if (targetPlace.entrancePos != lastEntrancePos)
            {
                UpdatePath();
            }

            if (targetPlace.workSpotAnchor != null)
            {
                // 내 위치와 최종 작업 위치(WorkSpot) 사이의 거리 계산 (높이 무시)
                Vector3 myPos = new Vector3(Mover.transform.position.x, 0, Mover.transform.position.z);
                Vector3 goalPos = new Vector3(targetPlace.WorkSpotWorldPos.x, 0, targetPlace.WorkSpotWorldPos.z);

                if (Vector3.Distance(myPos, goalPos) < 0.5f)
                {
                    return NodeState.RUNNING;
                }
            }

            if (Mover.HasInternalPath())
            {
                bool isFinished = Mover.MoveInternal();

                if (isFinished)
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.RUNNING;
                }

            }

            if (!Mover.IsArrived())
            {
                Mover.MoveAlongPath();
                return NodeState.RUNNING;
            }

            if (targetPlace.workSpotAnchor != null)
            {
                Mover.SetInternalPath(targetPlace);
                Mover.MoveInternal();
                return NodeState.RUNNING;
            }
            return NodeState.SUCCESS; // 내부에 갈 곳 없으면 입구까지만 이동

        }

        // 경로 계산 및 캐싱 함수
        private void UpdatePath()
        {
            lastEntrancePos = targetPlace.entrancePos;

            Mover.SetDestination(lastEntrancePos);
        }

    }
}

