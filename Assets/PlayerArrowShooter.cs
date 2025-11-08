using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
namespace LUP.RL
{
    public class PlayerArrowShooter : MonoBehaviour
    {
        //  화살 프리팹
        public GameObject arrowPrefab;
        //발사 위치
        [Header("SpawnPoint 할당")]
        public Transform spawnPoint;
        // 적 위치
        private Transform enemyTarget;
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
            spawnPoint.rotation = Quaternion.LookRotation(this.transform.forward);
            Enemy targetEnemy = FindClosestEnemy();

            Vector3 fireDir;
            if (targetEnemy != null)
                fireDir = (targetEnemy.transform.position - this.transform.position).normalized;
            else
                fireDir = transform.forward;

            fireDir.y = 0f;
            if (fireDir.sqrMagnitude > 0.01f)
            {
                //fireDir을 바라보는 회전값 쿼터니언 생성.
                Quaternion lookRot = Quaternion.LookRotation(fireDir);
                // 보간
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 1.5f);
            }
            //   화살생성
            GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.LookRotation(fireDir));

            // 값전달  
            HomingArrow homing = arrow.GetComponent<HomingArrow>();
            homing.Initialize(archer, targetEnemy.transform, arrowSpeed, archer.Adata.currentData.Attack);

        }
        Enemy FindClosestEnemy()
        {
            if (currentRoom == null)
            {
                return null;
            }
            Enemy[] enemies = currentRoom.GetComponentsInChildren<Enemy>(true);
            if (enemies.Length == 0)
            {
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