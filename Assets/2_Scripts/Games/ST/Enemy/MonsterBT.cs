using UnityEngine;
using System.Collections.Generic;
namespace LUP.ST
{

    public class MonsterBT : BehaviorTreeBase
    {
        private MonsterData data;

        void Awake()
        {
            data = GetComponent<MonsterData>();
        }

        protected override BaseNode SetupTree()
        {
            BaseNode root = new Selector(new List<BaseNode>
        {
            // 1. Dead
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckHPZero(data)),
                new ActionNode(() => MonsterActions.Dead(data))
            }),

            // 2. Stunned
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckIsStunned(data)),
                new ActionNode(() => MonsterActions.Idle(data))
            }),

            // 3. Using Skill
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckIsUsingSkill(data)),
                new ActionNode(() => MonsterActions.Idle(data))
            }),

            // 4. Attack
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckInAttackRange(data)),
                new ActionNode(() => MonsterActions.Attack(data))
            }),

            // 5. Hide
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckHPBelow30(data)),
                new ConditionNode(() => MonsterConditions.CheckCoverAhead(data)),
                new ConditionNode(() => MonsterConditions.CheckHideCooldownOK(data)),
                new ActionNode(() => MonsterActions.Hide(data))
            }),

            // 6. Move (Fallback)
            new ActionNode(() => MonsterActions.MoveToPlayer(data))
        });

            return root;
        }
    }
}