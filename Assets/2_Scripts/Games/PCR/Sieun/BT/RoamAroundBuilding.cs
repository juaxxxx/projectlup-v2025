using UnityEngine;

namespace LUP.PCR
{ 

    public class RoamAroundBuilding : WorkerBlackboardNode
    {
        private BuildingBase targetBuilding;
        private float waitTimer = 0f;
        private float waitDuration = 2f;
        private bool isWaiting = false;
    
        public RoamAroundBuilding(WorkerBlackboard bb) : base(bb) { }
    
        protected override NodeState OnUpdate()
        {
            if (!HasData(BBKeys.WorkerStation)) return NodeState.FAILURE;
            targetBuilding = GetData<BuildingBase>(BBKeys.WorkerStation);

            if (targetBuilding == null) return NodeState.FAILURE;

            if (isWaiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    isWaiting = false;
                    waitTimer = 0f;

                    SetNewRandomDestination();
                }
                return NodeState.RUNNING;
            }

            if (Mover.IsMoving)
            {
                Mover.MoveAlongPath();
                return NodeState.RUNNING;
            }
            else
            {
                // 도착했다면 대기 모드로 전환
                isWaiting = true;

                waitDuration = Random.Range(1.0f, 3.0f);
                return NodeState.RUNNING;
            }
        }
        
        private void SetNewRandomDestination()
        {
            Vector2Int center = targetBuilding.entrancePos;
            int radius = 3;

            for (int i = 0; i < 10; i++)
            {
                int randomX = Random.Range(-radius, radius + 1);
                int randomY = 0;

                // Mover 안에 있는 GridMap을 통해 갈 수 있는지 체크
                //(SetDestination 내부에서 유효성 검사를 하거나, 여기서 미리 체크)
                Vector2Int randomPos = new Vector2Int(center.x + randomX, center.y + randomY);
                
                if(Mover.SetDestination(randomPos))
                {
                    return;
                }
            }

        }
    }
}