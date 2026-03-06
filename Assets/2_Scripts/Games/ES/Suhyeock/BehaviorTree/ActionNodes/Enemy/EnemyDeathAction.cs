using DG.Tweening;
using Unity.Collections;
using UnityEngine;

namespace LUP.ES
{
    public class EnemyDeathAction : BTNode
    {
        EnemyBlackboard blackboard;
        float deathTime = 3f;

        bool isUpdatedUI = false;
        bool isDisappearing = false;
        public EnemyDeathAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (isUpdatedUI == false)
            {
                blackboard.enemyHPUI.UpdateHPUI();
                blackboard.enemyHPUI.UIInstance.SetActive(true);
            }
            blackboard.navMeshAgent.speed = 0;
            deathTime -= Time.deltaTime;
            blackboard.ChangeState(EnemyState.Death);
            if (deathTime < 0 && !isDisappearing)
            {
                isDisappearing = true;
                blackboard.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        RangedEnemyBlackboard enemyBlackboard = blackboard as RangedEnemyBlackboard;
                        if (enemyBlackboard != null)
                        {
                            enemyBlackboard.gun.Destroy();
                        }
                        if (blackboard.lootSpawner != null)
                        {
                            blackboard.lootSpawner.SpawnLoot();
                        }
                        Object.Destroy(blackboard.enemyHPUI.UIInstance);
                        Object.Destroy(blackboard.gameObject);
                    });
            }
            return NodeState.Running;
        }

        public override void Reset()
        {

        }
    }
}
