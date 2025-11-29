using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace LUP.RL
{
    public class EnemyBlackBoard : BlackBoard
    {
       private Enemy enemy;
       private ShooterComp shooter;
        [SerializeField] private float FullAttackCoolTime = 2f;
        private void Start()
        {
            {
                enemy = GetComponent<Enemy>();
             
                Target = FindFirstObjectByType<PlayerMove>().gameObject;
                if (Target == null)
                    UnityEngine.Debug.LogWarning("Can't find Target(Plaeyr)");

                targetPos = Target.transform;
                if (enemy.Type == EnemyType.Ranged)
                {
                    shooter = enemy.GetComponent<ShooterComp>();
                }
            }


            {
                agent = gameObject.GetComponent<NavMeshAgent>();

                if (agent == null)
                    UnityEngine.Debug.LogWarning("Can't find Nav Component");
            }
            
        }

        public override void UpdateBlackBoard()
        {
            float deltaTime = Time.deltaTime;
            if (Target == null || targetPos == null)
            {
                return;
            }
            
            TargetDistance = Vector3.Distance(targetPos.position, gameObject.transform.position);

         

            AtkCollTime -= deltaTime * AtkCoolTimeRecoverySpeed;
            if (AtkCollTime < 0)
            
                AtkCollTime = 0;
            
            if (enemy.Type == EnemyType.Ranged && shooter != null)
            {
                if (AtkCollTime == 0)
                {
                    shooter.TryShoot(Target.transform, enemy.EnemyStats.Attack);

                    // 쿨타임 리필
                    AtkCollTime = FullAttackCoolTime;   
                }

                // 원거리 몬스터는 아래 로직 안 타게 종료
                return;
            }


            {
                if (OnRampage)
                {
                    HittedAccumTime -= deltaTime;
                }

                else
                {
                    HittedAccumTime -= deltaTime * 0.3f;
                }

                if (HittedAccumTime <= 0)
                {
                    HittedAccumTime = 0;

                    if (OnRampage == true)
                        OnRampage = false;
                }
                    
            }
            
        }
    }
}

