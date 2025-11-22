using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace LUP.RL
{
    public class EnemyBlackBoard : BlackBoard
    {
        private void Start()
        {
            {
                Target = FindFirstObjectByType<PlayerMove>().gameObject;

                if (Target == null)
                    UnityEngine.Debug.LogWarning("Can't find Target(Plaeyr)");

                targetPos = Target.transform;
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
                Debug.Log("xxx");
                return;
            }
            TargetDistance = Vector3.Distance(targetPos.position, gameObject.transform.position);



            AtkCollTime = AtkCollTime - deltaTime * AtkCoolTimeRecoverySpeed;
            if (AtkCollTime < 0)
                AtkCollTime = 0;


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

