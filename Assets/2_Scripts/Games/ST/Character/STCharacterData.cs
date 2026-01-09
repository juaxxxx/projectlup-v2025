using UnityEngine;

namespace LUP.ST
{
    [CreateAssetMenu(menuName = "Scriptable Objects/STCharacter Data", fileName = "STCharacterData_")]
    public class STCharacterData : ScriptableObject
    {
        public int characterId;
        public int level;
        public int currentExp;
        public Sprite thumbnail;
        public GameObject prefab;
    }
}


