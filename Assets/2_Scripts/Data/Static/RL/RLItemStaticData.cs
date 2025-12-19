using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    [Serializable]
    public class RLItemStaticData : IItemStaticData, UnityEngine.ISerializationCallbackReceiver
    {
        // ===== �ʼ� �ʵ� (��� ��Ʈ�� �־�� ��) =====
        [Column("ItemID", Required = true)]
        public int ItemID;

        [Column("ItemName", Required = true)]
        public string ItemName;

        [Column("ItemType", Required = true)]
        public string ItemType;

        // ===== ���� �ʵ� (��Ʈ�� ������ �ε�, ������ �⺻��) =====
        [Column("IconPath")]
        public string IconPath = "";

        [Column("MaxStackSize")]
        public int MaxStackSize = 1;

        [Column("IsUsable")]
        public bool IsUsable = false;

        [Column("Description")]
        public string Description = "";

        // ===== Ȯ�� �ʵ� (�ڵ� ������) =====
        [System.NonSerialized]
        private Dictionary<string, string> customFields = new Dictionary<string, string>();

        // 직렬화를 위한 List
        [SerializeField]
        private List<CustomField> serializedCustomFields = new List<CustomField>();

        public LUPItemData ToItemData()
        {
            var item = new LUPItemData();

            // �ʼ� �ʵ� ����
            item.SetItemID(ItemID);
            item.SetItemName(ItemName);
            item.SetItemType(ParseItemType(ItemType));

            // ���� �ʵ� ����
            if (!string.IsNullOrEmpty(IconPath))
                item.SetIconPath(IconPath);

            if (MaxStackSize != 1)
                item.SetMaxStackSize(MaxStackSize);

            if (IsUsable)
                item.SetIsUsable(IsUsable);

            if (!string.IsNullOrEmpty(Description))
                item.SetDescription(Description);

            // Ȯ�� �ʵ� ����
            if (customFields != null && customFields.Count > 0)
            {
                item.SetCustomFields(customFields);
            }

            return item;
        }

        // ICustomFieldSupport ����
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

        // ===== Unity 직렬화 콜백 =====
        public void OnBeforeSerialize()
        {
            // Dictionary를 List로 변환 (직렬화 전)
            serializedCustomFields.Clear();
            if (customFields != null)
            {
                foreach (var kvp in customFields)
                {
                    serializedCustomFields.Add(new CustomField { key = kvp.Key, value = kvp.Value });
                }
            }
        }

        public void OnAfterDeserialize()
        {
            // List를 Dictionary로 복원 (역직렬화 후)
            customFields = new Dictionary<string, string>();
            if (serializedCustomFields != null)
            {
                foreach (var field in serializedCustomFields)
                {
                    customFields[field.key] = field.value;
                }
            }
        }
    }
}
