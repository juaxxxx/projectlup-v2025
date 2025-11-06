using UnityEngine;

namespace RL
{
    public enum ConditionCheckEnum
    {
        None,
        ISALIVE,
        INREADYTOATK,
        TARTINATKRANGE,
        INATKREADY,
        INATKSTATE,
        INHITTEDSTATE,
        INRAMPAGE,
        HASTARGET,
        ISLOCALLYCONTROLLED
    }   

    public class BlackboardConditionNode : DecoratorNode
    {
        IBlackBoardAble currBlackBoard;
        ConditionCheckEnum evaluateCondition = ConditionCheckEnum.None;
        bool whishCondition = false;
        public BlackboardConditionNode(IBlackBoardAble targetBlackBoard, ConditionCheckEnum evaluatedCondition, bool WhishCondition , Node decoratedNode) : base(decoratedNode)
        {
            //if (targetBlackBoard is EnemyBlackBoard)
            //{
            //    currBlackBoard = (EnemyBlackBoard)targetBlackBoard;
            //}

            currBlackBoard = targetBlackBoard;

            this.whishCondition = WhishCondition;
            this.evaluateCondition = evaluatedCondition;
        }

        public override NodeState Evaluate()
        {
            bool TargetCondition = false;

            switch(evaluateCondition)
            {
                case ConditionCheckEnum.ISALIVE:
                    TargetCondition = currBlackBoard.Alive;
                    break;
                case ConditionCheckEnum.INREADYTOATK:
                    TargetCondition = currBlackBoard.CanAtk;
                    break;
                case ConditionCheckEnum.INATKSTATE:
                    TargetCondition = currBlackBoard.OnAtk;
                    break;
                case ConditionCheckEnum.INHITTEDSTATE:
                    TargetCondition = currBlackBoard.OnHitted;
                    break;
                case ConditionCheckEnum.INRAMPAGE:
                    TargetCondition = currBlackBoard.OnRampage;
                    break;
                case ConditionCheckEnum.HASTARGET:
                    TargetCondition = currBlackBoard.HasTarget;
                    break;
                case ConditionCheckEnum.INATKREADY:
                    TargetCondition = currBlackBoard.AtkCollTime > 0;
                    break;
                case ConditionCheckEnum.TARTINATKRANGE:
                    TargetCondition = currBlackBoard.AtkRange > currBlackBoard.TargetDistance;
                    break;
                case ConditionCheckEnum.ISLOCALLYCONTROLLED:
                    TargetCondition = currBlackBoard.isLocallyControlled;
                    break;

            }

            if (TargetCondition == whishCondition)
                return NodeState.Success;

            else
                return NodeState.Fail;

        }
    }
}

