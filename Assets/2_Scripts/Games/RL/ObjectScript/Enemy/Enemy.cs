using JetBrains.Annotations;
using LUP.RL;
using Roguelike.Define;
using System;
//using UnityEditor.ShaderGraph;
using UnityEngine;
namespace LUP.RL
{
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Info")]
        [SerializeField] private EnemyType enemyType;
        public EnemyType Type => enemyType;


        [Header("Stats")]
        public BaseStats EnemyStats;
        public int expValue = 10;

        [Header("Grid")]
        public Vector2Int gridPos;

        [Header("Hpbar")]
        public GameObject hpbarPrefab;
        [SerializeField] private float hpbarOffsetY = 5f;
        private Hpbar hpbar;
        
        public static event Action<int> OnEnemyDied;
        public static event Action<Enemy> ObjectOnEnemyDied;
        

        public delegate void EnemyDeathHandler(Enemy deadEnemy);
 

        [Header("Runtime")]
        public HealthCenter healthCenter;
        private EnemyBlackBoard blackBoard;
        private EnemyBehaviorTree behaviorTree;
        public GameObject hitEffectPrefab;
        public Transform TargetPoint;
        private void Awake()
        {
            EnemyStats.MaxHp = 20;
            EnemyStats.Hp = EnemyStats.MaxHp;
            EnemyStats.Attack = 5;
            EnemyStats.speed = 3;
            healthCenter = new HealthCenter(EnemyStats.MaxHp);
        }
        void Start()
        {
          
            GameObject barObj = Instantiate(hpbarPrefab, transform.position + Vector3.up * hpbarOffsetY, Quaternion.identity);
            if(barObj == null)
            {
                Debug.Log("bar¾øÀ½");
            }
            hpbar = barObj.GetComponent<Hpbar>();
            hpbar.Init(this);
            //hpbar.SetHealthSystem(healthCenter);


            blackBoard = gameObject.GetComponent<EnemyBlackBoard>();
            behaviorTree = gameObject.GetComponent<EnemyBehaviorTree>();
        }
        public void SetGridPos(int x, int z)
        {
            gridPos = new Vector2Int(x, z);
        }

        public void TakeDamage(int damage, GameObject effect)
        {
            healthCenter.Damage(damage);
            if (effect)
            {
                var fx = Instantiate(effect, TargetPoint.position, Quaternion.identity);

                fx.transform.localScale *= 1.5f;
            }
            //Instantiate(hitEffectPrefab, TargetPoint.position, Quaternion.identity);
            
            if (blackBoard)
            {
                if(blackBoard.InAtkState == false)
                    blackBoard.OnHitted = true;
            }

        }
        private void OnEnable()
        {
            healthCenter.OnDead += Die;
        }
        private void OnDisable()
        {
            healthCenter.OnDead -= Die;
        }
        private void Die()
        {
             blackBoard.Alive = false;

            hpbar.DisableHPBar(healthCenter);

            ObjectOnEnemyDied?.Invoke(this);
            OnEnemyDied?.Invoke(expValue);

            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}