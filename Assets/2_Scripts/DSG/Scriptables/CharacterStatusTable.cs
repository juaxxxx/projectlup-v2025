using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Status Data Table", menuName = "Scriptable/Character Status Data Table", order = int.MaxValue)]
public class CharacterStatusTable : ScriptableObject
{
    public List<CharacterStatusData> characterModelDataList = new List<CharacterStatusData>();
}