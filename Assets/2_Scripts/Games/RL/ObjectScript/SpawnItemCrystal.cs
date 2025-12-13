using UnityEngine;
using Roguelike.Define;
using static UnityEngine.GraphicsBuffer;

namespace LUP.RL
{
    public class SpawnItemCrystal : MonoBehaviour
    {
        public float flyingSpeed = 50f;

        [HideInInspector]
        public int itemID = 0;

        public ItemType itemType;

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

        public void SetSpawnItemInfo(ItemType type, int ItemID, Transform playerPos, ItemSpawner spawner)
        {
            itemType = type;
            itemID = ItemID;
            target = playerPos;

            spawnPool = spawner;
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

