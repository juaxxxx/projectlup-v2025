using System;
using System.Collections.Generic;

namespace LUP
{
    [Serializable]
    public class ESItemStaticData : IItemStaticData
    {
        // ===== 필수 필드 (모든 시트에 있어야 함) =====
        [Column("ItemID", Required = true)]
        public int ItemID;

        [Column("ItemName", Required = true)]
        public string ItemName;

        [Column("ItemType", Required = true)]
        public string ItemType;

        // ===== 선택 필드 (시트에 있으면 로드, 없으면 기본값) =====
        [Column("IconPath")]
        public string IconPath = "";


        [Column("MaxStackSize")]
        public int MaxStackSize = 1;

        [Column("IsUsable")]
        public bool IsUsable = false;

        [Column("Description")]
        public string Description = "";

        // ===== 확장 필드 (자동 수집됨) =====
        private Dictionary<string, string> customFields = new Dictionary<string, string>();

        public LUPItemData ToItemData()
        {
            var item = new LUPItemData();

            // 필수 필드 설정
            item.SetItemID(ItemID);
            item.SetItemName(ItemName);
            item.SetItemType(ParseItemType(ItemType));

            // 선택 필드 설정
            if (!string.IsNullOrEmpty(IconPath))
                item.SetIconPath(IconPath);

            if (MaxStackSize != 1)
                item.SetMaxStackSize(MaxStackSize);

            if (IsUsable)
                item.SetIsUsable(IsUsable);

            if (!string.IsNullOrEmpty(Description))
                item.SetDescription(Description);

            // 확장 필드 설정
            if (customFields != null && customFields.Count > 0)
            {
                item.SetCustomFields(customFields);
            }

            return item;
        }

        // ICustomFieldSupport 구현
        public void SetCustomField(string fieldName, string value)
        {
            if (customFields == null)
            {
                customFields = new Dictionary<string, string>();
            }
            customFields[fieldName] = value;
        }

        private Define.ItemType ParseItemType(string type)
        {
            if (Enum.TryParse<Define.ItemType>(type, true, out var result))
            {
                return result;
            }
            UnityEngine.Debug.LogWarning($"[LUPItemStaticData] Invalid ItemType: {type}, defaulting to None");
            return Define.ItemType.None;
        }
    }
}
