using JetBrains.Annotations;
using System;
using UnityEngine;
namespace RL
{
    public class Enemy : MonoBehaviour
    {
        public BaseStats EnemyStats;
        public int expValue = 10;
        public static event Action<int> OnEnemyDied;
        public delegate void EnemyDeathHandler(Enemy deadEnemy);
        public static event EnemyDeathHandler ObjectOnEnemyDied;
        public Vector2Int gridPos;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            EnemyStats.Hp = 20;
            EnemyStats.Attack = 0;
            EnemyStats.speed = 3;

            Debug.Log($"enemy생성  체력  :  {EnemyStats.Hp}");
        }
        public  void SetGridPos(int x, int z)
        {
            gridPos = new Vector2Int(x, z);
        }
        public void TakeDamage(int damage)
        {
            EnemyStats.Hp -= damage;
            Debug.Log($"데미지 : {damage} 남은체력 {EnemyStats.Hp}");
            if (EnemyStats.Hp <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            OnEnemyDied?.Invoke(expValue);

            ObjectOnEnemyDied?.Invoke(this);


            Destroy(gameObject);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}