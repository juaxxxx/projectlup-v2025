using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerDataCenter : MonoBehaviour
    {
        [SerializeField] PCRDataCenter pcrDataCenter;
        //[SerializeField] TestBuildingSystem testBuildingSystem;
        [SerializeField] AGridMap aGrid;
        [SerializeField] WorkerAI workerAI;
        [HideInInspector] public TileInfo[,] tileInfoes;

        private void Awake()
        {
            pcrDataCenter = GetComponentInChildren<PCRDataCenter>();
        }

        private void Start()
        {
            aGrid.InitMap(pcrDataCenter.tileInfoes);
            workerAI.InitBTReferences();
        }

        private void Update()
        {
            workerAI.UpdateBT();
        }
    }
}

