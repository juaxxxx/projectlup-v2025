using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class UnitMover : MonoBehaviour
    {
        public Vector3 CurrentDestination => currentDestination;
        public bool IsMoving => path != null && currentIndex < path.Count;

        private AGridMap gridMap;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotateSpeed = 10f;

        private APathfinding pathfinder;
        private List<ANode> path;
        private int currentIndex;
        private Vector3 currentDestination;

        void Start()
        {
            gridMap = transform.root.GetComponentInChildren<AGridMap>();
            pathfinder = new APathfinding(gridMap);
        }

        //void Update()
        //{
        //    //@TODO : FindPath(RaycastHit hit) UI랑 연동되게 수정하기
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //        // 바닥과 충돌 검사
        //        if (Physics.Raycast(ray, out RaycastHit hit))
        //        {
        //            FindPath(hit);
        //        }
        //    }
        //    MoveAlongPath();
        //}

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

        public bool SetDestination(Vector2Int gridPos)
        {
            if (gridMap == null || pathfinder == null) return false;

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
                //Debug.LogError($"[UnitMover] 목표 지점({gridPos})은 갈 수 없는 곳입니다! (Null이거나 Wall)");
                return false;
            }

            List<ANode> calculatedPath = pathfinder.FindPath(startNode, targetNode);

            if (calculatedPath != null && calculatedPath.Count > 0)
            {
                //Debug.Log($"[UnitMover] 경로 찾기 성공! 노드 개수: {calculatedPath.Count}");
                ProcessPath(calculatedPath);
                return true;
            }
            else
            {
                return false;
                //Debug.LogWarning($"[UnitMover] 경로를 찾을 수 없습니다. 시작점: {startNode.indexX},{startNode.indexY} / 목표점: {gridPos}");
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
            if (!IsMoving) { return; }

            ANode targetNode = path[currentIndex];
            Vector3 targetPos = gridMap.GetNodeFootPosition(targetNode);

            bool isLadder = targetNode.isLadder;
            bool isElevator = targetNode.isElevator;
            bool isVerticalMove = isLadder || isElevator;

            Vector3 moveDir = (targetPos - transform.position);

            if (isVerticalMove)
            {
                // 중력 무시하고 타겟 방향으로 직진 (X축 정렬 후 Y축 이동)
                if (Mathf.Abs(moveDir.x) > 0.05f)
                {
                    moveDir.y = 0; // X축(좌우) 먼저 맞춤
                }
                else
                {
                    moveDir.x = 0; // X축 맞으면 위아래 이동
                }

                float currentSpeed = moveSpeed;

                if (isElevator)
                {
                    currentSpeed *= 2.5f; // 엘리베이터는 2.5배 빠름
                                          // @TODO : 엘리베이터 애니메이션 (가만히 서 있기)
                                          // anim.SetBool("IsClimbing", false); 
                }
                else
                {
                    // 사다리 애니메이션 (기어오르기)
                    // anim.SetBool("IsClimbing", true);
                }

                //transform.Translate(moveDir.normalized * currentSpeed * Time.deltaTime);
                transform.position += moveDir.normalized * currentSpeed * Time.deltaTime;
            }
            else
            {
                Vector3 walkTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                transform.position = Vector3.MoveTowards(transform.position, walkTarget, moveSpeed * Time.deltaTime);

                // 회전 처리
                Vector3 lookDir = walkTarget - transform.position;
                if (lookDir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed * Time.deltaTime);
                }

                // anim.SetBool("IsClimbing", false);
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                currentIndex++;
                // 도착하면 path가 null이 되거나 index가 초과되어 IsMoving이 자동으로 false가 됨
            }

            float dist = isVerticalMove ? Vector3.Distance(transform.position, targetPos)
                                        : Vector2.Distance(new Vector2(transform.position.x, transform.position.z)
                                        , new Vector2(targetPos.x, targetPos.z));

            if (dist < 0.1f)
            {
                currentIndex++;
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