using UnityEngine;

namespace Framework
{
    public abstract class BaseItemData : ScriptableObject, IInventoryItemable
    {
        [Header("기본 정보")]
        [SerializeField] protected int itemID;
        [SerializeField] protected string itemName;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected Define.ItemType itemType;

        [Header("스택 설정")]
        [SerializeField] protected int maxStackSize = 1;
        [SerializeField] protected bool isStackable = false;

        [Header("기타")]
        [SerializeField] protected int sellPrice = 0;
        [SerializeField] protected int buyPrice = 0;

        // IInventoryItem 인터페이스 구현
        public int ItemID => itemID;
        public string ItemName => itemName;
        public Sprite Icon => icon;
        public Define.ItemType ItemType => itemType;
        public int MaxStackSize => maxStackSize;
        public bool IsStackable => isStackable;

        public int SellPrice => sellPrice;
        public int BuyPrice => buyPrice;

        // 아이템 사용 가능 여부
        public virtual bool CanUse()
        {
            return false;
        }

        
        public virtual ItemRuntimeData ToSaveData()
        {
            return new ItemRuntimeData
            {
                ItemId = itemID,
                Count = 1
            };
        }

        /// <summary>
        /// 저장된 데이터에서 복원 (ScriptableObject는 읽기 전용이므로 의미 없음)
        /// </summary>
        public virtual void FromSaveData(ItemRuntimeData saveData)
        {
            // ScriptableObject는 읽기 전용 데이터이므로 구현 불필요
            // 런타임 아이템 인스턴스가 필요한 경우 별도 클래스 사용
        }


        protected virtual void OnValidate()
        {
            if (maxStackSize < 1)
            {
                maxStackSize = 1;
            }

            if (maxStackSize > 1 && !isStackable)
            {
                Debug.LogWarning($"[{itemName}] MaxStackSize가 1보다 크면 IsStackable을 true로 설정해야 합니다.");
            }

            if (sellPrice < 0)
            {
                sellPrice = 0;
            }

            if (buyPrice < 0)
            {
                buyPrice = 0;
            }
        }
    }

}

