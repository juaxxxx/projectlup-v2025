using UnityEngine;

namespace LUP.ST
{
    [CreateAssetMenu(menuName = "Scriptable Objects/STCharacter Data", fileName = "STCharacterData_")]
    public class STCharacterData : ScriptableObject
    {
        public int characterId;
        public Sprite thumbnail;
        public GameObject prefab;
    }
}


