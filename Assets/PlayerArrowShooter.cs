using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
namespace RL
{
    public class PlayerArrowShooter : MonoBehaviour
    {
        //  화살 프리팹
        public GameObject arrowPrefab;
        //발사 위치
        public Transform spawnPoint;
        // 적 위치
        public Transform enemyTarget;
        //발사  속도
        public float fireDelay = 2f;
        public float arrowSpeed = 10f;
        public Archer archer;

        public Transform currentRoom;
        private float lastFireTime = 0f;
        void Update()
        {
            //일정시간마다 공격하게끔
            if (enemyTarget != null && Time.time - lastFireTime >= fireDelay)
            {
                ShootArrow();
                lastFireTime = Time.time;
            }
        }
        public void ShootArrow()
        {
            Enemy targetEnemy = FindClosestEnemy();
           
            if (targetEnemy == null) return;
            Debug.Log("화살생성");
            Transform targetTransform = targetEnemy ? targetEnemy.transform : null;
            //   화살생성
            GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);

            // 값전달  
            HomingArrow homing = arrow.GetComponent<HomingArrow>();
            homing.Initialize(archer, targetTransform, arrowSpeed, archer.Adata.currentData.Attack);

            if (targetTransform == null)
            {
                arrow.transform.position = spawnPoint.forward;
            }
            else
            {
                arrow.transform.LookAt(targetTransform);
            }
        }
        Enemy FindClosestEnemy()
        {
            if (currentRoom == null)
            {
                Debug.Log("방없음");
                return null;
            }

            Enemy[] enemies = currentRoom.GetComponentsInChildren<Enemy>(true);
            if (enemies.Length == 0)
            {
                Debug.Log("적이없음"); 
                    return null;
            }
            Enemy closest = null;
            float minDist = Mathf.Infinity;
            
            foreach (var e in enemies)
            {
                if (e == null) continue;
                float dist = Vector3.Distance(transform.position, e.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = e;
                }
            }

            return closest;
        }
        private void OnEnable()
        {
            Enemy.ObjectOnEnemyDied += HandleEnemyDeath;
        }

        private void OnDisable()
        {
            Enemy.ObjectOnEnemyDied -= HandleEnemyDeath;
        }

        private void HandleEnemyDeath(Enemy dead)
        {
            Debug.Log($"{dead.name} 사망 감지!");
        }

    }
}