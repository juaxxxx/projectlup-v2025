using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
                var playerMove = FindFirstObjectByType<PlayerMove>();

                if (playerMove == null)
                {
                    Debug.LogWarning("No PlayerMove found");
                    return;
                }


                Target = FindFirstObjectByType<PlayerMove>().gameObject;
               
                if (Target == null)
                    UnityEngine.Debug.LogWarning("Can't find Target(Plaeyr)");

                targetPos = playerMove.targetPoint;
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
                    RotateHelper.LookAtTarget(enemy.transform, targetPos, 8f);
                    shooter.TryShoot(targetPos, enemy.EnemyStats.Attack);

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

