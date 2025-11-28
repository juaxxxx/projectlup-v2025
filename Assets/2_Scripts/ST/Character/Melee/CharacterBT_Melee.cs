using System.Collections.Generic;
using UnityEngine;
namespace LUP.ST
{
    public class CharacterBT_Melee : BehaviorTreeBase
    {
        private MeleeBlackBoard bb;
        private MeleeActions act;

        protected override BaseNode SetupTree()
        {
            bb = GetComponent<MeleeBlackBoard>();
            act = GetComponent<MeleeActions>();

            // 액션 래핑 (이름은 다이어그램 라벨과 1:1 매칭)
            NodeState Retire()            { return act.Retire(); }
            NodeState Cover()             { return act.Cover(); }
            NodeState Reload()            { return act.Reload(); }
            NodeState MeleeAttack()       { return act.MeleeAttackLoop(); }
            NodeState MoveToEnemy()       { return act.MoveToEnemy(); }
            NodeState Idle()              { return act.Idle(); }

            // 루트 셀렉터
            Selector root = new Selector(new List<BaseNode>
            {
                new Decorator(() => bb.IsHpZero(), new ActionNode(Retire)),

                new Decorator(() => !bb.HasAttackChance(), new Sequence(new List<BaseNode> {
                    new ActionNode(Cover),
                    new ActionNode(Reload)
                })),

                new Decorator(() => bb.CanAttack(), new ActionNode(MeleeAttack)),

                new Decorator(() => bb.IsEnemyWithinDetectionRange(), new ActionNode(MoveToEnemy)),

                new Decorator(() => bb.ShouldReturnByNoEnemy(), new ActionNode(Cover)),

                new ActionNode(Idle)
            });

            return root;
        }
    }

}