using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerDataCenter : MonoBehaviour
    {
        [SerializeField] PCRDataCenter pcrDataCenter;
        [SerializeField] AGridMap aGrid;
        [SerializeField] private const int maxWorkerCount = 50;
        [SerializeField] private List<WorkerAI> workers = new List<WorkerAI>(maxWorkerCount);
        [HideInInspector] public TileInfo[,] tileInfoes;

        // 외부에서 워커를 등록하는 함수
        public void RegisterWorker(WorkerAI newWorker)
        {
            if(!workers.Contains(newWorker))
            {
                workers.Add(newWorker);
                newWorker.InitBTReferences();
            }
        }

        private void Awake()
        {
            pcrDataCenter = GetComponentInChildren<PCRDataCenter>();
        }

        private void Start()
        {
            aGrid.InitMap(pcrDataCenter.tileInfoes);

            int count = workers.Count;

            for (int i = 0; i < count; i++)
            {
                if (workers[i] != null)
                {
                    workers[i].InitBTReferences();
                }
            }

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

