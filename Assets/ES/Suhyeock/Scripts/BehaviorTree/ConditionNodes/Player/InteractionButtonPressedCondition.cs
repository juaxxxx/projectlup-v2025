using UnityEngine;

namespace ES
{
    public class InteractionButtonPressedCondition : BTNode
    {
        PlayerBlackboard blackboard;
        public InteractionButtonPressedCondition(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }
        public override NodeState Evaluate()
        {
            if (blackboard.isInteractionButtonPressed == true)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        public override void Reset()
        {

        }
    }
}
