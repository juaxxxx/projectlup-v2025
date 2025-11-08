using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static OwnedCharacterTable;

namespace LUP.DSG
{

    [CreateAssetMenu(fileName = "Character Model Data Table", menuName = "Scriptable/Character Model Data Table", order = int.MaxValue)]
    public class CharacterModelDataTable : ScriptableObject
    {
        public List<CharacterModelData> characterModelDataList = new List<CharacterModelData>();
    }
}