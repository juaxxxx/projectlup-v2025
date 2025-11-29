using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    // @TODO: 에셋 적용 시 [RequireComponent(typeof(CharacterController))] 추가
    public class UnitMover : MonoBehaviour
    {
        public Vector3 CurrentDestination => currentDestination;
        [SerializeField] AGridMap gridMap;
        [SerializeField] float moveSpeed = 5f;

        private APathfinding pathfinder;
        private List<ANode> path;
        private int currentIndex;
        private Vector3 currentDestination;

        void Start()
        {
            pathfinder = new APathfinding(gridMap);
        }

        void Update()
        {
            //@TODO : FindPath(RaycastHit hit) UI랑 연동되게 수정하기
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //    // 바닥과 충돌 검사
            //    if (Physics.Raycast(ray, out RaycastHit hit))
            //    {
            //        FindPath(hit);
            //    }
            //}
            MoveAlongPath();
        }

        //void FindPath(RaycastHit hit)
        //{
        //    ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
        //    ANode targetNode = gridMap.GetNodeFromWorldPosition(hit.point);
        //    path = pathfinder.FindPath(startNode, targetNode);

        //    gridMap.pathToDraw = path; // 경로 시각화용
        //    currentIndex = 0;
        //}

        public void SetDestination(Vector3 worldPos)
        {
            if (gridMap == null || pathfinder == null) return;

            ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
            ANode targetNode = gridMap.GetNodeFromWorldPosition(worldPos);

            if (targetNode == null || !targetNode.isWalkable)
            {
                //@TODO // 시스템메시지 UI 호출
                return;
            }

            List<ANode> calculatedPath = pathfinder.FindPath(startNode, targetNode);

            //@TODO
            //: 만약 경로를 못 찾았으면 목적지만이라도 설정할지, 멈출지 결정
            if (calculatedPath == null || calculatedPath.Count == 0)
            {
                currentDestination = worldPos;
                path = null;
            }
            else
            {
                ProcessPath(calculatedPath);
            }

            MoveAlongPath();
        }
        public void SetDestination(Vector2Int gridPos)
        {
            if (gridMap == null || pathfinder == null) return;

            // 시작점은 현재 유닛의 월드 위치를 기준으로 찾음

            ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
            ANode targetNode = gridMap.GetNodeFromGridPos(gridPos);

            if (targetNode == null || !targetNode.isWalkable)
            {
                //@TODO // 시스템메시지 UI 호출
                return;
            }

            List<ANode> calculatedPath = pathfinder.FindPath(startNode, targetNode);

            if (calculatedPath != null && calculatedPath.Count > 0)
            {
                ProcessPath(calculatedPath);
            }

            MoveAlongPath();
        }
        private void ProcessPath(List<ANode> newPath)
        {
            path = newPath;
            currentIndex = 0;

            gridMap.pathToDraw = path;

            currentDestination = gridMap.GetNodeWorldPosition(path[path.Count - 1]);
        }

        private void MoveAlongPath()
        {
            if (path == null || currentIndex >= path.Count) { return; }

            Vector3 targetPos = gridMap.GetNodeWorldPosition(path[currentIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                currentIndex++;
        }

        // BT - 목적지 도착 확인용
        public bool IsArrived()
        {
            if (path == null || currentIndex >= path.Count)
            {
                return Vector3.Distance(transform.position, currentDestination) < 0.2f;
            }

            return false;
        }
        public void Stop()
        {
            path = null;
            currentIndex = 0;
            gridMap.pathToDraw = null;
        }

    }
}