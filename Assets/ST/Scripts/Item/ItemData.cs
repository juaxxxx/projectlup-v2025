using UnityEngine;
namespace ST
{

    [CreateAssetMenu(fileName = "New Item", menuName = "Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("기본 정보")]
        public int itemID;
        public string itemName;
        public Sprite icon;

        [Header("타입")]
        public ItemType itemType;

        [Header("버프 효과 (버프 타입만)")]
        [Range(1f, 2f)] public float attackMultiplier = 1.2f;       // 공격력 1.2배
        [Range(1f, 2f)] public float defenseMultiplier = 1.2f;      // 방어력 1.2배
        [Range(1f, 2f)] public float attackSpeedMultiplier = 1.2f;  // ← 공격속도 1.2배 (빠르게!)
        [Range(0f, 1f)] public float healthRecoveryPercent = 0.2f;  // 체력 20% 회복
    }

    public enum ItemType
    {
        Buff,           // 공격/방어/공속 증가
        HealthRecover,  // 체력 회복
        Material,       // 재료
        Gold            // 골드
    }
}