using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    // @TODO: 에셋 적용 시 [RequireComponent(typeof(CharacterController))] 추가
    public class UnitMover : MonoBehaviour
    {
        public Vector3 CurrentDestination => currentDestination;
        public bool IsMoving => path != null && currentIndex < path.Count;

        [SerializeField] public AGridMap gridMap;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotateSpeed = 10f;

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
            //MoveAlongPath();
        }

        //void FindPath(RaycastHit hit)
        //{
        //    ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
        //    ANode targetNode = gridMap.GetNodeFromWorldPosition(hit.point);
        //    path = pathfinder.FindPath(startNode, targetNode);

        //    gridMap.pathToDraw = path; // 경로 시각화용
        //    currentIndex = 0;
        //}

        //public void SetDestination(Vector3 worldPos)
        //{
        //    if (gridMap == null || pathfinder == null) return;

        //    ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
        //    ANode targetNode = gridMap.GetNodeFromWorldPosition(worldPos);

        //    List<ANode> calculatedPath = pathfinder.FindPath(startNode, targetNode);

        //    //@TODO : 만약 경로를 못 찾았으면 목적지만이라도 설정할지, 멈출지 결정
        //    if (calculatedPath == null || calculatedPath.Count == 0)
        //    {
        //        currentDestination = worldPos;
        //        path = null;
        //    }
        //    else
        //    {
        //        ProcessPath(calculatedPath);
        //    }
        //}

        ANode GetStartNodeByPhysics()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 2.0f))
            {
                return gridMap.GetNodeFromWorldPosition(hit.point);
            }
            // 공중에 떠 있거나 감지 실패시 기존 방식 사용
            return gridMap.GetNodeFromWorldPosition(transform.position);
        }

        public void SetDestination(Vector2Int gridPos)
        {
            if (gridMap == null || pathfinder == null) return;

            //ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
            ANode startNode = GetStartNodeByPhysics();
            
            if (!startNode.isWalkable)
            {
                startNode = FindNearestWalkableNode(startNode);
            }

            ANode targetNode = gridMap.GetNodeFromGridPos(gridPos);

            gridMap.debugStartNode = startNode;
            gridMap.debugTargetNode = targetNode;

            if (targetNode == null || !targetNode.isWalkable)
            {
                //@TODO // 시스템메시지 UI 호출
                Debug.LogError($"[UnitMover] 목표 지점({gridPos})은 갈 수 없는 곳입니다! (Null이거나 Wall)");
                return;
            }

            List<ANode> calculatedPath = pathfinder.FindPath(startNode, targetNode);

            if (calculatedPath != null && calculatedPath.Count > 0)
            {
                Debug.Log($"[UnitMover] 경로 찾기 성공! 노드 개수: {calculatedPath.Count}");
                ProcessPath(calculatedPath);
            }
            else
            {
                Debug.LogWarning($"[UnitMover] 경로를 찾을 수 없습니다. 시작점: {startNode.indexX},{startNode.indexY} / 목표점: {gridPos}");
            }

        }

        private ANode FindNearestWalkableNode(ANode centerNode)
        {
            if (centerNode.isWalkable) return centerNode;

            ANode nearest = null;
            float minDist = float.MaxValue;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector2Int neighborPos = new Vector2Int(centerNode.indexX + x, centerNode.indexY + y);
                    ANode neighbor = gridMap.GetNodeFromGridPos(neighborPos);

                    if (neighbor != null && neighbor.isWalkable)
                    {
                        float dist = Vector3.Distance(transform.position, neighbor.worldPos);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            nearest = neighbor;
                        }
                    }
                }
            }
            return nearest;
        }

        private void ProcessPath(List<ANode> newPath)
        {
            path = newPath;
            currentIndex = 0;

            gridMap.pathToDraw = path;

            currentDestination = gridMap.GetNodeFootPosition(path[path.Count - 1]);
        }

        public void MoveAlongPath()
        {
            if (!IsMoving) return;
            
            //Vector3 targetNodePos = gridMap.GetNodeWorldPosition(path[currentIndex]);
            Vector3 targetPos = gridMap.GetNodeFootPosition(path[currentIndex]);

            Vector3 direction = (targetPos - transform.position);
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }



            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                currentIndex++;
                // 도착하면 path가 null이 되거나 index가 초과되어 IsMoving이 자동으로 false가 됨
            }

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