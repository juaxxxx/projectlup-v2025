using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.ES
{
    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "ES/ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        [Header("Data Source")]
        public ESItemStaticDataLoader itemLoader;

        [SerializeReference]
        public List<BaseItemData> items = new List<BaseItemData>();
        //{
        //        new RangedWeaponItemData(0, "Pistol", "Pistol",  5f, 5f, 0.3f, 10f, 8,  1.2f),
        //        new RangedWeaponItemData(1, "AR", "AR",  20f, 10f, 0.15f, 20f, 30, 2.0f),
        //        new RangedWeaponItemData(2, "Rifle", "Rfile",  45f, 20f, 0.7f, 25f, 10, 4.0f),
        //        new ArmorItemData(3, "Helmet", "Helmet",5, ArmorSlot.Head ),
        //        new ArmorItemData(4, "BodyArmor", "BodyArmor",5, ArmorSlot.Body ),
        //        new ConsumableItemData(5, "Bandage", "Bandage", 5.0f, 5.0f, EffectType.Heal, 30.0f, 3),
        //        new MaterialItemData(6, "Key", "Key", MaterialTier.Common, 5),
        //        new MeleeWeaponItemData(7, "Sword", "Sword", 30.0f, 2f, 0.8f, 160f),
        //        new ThrowingWeaponData(8, "Rock", "Sword", 30.0f, 8f, 0.9f, 1.0f, 4.0f, 2.0f),
        //};

        private Dictionary<int, BaseItemData> itemDictionary = new Dictionary<int, BaseItemData>();
        private bool isInitialized = false;

        public void Initiallize()
        {
            if (itemLoader == null)
            {
                Debug.LogError("[ItemDataBase] ItemLoader가 연결되지 않았습니다!");
                return;
            }
            List<ESItemStaticData> sourceList = itemLoader.GetDataList();
            if (sourceList == null) return;

            items.Clear();
            itemDictionary.Clear();

            foreach (ESItemStaticData data in sourceList)
            {
                BaseItemData convertedItem = ConvertData(data);

                if (convertedItem != null)
                {
                    items.Add(convertedItem);

                    if (!itemDictionary.ContainsKey(convertedItem.ID))
                    {
                        itemDictionary.Add(convertedItem.ID, convertedItem);
                    }
                }
            }

            isInitialized = true;
            Debug.Log($"[ItemDataBase] 초기화 완료. 로드된 아이템 수: {items.Count}");

            //for (int i = 0; i < items.Count; i++)
            //{
            //    if (!itemDictionary.ContainsKey(items[i].ID))
            //    {
            //        itemDictionary.Add(items[i].ID, items[i]);
            //    }
            //}
        }

        public BaseItemData GetItemByID(int id)
        {
            // 아직 초기화가 안 되어 있다면 자동으로 초기화 시도
            if (!isInitialized || itemDictionary.Count == 0)
            {
                Initiallize();
            }

            if (itemDictionary.TryGetValue(id, out BaseItemData item))
            {
                return item;
            }

            Debug.LogWarning($"[ItemDataBase] ID {id} 아이템을 찾을 수 없습니다.");
            return null;
        }
        public int GetItemCount() 
        { 
            return items.Count; 
        }


        private BaseItemData ConvertData(ESItemStaticData data)
        {
            int id = data.ItemID;
            string name = data.ItemName;
            string desc = data.Description;

            // ItemType 문자열에 따라 적절한 클래스 생성
            // (대소문자 무시 비교)
            if (IsType(data.WeaponType, "Ranged"))
            {
                return new RangedWeaponItemData(
                    id, name, desc,
                    data.IconPath,
                    data.DropChance,
                    data.Damage,
                    data.Range,
                    data.TimeBetAttack,
                    data.BulletSpeed,
                    (int)data.MagCapacity,
                    data.ReloadTime
                );
            }
            else if (IsType(data.WeaponType, "Melee"))
            {
                // 생성자 파라미터에 맞게 데이터 매핑
                return new MeleeWeaponItemData(id, name, desc, data.IconPath,data.DropChance, data.Damage, data.Range, data.TimeBetAttack, data.AttackAngle);
            }
            else if (IsType(data.WeaponType, "Throwing"))
            {
                return new ThrowingWeaponData(id, name, desc, data.IconPath, data.DropChance, data.Damage, data.Range, data.TimeBetAttack, data.AttackRadius, data.ArcHeight, data.MaxChargeTime);
            }
            else if (IsType(data.ItemType, "Armor"))
            {
                // ArmorSlot 등 Enum 파싱이 필요할 수 있음. 여기서는 예시 값.
                return new ArmorItemData(id, name, desc, data.IconPath, data.DropChance, (int)data.Damage, ArmorSlot.Body);
            }
            else if (IsType(data.ItemType, "Consumable"))
            {
                //소모품은 나중에
                return new ConsumableItemData(id, name, desc, data.IconPath, data.DropChance, data.Damage, data.Damage, EffectType.Heal, 30f, 3);
            }
            // 필요한 다른 타입들도 else if로 추가...

            return null; // 처리하지 않는 타입
        }

        private bool IsType(string current, string target)
        {
            return string.Equals(current, target, StringComparison.OrdinalIgnoreCase);
        }

        [ContextMenu("Load Data From Framework")]
        public void ForceLoadInEditor()
        {
            Initiallize();
        }
    }
}
