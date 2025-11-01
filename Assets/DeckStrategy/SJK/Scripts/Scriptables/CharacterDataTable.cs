using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character Data Table", menuName = "Scriptable/Character Data Table", order = int.MaxValue)]
public class CharacterDataTable : ScriptableObject
{
    public List<CharacterData> characterDataList = new List<CharacterData>();
}
