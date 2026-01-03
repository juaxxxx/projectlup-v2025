using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    [Serializable]
    public class ESItemStaticData : IItemStaticData, UnityEngine.ISerializationCallbackReceiver
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
        [Column("DropChance")]
        public float DropChance = 0f;

        [Column("WeaponType")]
        public string WeaponType = "";

        [Column("Damage")]
        public float Damage = 0;

        [Column("Range")]
        public float Range = 0;

        [Column("TimeBetAttack")]
        public float TimeBetAttack = 0;

        [Column("AttackAngle")]
        public float AttackAngle = 0;

        [Column("BulletSpeed")]
        public float BulletSpeed = 0;

        [Column("MagCapacity")]
        public float MagCapacity = 0;

        [Column("ReloadTime")]
        public float ReloadTime = 0;

        [Column("AttackRadius")]
        public float AttackRadius = 0;

        [Column("ArcHeight")]
        public float ArcHeight = 0;

        [Column("MaxChargeTime")]
        public float MaxChargeTime = 0;

        [Column("MinRange")]
        public float MinRange = 0;

        [System.NonSerialized]
        private Dictionary<string, string> customFields = new Dictionary<string, string>();


        // 직렬화를 위한 List
        [SerializeField]
        private List<CustomField> serializedCustomFields = new List<CustomField>();

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
