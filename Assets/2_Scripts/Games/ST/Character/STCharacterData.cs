using UnityEngine;

namespace LUP.ST
{
    [CreateAssetMenu(menuName = "ST/Character Data")]
    public class STCharacterData : ScriptableObject
    {
        public int characterId;
        public string characterName;
        public Sprite thumbnail;
        public GameObject prefab;
    }
}