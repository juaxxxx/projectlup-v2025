using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    [CreateAssetMenu(fileName = "InitialBuildingSettingTable", menuName = "PCR/InitialBuildingSettingTable")]
    public class InitialBuildingSettingTable : ScriptableObject
    {
        public List<BuildingInfo> buildingList = new List<BuildingInfo>();
    }
}
