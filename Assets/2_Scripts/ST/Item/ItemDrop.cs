using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Rendering.FilterWindow;
namespace LUP.ST
{

    public class ItemDrop : MonoBehaviour
    {
        [Header("드롭 데이터")]
        public ItemData itemData;
        public int goldAmount;

        [Header("모션 설정")]
        public float dropHeight = 2f;
        public float dropDuration = 0.5f;
        public float collectDelay = 0.3f;

        void Start()
        {
            StartCoroutine(DropAndCollect());
        }

        private IEnumerator DropAndCollect()
        {
            Vector3 startPos = transform.position + Vector3.up * dropHeight;
            Vector3 endPos = transform.position;

            transform.position = startPos;

            // 떨어지는 모션
            float elapsed = 0f;
            while (elapsed < dropDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dropDuration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                transform.Rotate(Vector3.up, 360f * Time.deltaTime);
                yield return null;
            }

            transform.position = endPos;

            // 잠깐 대기
            yield return new WaitForSeconds(collectDelay);

            // 획득!
            Collect();
        }

        private void Collect()
        {
            // 골드
            if (goldAmount > 0)
            {
                GameData.AddGold(goldAmount);
            }

            // 아이템
            if (itemData != null)
            {
                if (itemData.itemType == ItemType.Buff)
                {
                    // 스탯 버프 = 즉시 모든 플레이어에게 적용!
                    ApplyBuffToAllPlayers();
                }
                else if (itemData.itemType == ItemType.HealthRecover)
                {
                    // 체력 회복 = 즉시 모든 플레이어 회복!
                    HealAllPlayers();
                }
                else if (itemData.itemType == ItemType.Material)
                {
                    // 재료 = 데이터 저장
                    GameData.AddMaterial(itemData);
                }
            }

            Destroy(gameObject);
        }

        private void ApplyBuffToAllPlayers()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                StatComponent stats = player.GetComponent<StatComponent>();
                if (stats != null)
                {
                    if (itemData.attackMultiplier > 1f)
                    {
                        stats.MultiplyAttackDamage(itemData.attackMultiplier);
                    }

                    if (itemData.defenseMultiplier > 1f)
                    {
                        stats.MultiplyDefense(itemData.defenseMultiplier);
                    }
                    if (itemData.attackSpeedMultiplier > 1f)
                    {
                        stats.MultiplyAttackSpeed(itemData.attackSpeedMultiplier);
                    }

                    Debug.Log($"{player.name}에게 {itemData.itemName} 버프 적용!");
                }
            }
        }

        private void HealAllPlayers()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                StatComponent stats = player.GetComponent<StatComponent>();
                if (stats != null && !stats.IsDead)
                {
                    stats.HealPercent(itemData.healthRecoveryPercent);
                    Debug.Log($"{player.name}에게 {itemData.itemName} 사용! 체력 {itemData.healthRecoveryPercent * 100}% 회복");
                }
            }
        }
    }

}
