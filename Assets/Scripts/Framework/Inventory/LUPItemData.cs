using UnityEngine;

namespace LUP
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "LUP/Item Data")]
    public class LUPItemData : ScriptableObject, IItemable
    {
        [Header("기본 정보")]
        [SerializeField] private int itemID;
        [SerializeField] private string itemName;
        [SerializeField] private Define.ItemType itemType;
        [SerializeField] private Sprite icon;

        [Header("스택 설정")]
        [SerializeField] private int maxStackSize = 1;

        [Header("사용 가능 여부")]
        [SerializeField] private bool isUsable = false;

        [Header("사용 효과 (옵션)")]
        [SerializeField] private string useEffectDescription;

        public int ItemID => itemID;
        public string ItemName => itemName;
        public Define.ItemType Type => itemType;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
        public bool IsUsable => isUsable;

        public void OnUse()
        {
            if (!isUsable)
            {
                Debug.LogWarning($"{itemName}은(는) 사용할 수 없는 아이템입니다.");
                return;
            }

            Debug.Log($"{itemName} 사용! 효과: {useEffectDescription}");

            // TODO: 실제 게임에서는 여기에 아이템별 효과 구현
            // 예: 체력 회복, 버프 적용 등
        }

        private void OnValidate()
        {
            // itemID가 0이면 경고 (int는 자동 할당 불가)
            if (itemID == 0)
            {
                Debug.LogWarning($"[{name}] ItemID가 설정되지 않았습니다. 고유한 ID를 지정해주세요.");
            }
        }
    }
}
