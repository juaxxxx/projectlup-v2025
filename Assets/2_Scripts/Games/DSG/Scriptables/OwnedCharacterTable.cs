using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Owned Character List Table", menuName = "DSG/Owned Character List Table", order = int.MaxValue)]
public class OwnedCharacterTable : ScriptableObject
{
    public List<OwnedCharacterInfo> ownedCharacterList = new List<OwnedCharacterInfo>();
}
