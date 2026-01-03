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
                switch (blackboard.weapon.weaponItem.data.weaponType)
                {
                    case WeaponType.Ranged:
                        bool isAttack = blackboard.weapon.Attack();
                        if (blackboard.animator != null && isAttack)
                            blackboard.animator.SetTrigger("Attack");
                        blackboard.weapon.state = WeaponState.ATTACKING;
                        break;
                    case WeaponType.Melee: case WeaponType.Throwing:
                        if (blackboard.weapon.CanAttack() && blackboard.weapon.state == WeaponState.READY)
                        {
                            if (blackboard.animator != null)
                            {
                                blackboard.animator.SetTrigger("Attack");
                                blackboard.weapon.state = WeaponState.ATTACKING;
                            }
                        }
                        break;
                    default:
                        break;
                }
                blackboard.playerOverheadUI.UpdateAmmoUI();
                
                return NodeState.Running;
            }

            if (blackboard.weapon.weaponItem.data.weaponType != WeaponType.Ranged && blackboard.weapon.state == WeaponState.ATTACKING)
            {
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

