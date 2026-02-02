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
                Debug.Log($"[재경로] {targetPlace.placeName} 위치 변경! ({lastEntrancePos} -> {targetPlace.entrancePos})");
                UpdatePath();
            }

            if (Mover.IsArrived())
            {
                return NodeState.SUCCESS;
            }
            else
            {
                Debug.Log($"{targetPlace.placeName}으로 이동 중---! ");

                Mover.MoveAlongPath();
                return NodeState.RUNNING;
            }
        }

        // 경로 계산 및 캐싱 함수
        private void UpdatePath()
        {
            lastEntrancePos = targetPlace.entrancePos;

            Mover.SetDestination(lastEntrancePos);
        }

    }
}

