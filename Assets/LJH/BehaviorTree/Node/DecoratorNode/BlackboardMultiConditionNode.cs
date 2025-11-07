using System.Collections.Generic;
using UnityEngine;

namespace RL
{
    public enum ConditionMode
    {
        AND,
        OR
    }

    public class BlackboardMultiConditionNode : DecoratorNode
    {
        private readonly BlackBoar currBlackBoard;
        private readonly List<(ConditionCheckEnum condition, bool whishConditions)> conditionList;

        public BlackboardMultiConditionNode(BlackBoar targetBlackBoard, List<(ConditionCheckEnum, bool)> conditions, Node decoratedNode) : base(decoratedNode)
        {
            currBlackBoard = targetBlackBoard;
            conditionList = conditions;
        }

        public override NodeState Evaluate()
        {

            foreach (var (condition, whishCondition) in conditionList)
            {
                if (EvaluateCondition(condition) != whishCondition)
                    return NodeState.Fail;
            }

            return targetNode.Evaluate();
        }

        private bool EvaluateCondition(ConditionCheckEnum condition)
        {
            return condition switch
            {
                ConditionCheckEnum.ISALIVE => currBlackBoard.Alive,
                ConditionCheckEnum.INATKSTATE => currBlackBoard.InAtkState,
                ConditionCheckEnum.INHITTEDSTATE => currBlackBoard.OnHitted,
                ConditionCheckEnum.INRAMPAGE => currBlackBoard.OnRampage,
                ConditionCheckEnum.HASTARGET => currBlackBoard.HasTarget,
                ConditionCheckEnum.INREADYTOATK => currBlackBoard.AtkCollTime <= 0,
                ConditionCheckEnum.TargetINATKRANGE => currBlackBoard.AtkRange > currBlackBoard.TargetDistance,
                ConditionCheckEnum.ISLOCALLYCONTROLLED => currBlackBoard.isLocallyControlled,
                _ => false,
            };
        }
    }
}
