using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [CreateAssetMenu(fileName = "Character Prefab Data Table", menuName = "DSG/Character Prefab Data Table", order = int.MaxValue)]
    public class CharacterPrefabDataTable : ScriptableObject
    {
        public List<CharacterPrefabData> characterModelDataList = new List<CharacterPrefabData>();
    }
}