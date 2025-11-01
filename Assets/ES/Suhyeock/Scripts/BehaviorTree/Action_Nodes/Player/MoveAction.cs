using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAction : BTNode
{
    private PlayerBlackboard blackboard;
    private CharacterController characterController;
    public MoveAction( PlayerBlackboard blackboard, CharacterController characterController)
    {
        this.blackboard = blackboard;
        this.characterController = characterController;
    }

    public override NodeState Evaluate()
    {
        float horizontal = blackboard.leftJoystick.Horizontal;
        float Vertical = blackboard.leftJoystick.Vertical;

        if (horizontal != 0 || Vertical != 0)
        {
            Vector3 dir = new Vector3(horizontal, 0f, Vertical);
            dir.Normalize();

            characterController.Move(dir * blackboard.speed * Time.deltaTime);

            if (dir != Vector3.zero && blackboard.gun.state != GunState.ATTACKING)
                characterController.transform.forward = dir; //플레이어의 이동 방향으로 회전

            blackboard.moveState = MoveState.MOVING;
            return NodeState.Running;
        }
        blackboard.moveState = MoveState.IDLE;
        return NodeState.Success;
    }
}
