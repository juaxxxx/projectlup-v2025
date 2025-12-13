using System.Collections.Generic;
using UnityEngine;
using Roguelike.Define;
using System;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace LUP.RL
{
    public class ItemSpawner : MonoBehaviour
    {
        public Transform SpawnPool;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public GameObject commoditiesPrefab;
        public GameObject equipmentPrefab;

        public Action<int> OnItemGained;

        //1번은 공용 재화, 2번은 장비구슬
        private Dictionary<ItemType, Queue<GameObject>> poolDictionaray;

        public int SpawnCrystalPoolNum = 10;

        private Transform playerPos;
        private List<SpawnItemCrystal> activeCrystals = new List<SpawnItemCrystal>();

        void Start()
        {
            //playerPos = FindFirstObjectByType<PlayerMove>().GetComponent<Transform>();

            poolDictionaray = new Dictionary<ItemType, Queue<GameObject>>();

            for (int i = 1; i < (int)ItemType.Max; i++)
            {
                ItemType itemType = (ItemType)i;
                poolDictionaray[itemType] = new Queue<GameObject>();
                GameObject targetPrefabObject = commoditiesPrefab;

                switch (itemType)
                {
                    case ItemType.Commodities:
                        targetPrefabObject = commoditiesPrefab;
                        break;

                    case ItemType.equipment:
                        targetPrefabObject = equipmentPrefab;
                        break;
                }

                for(int count = 0; count < SpawnCrystalPoolNum; count++ )
                {
                    GameObject obj = Instantiate(targetPrefabObject);

                    obj.transform.SetParent(SpawnPool);

                    obj.SetActive(false);
                    poolDictionaray[itemType].Enqueue(obj);
                }
            }
        }

        public void SetPlayerPos(Transform playerTransform)
        {
            playerPos = playerTransform;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpawnItem(Transform spawnPos)
        {
            Array values = Enum.GetValues(typeof(RLSpanableItem));

            RLSpanableItem randomSpawnItem = (RLSpanableItem)values.GetValue(UnityEngine.Random.Range(0, values.Length - 1));
            ItemType spanwedItemType = (ItemType)((int)randomSpawnItem / 10000);

            GameObject obj = poolDictionaray[spanwedItemType].Dequeue();
            obj.SetActive(true);

            obj.transform.position = spawnPos.position;

            SpawnItemCrystal crystalball = obj.GetComponent<SpawnItemCrystal>();

            crystalball.SetSpawnItemInfo(spanwedItemType, (int)randomSpawnItem, playerPos, this);

            activeCrystals.Add(crystalball);

        }

        public void ReturnCrystal(ItemType type, GameObject obj)
        {
            SpawnItemCrystal comp = obj.GetComponent<SpawnItemCrystal>();

            OnItemGained?.Invoke(comp.itemID);

            comp.bIsStageCleared = false;
            comp.target = null;
            comp.itemID = 0;

            obj.SetActive(false);

            poolDictionaray[type].Enqueue(obj);

            activeCrystals.Remove(comp);
        }

        public void OnRoomCleared()
        {
            for(int i = 0; i < activeCrystals.Count; i++)
            {
                activeCrystals[i].CallRoomCleared();
            }
        }
    }
}

