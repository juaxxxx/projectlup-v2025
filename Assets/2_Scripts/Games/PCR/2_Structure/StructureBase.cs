using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public abstract class StructureBase : MonoBehaviour
    {
        public string placeName;
        public Vector2Int entrancePos;
        
        [Header("Path Settings")]
        public Transform entranceAnchor;
        public Transform workSpotAnchor;

        public List<Vector3> localWaypoints = new List<Vector3>();
        public Vector3 WorkSpotWorldPos => workSpotAnchor != null ? workSpotAnchor.position : transform.position;

        protected bool hasWork;
        public bool IsWorkRequested { get; protected set; } = false; // 작업 요청 여부
        public WorkerAI AssignedWorker { get; protected set; } // 배정된 작업자
        // @TODO : AssignedWorker를 안 쓰는 게 더 나은지 고민해보기
        public void SetupEntranceByAnchor(AGridMap gridMap)
        {
            if (entranceAnchor != null && gridMap != null)
            {
                ANode node = gridMap.GetNodeFromWorldPosition(entranceAnchor.position);
                if (node != null)
                {
                    entrancePos = new Vector2Int(node.indexX, node.indexY);
                    return;
                }
            }
        }

        public void ToggleWorkRequest()
        {
            IsWorkRequested = !IsWorkRequested;

            if (IsWorkRequested)
            {
                if (WorkerSystem.Instance != null)
                {
                    WorkerSystem.Instance.RegisterTask(this);
                }
            }
            else
            {
                // 만약 이미 배정된 작업자가 있다면 ReleaseWorker()를 호출할 수도 있음
            }
        }
        public virtual void SetWorker(WorkerAI worker)
        {
            this.AssignedWorker = worker;
        }

        // 작업자 해제 (내보내거나 떠날 때)
        public virtual void ReleaseWorker()
        {
            if (AssignedWorker != null)
            {
                AssignedWorker.HasTask = false;
                AssignedWorker = null;
            }

            ExitWorker();

            if (IsWorkRequested && WorkerSystem.Instance != null)
            {
                WorkerSystem.Instance.RegisterTask(this);
            }
            else
            {
                // 예약목록에서 해제 (작업요청 토글 Off)
            }
        }

        public void EnterWorker()
        {
            hasWork = true;
        }

        public void ExitWorker()
        {
            hasWork = false;
        }
        public bool HasWorker() => AssignedWorker != null;

        private void OnDrawGizmosSelected()
        {
            if (entranceAnchor == null || workSpotAnchor == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            // 시작점: 입구
            Vector3 prevPos = entranceAnchor.position;

            foreach (Vector3 localPoint in localWaypoints)
            {
                // 로컬 좌표 -> 월드 좌표 변환
                Vector3 worldPoint = transform.TransformPoint(localPoint);

                Gizmos.DrawSphere(worldPoint, 0.3f); // 점 찍기
                Gizmos.DrawLine(prevPos, worldPoint); // 선 잇기
                prevPos = worldPoint;
            }

            // 끝점: 작업 위치
            Gizmos.DrawLine(prevPos, workSpotAnchor.position);
        }
    }
}
