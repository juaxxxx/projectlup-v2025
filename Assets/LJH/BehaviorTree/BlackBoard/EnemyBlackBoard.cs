using Unity.VisualScripting;
using UnityEngine;

namespace RL
{
    public class EnemyBlackBoard : BlackBoard
    {
        private void Start()
        {
            Target = FindFirstObjectByType<PlayerMove>().gameObject;

            if (Target == null)
                UnityEngine.Debug.LogWarning("Can't find Target(Plaeyr)");

            targetPos = Target.transform;
        }

        public override void UpdateBlackBoard()
        {
            TargetDistance = Vector3.Distance(targetPos.position, gameObject.transform.position);

            AtkCollTime = AtkCollTime - Time.deltaTime * AtkCoolTimeRecoverySpeed;
            if (AtkCollTime < 0)
                AtkCollTime = 0;
        }
    }
}

