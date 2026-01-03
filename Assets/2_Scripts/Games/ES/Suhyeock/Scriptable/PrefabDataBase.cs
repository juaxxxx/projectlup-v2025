using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    [CreateAssetMenu(fileName = "PrefabDataBase", menuName = "ES/PrefabDataBase")]
    public class PrefabDataBase : ScriptableObject
    {
        [System.Serializable]
        public class ItemPrefabEntry
        {
            [ReadOnly] public int id;              // 자동 입력됨
            [ReadOnly] public string name;         // 자동 입력됨
            public GameObject prefab;   // 직접 연결할 곳
            public Vector3 positionOffset; // 위치 보정값
            public Vector3 rotationOffset; // 회전 보정값
        }

        //public ItemDataBase itemDataBase;

        [Header("Source Data")]
        public ESItemStaticDataLoader itemLoader; 

        [Header("Prefab List")]
        public List<ItemPrefabEntry> prefabList = new List<ItemPrefabEntry>();


        // 무기 등 프리팹이 필요한 아이템 타입 필터
        // 시트에 적혀있는 ItemType 문자열과 일치해야 합니다.
        private readonly List<string> weaponTypeStrings = new List<string>()
        {
            "Weapon",
            "RangedWeapon",
            "MeleeWeapon",
            "ThrowingWeapon" 
            // 엑셀에 "Sniper"나 "Pistol" 같은 타입이 별도로 있다면 여기 추가해야 합니다.
        };

        // =========================================================
        // 인스펙터 우클릭 메뉴에 "Sync IDs from ItemDB" 버튼을 만듭니다.
        // =========================================================
        [ContextMenu("Sync IDs from ItemDB")]
        public void SyncIds()
        {

            if (itemLoader == null)
            {
                Debug.LogError("Item Loader가 연결되지 않았습니다! 인스펙터에서 연결해주세요.");
                return;
            }

            List<ESItemStaticData> sourceList = itemLoader.GetDataList();

            if (sourceList == null || sourceList.Count == 0)
            {
                Debug.LogError("로더에 데이터가 없습니다. 로더 에셋에서 'Load' 버튼을 먼저 눌러주세요.");
                return;

            }
            int updateCount = 0;
            int newCount = 0;
            int removeCount = 0;

            // 1. 소스 데이터를 기반으로 "무기 리스트"에 있어야 할 ID들을 수집
            HashSet<int> validWeaponIds = new HashSet<int>();

            foreach (var staticData in sourceList)
            {
                // (1) 무기 타입인지 확인
                if (IsWeapon(staticData.ItemType))
                {
                    validWeaponIds.Add(staticData.ItemID); // 유효한 무기 ID로 등록

                    // (2) 리스트에 추가 또는 갱신
                    ItemPrefabEntry existingEntry = prefabList.Find(x => x.id == staticData.ItemID);

                    if (existingEntry != null)
                    {
                        // 이미 존재하면 이름만 동기화
                        if (existingEntry.name != staticData.ItemName)
                            existingEntry.name = staticData.ItemName;
                        updateCount++;
                    }
                    else
                    {
                        // 없으면 새로 추가
                        prefabList.Add(new ItemPrefabEntry
                        {
                            id = staticData.ItemID,
                            name = staticData.ItemName,
                            prefab = null,
                            positionOffset = Vector3.zero,
                            rotationOffset = Vector3.zero
                        });
                        newCount++;
                    }
                }
            }

            // 2. [청소 로직] 더 이상 무기가 아닌 아이템은 리스트에서 제거
            // (뒤에서부터 지워야 인덱스 에러가 안 납니다)
            for (int i = prefabList.Count - 1; i >= 0; i--)
            {
                // 현재 리스트에 있는 ID가, 방금 수집한 유효 무기 ID 목록에 없다면? -> 제거
                if (!validWeaponIds.Contains(prefabList[i].id))
                {
                    prefabList.RemoveAt(i);
                    removeCount++;
                }
            }

            Debug.Log($"[PrefabDB] 동기화 완료! (갱신: {updateCount}, 신규: {newCount}, 제거됨: {removeCount})");

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
            
            //if (itemDataBase == null)
            //{
            //    Debug.LogError("먼저 Source Item Data Base 항목에 'ItemDataBase' 파일을 연결해주세요!");
            //    return;
            //}

            //// ItemDataBase의 모든 아이템을 순회
            //// 주의: 사용자가 제공한 코드의 변수명이 'items'라고 가정 (private이면 public으로 바꿔주세요)
            //// 만약 private이라면 ItemDataBase에 public Getter를 만들어야 합니다.

            //// *편의상 ItemDataBase의 items 리스트가 public이라고 가정하고 작성합니다.*
            //// *private라면 [SerializeField]를 붙이거나 public으로 변경해주세요.*

            //foreach (BaseItemData itemData in itemDataBase.items) // 사용자의 DB 리스트 접근
            //{
            //    if (itemData.itemType != ItemType.Weapon)
            //        continue;
            //    // 이미 리스트에 해당 ID가 있는지 확인
            //    ItemPrefabEntry existingEntry = prefabList.Find(x => x.id == itemData.ID);

            //    if (existingEntry != null)
            //    {
            //        // 이미 있으면 이름만 최신화 (보기 편하게)
            //        existingEntry.name = itemData.Name;
            //    }
            //    else
            //    {
            //        // 없으면 새로 추가
            //        prefabList.Add(new ItemPrefabEntry
            //        {
            //            id = itemData.ID,
            //            name = itemData.Name,
            //            prefab = null // 프리팹은 비워둠
            //        });
            //    }
            //}
        }

        private bool IsWeapon(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;

            // 리스트에 있는 문자열과 정확히 일치하거나 포함하는지 확인
            foreach (var weaponType in weaponTypeStrings)
            {
                // StringComparison.OrdinalIgnoreCase : 대소문자 구분 없이 비교 (RangedWeapon == rangedweapon)
                if (string.Equals(typeStr, weaponType, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // 런타임에 프리팹을 가져오는 함수
        public GameObject GetPrefab(int id)
        {
            ItemPrefabEntry entry = prefabList.Find(x => x.id == id);
            if (entry != null) return entry.prefab;
            return null;
        }

        public ItemPrefabEntry GetEntry(int id)
        {
            ItemPrefabEntry entry = prefabList.Find(x => x.id == id);
            return entry;
        }
    }

}
