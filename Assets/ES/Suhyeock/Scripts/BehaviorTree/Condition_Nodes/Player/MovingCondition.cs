using UnityEngine;

public class MovingCondition : BTNode
{
    private PlayerBlackboard blackboard;
    public MovingCondition(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.leftJoystick.Horizontal == 0 && blackboard.leftJoystick.Vertical == 0)
        {
            return NodeState.Failure;
        }
        return NodeState.Success;
    }
}
