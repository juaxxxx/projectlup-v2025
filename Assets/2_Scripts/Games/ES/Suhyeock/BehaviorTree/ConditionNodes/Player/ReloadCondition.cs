using UnityEngine;

namespace LUP.ES
{
    public class ReloadCondition : BTNode
    {
        PlayerBlackboard blackboard;
        public ReloadCondition(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if(blackboard.weapon.weaponItem.data.weaponType == WeaponType.Melee)
                return NodeState.Failure;
            Gun gun = blackboard.weapon as Gun;
            if (gun == null)
                return NodeState.Failure;
            RangedWeaponItemData data = blackboard.weapon.weaponItem.data as RangedWeaponItemData;
            if (data != null && gun.magAmmo == data.magCapacity)
            {
                blackboard.isReloadButtonPressed = false;
                return NodeState.Failure;
            }

            if (blackboard.weapon.state == WeaponState.RELOADING)
            {
                return NodeState.Running;
            }
            if (gun.magAmmo <= 0 || blackboard.isReloadButtonPressed == true)
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
