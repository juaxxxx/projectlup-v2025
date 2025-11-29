using UnityEngine;

namespace LUP.RL
{
    public enum ConditionCheckEnum
    {
        None,

        ISALIVE,

        INREADYTOATK,
        TargetINATKRANGE,
        INATKSTATE,
        OnHit,
        INHITTEDSTATE,
        INRAMPAGE,
        HASTARGET,
        ISLOCALLYCONTROLLED
    }   

    public class BlackboardConditionNode : DecoratorNode
    {
        BlackBoard currBlackBoard;
        ConditionCheckEnum evaluateCondition = ConditionCheckEnum.None;
        bool whishCondition = false;
        public BlackboardConditionNode(BlackBoard targetBlackBoard, ConditionCheckEnum evaluatedCondition, bool WhishCondition , Node decoratedNode) : base(decoratedNode)
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
                    TargetCondition = currBlackBoard.AtkCollTime <= 0;
                    break;
                case ConditionCheckEnum.INATKSTATE:
                    TargetCondition = currBlackBoard.InAtkState;
                    break;


                case ConditionCheckEnum.OnHit:
                    TargetCondition = currBlackBoard.OnHitted;
                    break;
                case ConditionCheckEnum.INHITTEDSTATE:
                    TargetCondition = currBlackBoard.InHittedState;
                    break;
                case ConditionCheckEnum.INRAMPAGE:
                    TargetCondition = currBlackBoard.OnRampage;
                    break;



                case ConditionCheckEnum.HASTARGET:
                    TargetCondition = currBlackBoard.HasTarget;
                    break;

                case ConditionCheckEnum.TargetINATKRANGE:
                    TargetCondition = currBlackBoard.AtkRange > currBlackBoard.TargetDistance;
                    break;

                case ConditionCheckEnum.ISLOCALLYCONTROLLED:
                    TargetCondition = currBlackBoard.isLocallyControlled;
                    break;

            }

            if (TargetCondition == whishCondition)
                return targetNode.Evaluate();

            else
                return NodeState.Fail;

        }
    }
}

