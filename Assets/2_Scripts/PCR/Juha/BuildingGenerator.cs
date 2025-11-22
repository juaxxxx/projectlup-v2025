using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class BuildingGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject wheatFarmPrefab;
        [SerializeField]
        private GameObject mushroomFarmPrefab;
        [SerializeField]
        private GameObject restaurantPrefab;

        private Dictionary<int, WallBase> currWalls;
        private Dictionary<int, BuildingBase> currBuildings;
    }


}
