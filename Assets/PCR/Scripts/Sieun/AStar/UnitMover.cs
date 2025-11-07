using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PCR
{
    public class UnitMover : MonoBehaviour
    {
        [SerializeField] AGridMap gridMap;
        [SerializeField] float moveSpeed = 5f;

        APathfinding pathfinder;
        List<ANode> path;
        int currentIndex;

        void Start()
        {
            pathfinder = new APathfinding(gridMap);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // 바닥과 충돌 검사
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    FindPath(hit);
                }
            }
            MoveAlongPath();
        }

        void FindPath(RaycastHit hit)
        {
            ANode startNode = gridMap.GetNodeFromWorldPosition(transform.position);
            ANode targetNode = gridMap.GetNodeFromWorldPosition(hit.point);
            path = pathfinder.FindPath(startNode, targetNode);

            gridMap.pathToDraw = path; // 경로 시각화용
            currentIndex = 0;
        }


        void MoveAlongPath()
        {
            if (path == null || currentIndex >= path.Count)
                return;

            Vector3 targetPos = gridMap.GetNodeWorldPosition(path[currentIndex]);
            //Debug.Log(targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                currentIndex++;
        }
    }
}