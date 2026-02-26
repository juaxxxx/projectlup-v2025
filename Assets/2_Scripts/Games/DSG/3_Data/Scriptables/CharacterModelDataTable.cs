using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [CreateAssetMenu(fileName = "Character Model Data Table", menuName = "DSG/Character Model Data Table", order = int.MaxValue)]
    public class CharacterModelDataTable : ScriptableObject
    {
        public List<CharacterModelData> characterModelDataList = new List<CharacterModelData>();
    }
}