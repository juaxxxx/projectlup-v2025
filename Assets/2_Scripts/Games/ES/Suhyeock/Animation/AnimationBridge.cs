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

        public void OnChargingStart()
        {
            if (blackboard != null)
            {
                ThrowingWeapon throwingWeapon = blackboard.weapon as ThrowingWeapon;
                if (throwingWeapon)
                {
                    throwingWeapon.SetIsCharging(true);
                    blackboard.animator.SetFloat("ThrowSpeed", 0f);
                }
            }
        }

        public void OnThrowStart()
        {
            if (blackboard != null)
            {
                ThrowingWeapon throwingWeapon = blackboard.weapon as ThrowingWeapon;
                if (throwingWeapon)
                {
                    throwingWeapon.ThrowStart();
                }
            }
        }
    }

}
