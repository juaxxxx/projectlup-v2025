using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerSystem : MonoBehaviour
    {
        // 옵션
        [SerializeField] private GameObject workerPrefab;
        [SerializeField] private Transform workerContainer;
        private BuildingBase defaultRestaurant; 
        private BuildingBase defaultStation; // @TODO : 같은 종류의 목적지 중에 가장 가까운 곳을 찾게 bt 로직 변경
        private int maxWorkerCount = 50;
        private bool isInitialized = false;

        private TileMap tileMap;
        private AGridMap aGrid;

        //// 런타임에서 갱신되는 데이터들
        private ProductionRuntimeData pcrRuntimeData;
        private List<int> curReservedBuildingIdList;
        private List<int> curAssignedBuildingIdList;
        // 모든 건물정보와 작업자 정보
        private Dictionary<int, BuildingBase> curBuildings; // 건물 Id로 BuildingBase 읽기전용
        private List<WorkerInfo> curWorkerInfoList;

        // 실제 작업 할당
        private List<BuildingBase> taskBuildingList = new List<BuildingBase>();
        private List<WorkerAI> activeWorkers;
        private Queue<StructureBase> taskQueue = new Queue<StructureBase>();

        public void InitWorkerSystem(BuildingSystem buildingSystem, TileMap tileMap)
        {
            aGrid = GetComponentInChildren<AGridMap>();
            this.tileMap = tileMap;
            aGrid.InitMap(tileMap.tiles);

            curBuildings = buildingSystem.GetCurrentBuildingDictionary();
            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
            pcrRuntimeData = stage.productionRuntimeData;
            curReservedBuildingIdList = pcrRuntimeData.ReservedBuildingIdList;
            curAssignedBuildingIdList = pcrRuntimeData.AssignedBuildingIdList;
            curWorkerInfoList = pcrRuntimeData.WorkerInfoList;

            // 위 데이터 기반으로 초기화.
            curWorkerInfoList.Clear();
            activeWorkers = new List<WorkerAI>(maxWorkerCount);

            InitDefaults();
            // @TODO: 초기 건물 외에 다른 건물들(앞으로 새로 배치될 때 포함해서) 세워질 때마다 호출되게 수정하기
            AddWorkPlaces(); 
            
            TestDebuging();
            isInitialized = true;
        }

        private void InitDefaults()
        {
            // 초기 건물정보 생성
            if (curBuildings.TryGetValue(1, out BuildingBase b1) && b1 is BuildingRestaurant)
            {
                defaultRestaurant = b1;
            }

            if (curBuildings.TryGetValue(2, out BuildingBase b2) && b2 is BuildingWorkStation)
            {
                defaultStation = b2;
            }

            if (defaultRestaurant == null)
            {
                foreach (var building in curBuildings.Values)
                {
                    if (building is BuildingWorkStation)
                    {
                        defaultStation = building;
                        break;
                    }
                }
            }
            if (defaultStation == null)
            {
                foreach (var building in curBuildings.Values)
                {
                    if (building is BuildingWorkStation)
                    {
                        defaultStation = building;
                        break;
                    }
                }
            }

            // @TODO: 식당과 워크스테이션도 작업장소로 포함시킬지 고민하기
            //if (defaultRestaurant != null) taskBuildingList.Add(defaultRestaurant);
            //if (defaultStation != null) taskBuildingList.Add(defaultStation);

            // 초기 작업자 정보 생성
            int defaultWorkerCount = 5;
            if (curWorkerInfoList.Count == 0)
            {
                for (int i = 0; i < defaultWorkerCount; i++)
                {
                    WorkerInfo testInfo = new WorkerInfo();
                    testInfo.id = i;
                    testInfo.name = $"DefaultWorker_{i}";

                    curWorkerInfoList.Add(testInfo);
                    CreateWorkerObject(testInfo);
                }
            }
        }
        private void AddWorkPlaces()
        {
            foreach ((int id, BuildingBase building) in curBuildings)
            {
                if (building == defaultRestaurant || building == defaultStation) continue;

                taskBuildingList.Add(building);
            }

            //@TODO : 건물 외 작업 구역(채집 등)도 별도의 task~List.Add 로 별도의 목록에 추가
        }
        private void CreateWorkerObject(WorkerInfo info)
        {
            if (defaultStation == null) return;

            ANode spawnNode = aGrid.GetNodeFromGridPos(defaultStation.entrancePos);
            
            if (spawnNode != null)
            {
                Vector3 floorPos = aGrid.GetNodeFootPosition(spawnNode);
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Vector3 spawnPos = floorPos + randomOffset;
                GameObject newWorker = Instantiate(workerPrefab, spawnPos, Quaternion.identity, workerContainer);
                WorkerAI ai = newWorker.GetComponent<WorkerAI>();
                
                if (ai == null)
                {
                    ai = ai.GetComponentInChildren<WorkerAI>();
                }
                
                if (!activeWorkers.Contains(ai))
                {
                    activeWorkers.Add(ai);
                }

                ai.Initialize(info, defaultRestaurant, defaultStation);
            }
        }

        private void TestDebuging()
        {
//            if (taskBuildingList != null)
//            {
//                Debug.Log($"작업가능 건물 : (총 {taskBuildingList.Count}개)");

//                for (int i = 0; i < taskBuildingList.Count; i++)
//                {
//                    BuildingBase station = taskBuildingList[i];
//                    if (station != null)
//                    {
//                        Debug.Log($"{i}번 이름: {station.name}, 위치: {station.transform.position}");
//                    }
//                    else
//                    {
//                        Debug.Log($"[{i}번] NULL (비어있음)");
//                    }
//                }
//;
//            }
        }

        private void Update()
        {
            if (!isInitialized || activeWorkers == null) return;

            int count = activeWorkers.Count;

            for (int i = 0; i < count; i++)
            {
                if (activeWorkers[i] != null)
                {
                    if (i >= activeWorkers.Count)
                    {
                        break;
                    }

                    activeWorkers[i].UpdateBT();
                }

            }

            AssignPendingTasks();
        }

        private void AssignPendingTasks()
        {
            if (taskQueue.Count == 0) return;

            // 노는 일꾼 찾기
            List<WorkerAI> idleWorkers = GetIdleWorkers();
            if (idleWorkers.Count == 0) return;

            // 큐에서 일감 꺼내서 배정
            while (taskQueue.Count > 0 && idleWorkers.Count > 0)
            {
                StructureBase targetStructure = taskQueue.Peek();

                // 해당 장소로 갈 수 있는 가장 가까운 일꾼 찾기
                WorkerAI bestWorker = GetBestInIdleWorkers(idleWorkers, targetStructure);

                if (bestWorker != null)
                {
                    taskQueue.Dequeue(); // 큐에서 제거
                    bestWorker.AssignTask(targetStructure); // 작업 할당
                    idleWorkers.Remove(bestWorker); // 목록에서 제외
                }
                else
                {
                    // 지금 갈 수 있는 일꾼이 없다면 다음 기회에
                    break;
                }
            }
        }
        public void RegisterTask(StructureBase structure)
        {
            if (!taskQueue.Contains(structure))
            {
                taskQueue.Enqueue(structure);
            }
        }

        public List<WorkerAI> GetIdleWorkers()
        {
            List<WorkerAI> idleList = new List<WorkerAI>();

            for (int i = 0; i < activeWorkers.Count; i++)
            {
                WorkerAI w = activeWorkers[i];
                
                // 작업자가 존재하고, 예약된 작업이 없을 때만 추가
                if (w != null && !w.HasTask)
                {
                    idleList.Add(w);
                }
            }

            return idleList;
        }
        private WorkerAI GetBestInIdleWorkers(List<WorkerAI> candidates, StructureBase structure)
        {
            if (candidates == null || candidates.Count == 0) return null;

            ANode targetNode = aGrid.GetNodeFromWorldPosition(structure.transform.position);

            if (targetNode == null || !targetNode.isWalkable) return null;

            WorkerAI bestWorker = null;
            float minScore = float.MaxValue;
            float tolerance = 0.1f;

            foreach (var w in candidates)
            {
                if (w == null) continue; // 이미 idle 상태인 것만 넘겨받았으므로 HasTask 체크 불필요

                ANode workerNode = aGrid.GetNodeFromWorldPosition(w.transform.position);
                if (workerNode == null) continue;

                // 맨해튼 거리 계산
                int dx = Mathf.Abs(workerNode.indexX - targetNode.indexX);
                int dy = Mathf.Abs(workerNode.indexY - targetNode.indexY);
                float distScore = dx + dy;

                if (distScore < minScore - tolerance)
                {
                    minScore = distScore;
                    bestWorker = w;
                }
                else if (Mathf.Abs(distScore - minScore) <= tolerance)
                {
                    // 거리가 비슷하면 ID가 낮은 순 (일관성 유지)
                    if (bestWorker != null && w.GetInstanceID() < bestWorker.GetInstanceID())
                    {
                        bestWorker = w;
                    }
                    else if (bestWorker == null)
                    {
                        bestWorker = w;
                    }
                }
            }

            return bestWorker;
        }
        

    }
}

/*

        //// [새로운 워커 고용] 게임 도중 버튼을 눌러 추가할 때 사용
        //public void HireNewWorker(string name)
        //{
        //    int newId = curWorkerInfoList.Count + 1; // ID 생성
        //    //WorkerInfo newInfo = new WorkerInfo { workerId = newId, workerName = name };
        //    //curWorkerInfoList.Add(newInfo);
        //    //CreateWorkerObject(newInfo);
        //}

        //// [워커 삭제]
        //public void RemoveWorker(WorkerAI worker)
        //{
        //    if (activeWorkers.Contains(worker))
        //    {
        //        activeWorkers.Remove(worker);
        //        // curWorkerInfoList.Remove(...) 
        //        Destroy(worker.gameObject);
        //    }
        //}


 */