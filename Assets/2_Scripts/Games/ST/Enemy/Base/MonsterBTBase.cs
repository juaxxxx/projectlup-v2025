using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{
    public abstract class MonsterBTBase : BehaviorTreeBase
    {
        protected MonsterData data;

        protected virtual void Awake()
        {
            data = GetComponent<MonsterData>();
        }

        protected BaseNode DeadSequence()
        {
            return new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckHPZero(data)),
                new ActionNode(() => MonsterActions.Dead(data))
            });
        }

        protected BaseNode StunnedSequence()
        {
            return new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckIsStunned(data)),
                new ActionNode(() => MonsterActions.Idle(data))
            });
        }

        protected BaseNode UsingSkillSequence()
        {
            return new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckIsUsingSkill(data)),
                new ActionNode(() => MonsterActions.Idle(data))
            });
        }

        protected BaseNode AttackSequence()
        {
            return new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => MonsterConditions.CheckInAttackRange(data)),
                new ActionNode(() => MonsterActions.Attack(data))
            });
        }

        protected BaseNode MoveToPlayerAction()
        {
            return new ActionNode(() => MonsterActions.MoveToPlayer(data));
        }
    }
}