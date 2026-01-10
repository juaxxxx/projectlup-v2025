using UnityEngine;
using Roguelike.Define;
using static UnityEngine.GraphicsBuffer;

namespace LUP.RL
{
    public class SpawnItemCrystal : MonoBehaviour
    {
        public float flyingSpeed = 10f;

        [HideInInspector]
        public int itemID = 0;

        [HideInInspector]
        public int amount = 0;

        public RLDropItemType itemType;

        [HideInInspector]
        public bool bIsStageCleared = false;

        [HideInInspector]
        public Transform target;

        private ItemSpawner spawnPool;

        // Update is called once per frame
        void Update()
        {
            if (bIsStageCleared == false)
                return;

            if (target != null)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    flyingSpeed * Time.deltaTime
                );
            }
        }

        public void SetSpawnItemInfo(RLDropItemType type, int ItemID, int gainedAmount, Transform playerPos, ItemSpawner spawner)
        {
            itemType = type;
            itemID = ItemID;
            target = playerPos;

            spawnPool = spawner;

            amount = gainedAmount;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                spawnPool.ReturnCrystal(itemType, this.gameObject);
            }
        }

        public void CallRoomCleared()
        {
            bIsStageCleared = true;
        }
    }
}

