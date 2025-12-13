using UnityEngine;

namespace LUP.ES
{
    public class AttackAction : BTNode
    {
        private PlayerBlackboard blackboard;
        private CharacterController characterController;

        public AttackAction(PlayerBlackboard blackboard, CharacterController characterController)
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
                //Vector3 dir = new Vector3(horizontal, 0f, Vertical);
                //dir.Normalize();
                //characterController.transform.forward = dir;
                bool isAttack = blackboard.weapon.Attack();
                blackboard.playerOverheadUI.UpdateAmmoUI();
                blackboard.weapon.state = WeaponState.ATTACKING;
                if (blackboard.animator != null && isAttack)
                {
                    //blackboard.animator.SetBool("IsAttacking", true);
                    blackboard.animator.SetTrigger("Attack");
                }
                else
                {
                    blackboard.animator.ResetTrigger("Attack");
                }
                    return NodeState.Running;
            }
            blackboard.weapon.state = WeaponState.READY;
            return NodeState.Success;
        }

        public override void Reset()
        {
            if (blackboard.animator != null)
            {
                blackboard.animator.ResetTrigger("Attack");
            }
        }
    }
}

