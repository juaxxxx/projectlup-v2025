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

        [Header("크로스헤어")]
        public Sprite normalCrosshair;   // 기본 크로스헤어
        public Sprite scopeCrosshair;    // 스코프 크로스헤어

    }
}