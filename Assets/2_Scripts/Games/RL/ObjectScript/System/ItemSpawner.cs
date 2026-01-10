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

        public Action<int, int> OnItemGained;

        //1번은 공용 재화, 2번은 장비구슬
        private Dictionary<RLDropItemType, Queue<GameObject>> poolDictionaray;

        public int SpawnCrystalPoolNum = 10;

        private Transform playerPos;
        private List<SpawnItemCrystal> activeCrystals = new List<SpawnItemCrystal>();

        void Start()
        {
            //playerPos = FindFirstObjectByType<PlayerMove>().GetComponent<Transform>();

            poolDictionaray = new Dictionary<RLDropItemType, Queue<GameObject>>();

            for (int i = 1; i < (int)RLDropItemType.Max; i++)
            {
                RLDropItemType itemType = (RLDropItemType)i;
                poolDictionaray[itemType] = new Queue<GameObject>();
                GameObject targetPrefabObject = commoditiesPrefab;

                switch (itemType)
                {
                    case RLDropItemType.Commodities:
                        targetPrefabObject = commoditiesPrefab;
                        break;

                    case RLDropItemType.equipment:
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
            Array values = Enum.GetValues(typeof(RLItemID));

            RLItemID randomSpawnItem = (RLItemID)values.GetValue(UnityEngine.Random.Range(0, values.Length - 1));
            RLDropItemType spanwedItemType = (RLDropItemType)((int)randomSpawnItem / 10000);

            int amount = 0;

            if(spanwedItemType == RLDropItemType.equipment)
            {
                amount = 1;
            }

            else if(spanwedItemType == RLDropItemType.Commodities)
            {
                amount = UnityEngine.Random.Range(1, 30);
            }

                GameObject obj = poolDictionaray[spanwedItemType].Dequeue();
            obj.SetActive(true);

            obj.transform.position = spawnPos.position;

            SpawnItemCrystal crystalball = obj.GetComponent<SpawnItemCrystal>();

            crystalball.SetSpawnItemInfo(spanwedItemType, (int)randomSpawnItem, amount, playerPos, this);

            activeCrystals.Add(crystalball);

        }

        public void ReturnCrystal(RLDropItemType type, GameObject obj)
        {
            SpawnItemCrystal comp = obj.GetComponent<SpawnItemCrystal>();

            OnItemGained?.Invoke(comp.itemID, comp.amount);

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

