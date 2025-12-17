using LUP.ES;
using Roguelike.Define;
using UnityEngine;

namespace LUP.RL
{
    public class ShooterComp : MonoBehaviour
    {
        public FireSystem fireSystem;
        public WeaponHand weapon;
        public MeleeSystem meleeWeapon;


        public void TryAttack(Transform target, int damage)
        {
            if (!weapon)
            {
                Debug.Log("nullweapon");
                return;

            }
            switch (weapon.weaponType)
            {
                case RWeaponType.Throw:
                    fireSystem.TryFire(target, damage);

                    break;
                case RWeaponType.TwoHandSword:
                    meleeWeapon.MeleeAttack(damage);

                    break;
                    //case RWeaponType.OneHandSword;


            }


        }
     
    }


    }
