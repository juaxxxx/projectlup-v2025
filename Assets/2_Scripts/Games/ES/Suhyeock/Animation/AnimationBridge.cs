using UnityEngine;

namespace LUP.ES
{
    public class AnimationBridge : MonoBehaviour
    {
        private Weapon weapon;
        private PlayerBlackboard blackboard;
        public void SetWeapon()
        {
            weapon = GetComponentInChildren<Weapon>();
            blackboard = GetComponentInParent<PlayerBlackboard>();
        }

        public void OnAttackStart()
        {
            if (weapon != null)
            {
                weapon.Attack();
            }
        }

        public void OnAttackEnd()
        {
            if (blackboard != null)
            {
                blackboard.weapon.state = WeaponState.READY;
            }
        }
    }

}
