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
            public int id;              // 자동 입력됨
            public string name;         // 자동 입력됨
            public GameObject prefab;   // 직접 연결할 곳
            public Vector3 positionOffset; // 위치 보정값
            public Vector3 rotationOffset; // 회전 보정값
        }

        // [핵심 2] 원본 데이터베이스 연결 (여기에 ItemDataBase를 넣으세요)
        public ItemDataBase itemDataBase;

        // 실제 프리팹 리스트
        public List<ItemPrefabEntry> prefabList = new List<ItemPrefabEntry>();

        // =========================================================
        // [핵심 3] 자동 동기화 버튼 기능
        // 인스펙터 우클릭 메뉴에 "Sync IDs from ItemDB" 버튼을 만듭니다.
        // =========================================================
        [ContextMenu("Sync IDs from ItemDB")]
        public void SyncIds()
        {
            if (itemDataBase == null)
            {
                Debug.LogError("먼저 Source Item Data Base 항목에 'ItemDataBase' 파일을 연결해주세요!");
                return;
            }

            // ItemDataBase의 모든 아이템을 순회
            // 주의: 사용자가 제공한 코드의 변수명이 'items'라고 가정 (private이면 public으로 바꿔주세요)
            // 만약 private이라면 ItemDataBase에 public Getter를 만들어야 합니다.

            // *편의상 ItemDataBase의 items 리스트가 public이라고 가정하고 작성합니다.*
            // *private라면 [SerializeField]를 붙이거나 public으로 변경해주세요.*

            foreach (BaseItemData itemData in itemDataBase.items) // 사용자의 DB 리스트 접근
            {
                if (itemData.itemType != ItemType.Weapon)
                    continue;
                // 이미 리스트에 해당 ID가 있는지 확인
                ItemPrefabEntry existingEntry = prefabList.Find(x => x.id == itemData.id);

                if (existingEntry != null)
                {
                    // 이미 있으면 이름만 최신화 (보기 편하게)
                    existingEntry.name = itemData.name;
                }
                else
                {
                    // 없으면 새로 추가
                    prefabList.Add(new ItemPrefabEntry
                    {
                        id = itemData.id,
                        name = itemData.name,
                        prefab = null // 프리팹은 비워둠
                    });
                }
            }
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
