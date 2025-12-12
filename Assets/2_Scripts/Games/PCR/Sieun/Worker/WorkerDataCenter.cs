using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerDataCenter : MonoBehaviour
    {
        
        [Header("단일 건물")]
        [SerializeField] private BuildingBase restaurant;
        [SerializeField] private BuildingBase station;

        [Header("시스템")]
        [SerializeField] PCRDataCenter pcrDataCenter;
        [SerializeField] AGridMap aGrid;
        [HideInInspector] public TileInfo[,] tileInfoes;

        [Header("작업자 데이터")]
        [SerializeField] private const int maxWorkerCount = 50;
        [SerializeField] private List<WorkerAI> workers = new List<WorkerAI>(maxWorkerCount);

        private bool isInitialized = false;
        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            if (isInitialized) return;

            // 컴포넌트 찾기 로직
            if (!pcrDataCenter) pcrDataCenter = GetComponentInChildren<PCRDataCenter>();
            if (!restaurant) restaurant = GetComponentInChildren<BuildingRestaurant>();
            if (!station) station = GetComponentInChildren<BuildingWorkStation>();

            isInitialized = true;
        }

        // 외부에서 워커를 등록하는 함수
        public void RegisterWorker(WorkerAI newWorker)
        {
            if (!isInitialized)
            {
                Initialize();
            }

            if (!workers.Contains(newWorker))
            {
                workers.Add(newWorker);

                newWorker.InitBTReferences();
                newWorker.SetGlobalBuildings(restaurant, station);
            }
        }


        private void Start()
        {
            aGrid.InitMap(pcrDataCenter.tileInfoes);
        }

        private void Update()
        {
            int count = workers.Count;

            for (int i = 0; i < count; i++)
            {
                if(workers[i] != null)
                {
                    if (i >= workers.Count)
                    {
                        break;
                    }

                    workers[i].UpdateBT();
                }

            }
        }
        public List<WorkerAI> GetIdleWorkers()
        {
            List<WorkerAI> idleList = new List<WorkerAI>();

            for (int i = 0; i < workers.Count; i++)
            {
                WorkerAI w = workers[i];
                // 작업자가 존재하고, 예약된 작업이 없을 때만 추가
                if (w != null && !w.HasTask)
                {
                    idleList.Add(w);
                }

            }

            Debug.Log($"전체 워커 {workers.Count}명 / 일 가능 워커 : {idleList.Count}명");

            return idleList;
        }

    }
}

