using UnityEngine;

namespace LUP.PCR
{
    public abstract class MoveActionBase : WorkerBlackboardNode
    {
        protected BuildingBase targetBuilding;
        private Vector2Int lastEntrancePos = new Vector2Int(-999, -999);
        
        public MoveActionBase(WorkerBlackboard bb) : base(bb) { }
        protected abstract string GetBuildingKey();

        protected override void OnStart()
        {
            if(HasData(GetBuildingKey()))
            {
                targetBuilding = GetData<BuildingBase>(GetBuildingKey());

                // OnStart에서는 강제로 경로를 한번 계산
                if (targetBuilding != null)
                {
                    UpdatePath();
                }
            }
        }

        protected override NodeState OnUpdate()
        {
            if (Mover == null || targetBuilding == null)
            {
                return NodeState.FAILURE;
            }

            if (targetBuilding.entrancePos != lastEntrancePos)
            {
                Debug.Log($"[재경로] {targetBuilding.buildingName} 위치 변경! ({lastEntrancePos} -> {targetBuilding.entrancePos})");
                UpdatePath();
            }

            if (Mover.IsArrived())
            {
                return NodeState.SUCCESS;
            }
            else
            {
                Debug.Log($"{targetBuilding.buildingName}으로 이동 중---! ");

                Mover.MoveAlongPath();
                return NodeState.RUNNING;
            }
        }

        // 경로 계산 및 캐싱 함수
        private void UpdatePath()
        {
            lastEntrancePos = targetBuilding.entrancePos;

            Mover.SetDestination(lastEntrancePos);
        }

    }
}

