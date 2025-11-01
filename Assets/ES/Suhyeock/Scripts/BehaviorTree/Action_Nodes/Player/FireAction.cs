using UnityEngine;

public class FireAction : BTNode
{
    private PlayerBlackboard blackboard;
    private CharacterController characterController;

    public FireAction(PlayerBlackboard blackboard, CharacterController characterController)
    {
        this.blackboard = blackboard;
        this.characterController = characterController;
    }

    public override NodeState Evaluate()
    {
        float horizontal = blackboard.rightJoystick.Horizontal;
        float Vertical = blackboard.rightJoystick.Vertical;

        if (horizontal != 0 || Vertical != 0)
        {
            Vector3 dir = new Vector3(horizontal, 0f, Vertical);
            dir.Normalize();
            characterController.transform.forward = dir;
            //blackboard.isFiring = true;
            blackboard.gun.Fire();
            blackboard.playerOverheadUI.UpdateAmmoUI();
            blackboard.gun.state = GunState.ATTACKING;
            return NodeState.Running;
        }
        blackboard.gun.state = GunState.READY;
        return NodeState.Success;
    }
}
