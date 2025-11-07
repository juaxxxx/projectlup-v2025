using UnityEngine;

namespace ES
{
    public class AttackingCondition : BTNode
    {
        PlayerBlackboard blackboard;

        public AttackingCondition(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.rightJoystick.Horizontal == 0 && blackboard.rightJoystick.Vertical == 0)
            {
                return NodeState.Failure;
            }
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}
