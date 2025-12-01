using JetBrains.Annotations;
using LUP.RL;
using System;
using UnityEngine;
namespace LUP.RL
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        public EnemyType Type => enemyType;
        public BaseStats EnemyStats;
        public int expValue = 10;
        public static event Action<int> OnEnemyDied;
        public delegate void EnemyDeathHandler(Enemy deadEnemy);
        public static event EnemyDeathHandler ObjectOnEnemyDied;
        public Vector2Int gridPos;
        private Hpbar hpbar;
        public GameObject HpbarPrefab;
        public HealthCenter healthSystem;
        private EnemyBlackBoard blackBoard;
        private EnemyBehaviorTree behaviorTree;
        [SerializeField] private float hpbaroffsetY = 5;
        public Transform TargetPoint;
        void Start()
        {
            EnemyStats.MaxHp = 50;
            EnemyStats.Hp = EnemyStats.MaxHp;
            EnemyStats.Attack = 0;
            EnemyStats.speed = 3;

            healthSystem = new HealthCenter(EnemyStats.MaxHp);
            if (healthSystem == null)
            {
                Debug.Log("health null");
                return;

            }
            GameObject barObj = Instantiate(HpbarPrefab, transform.position + Vector3.up * hpbaroffsetY, Quaternion.identity);
            if(barObj == null)
            {
                Debug.Log("bar¾øÀ½");
            }
            hpbar = barObj.GetComponent<Hpbar>();
            hpbar.Init(this);
            hpbar.SetHealthSystem(healthSystem);


            blackBoard = gameObject.GetComponent<EnemyBlackBoard>();
            behaviorTree = gameObject.GetComponent<EnemyBehaviorTree>();
        }
        public void SetGridPos(int x, int z)
        {
            gridPos = new Vector2Int(x, z);
        }
        public void TakeDamage(int damage)
        {
            healthSystem.Damage(damage);

            if(blackBoard)
            {
                if(blackBoard.InAtkState == false)
                    blackBoard.OnHitted = true;
            }
                

            if (healthSystem.CurrentHp <= 0)
            {
                Die();

              
                    
            }
        }
        private void Die()
        {
            OnEnemyDied?.Invoke(expValue);

            ObjectOnEnemyDied?.Invoke(this);

          
             blackBoard.Alive = false;

           
            behaviorTree.ResetWorkingNodeIndex();
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}