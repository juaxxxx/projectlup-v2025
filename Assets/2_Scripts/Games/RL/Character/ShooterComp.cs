using LUP.ES;
using Roguelike.Define;
using UnityEngine;

namespace LUP.RL
{
    public class ShooterComp : MonoBehaviour
    {
        public FireSystem fireSystem;
        public WeaponData weapon;
        public MeleeSystem meleeWeapon;


        public void TryAttack(Transform target, int damage)
        {
            Debug.Log("try attack");
            if (!weapon)
            {
                Debug.Log("nullweapon");
                return;

            }
            switch (weapon.weaponType)
            {

                case RWeaponType.Throw:
                    Debug.Log("try fire");
                    fireSystem.TryFire(target, damage);
                    break;
                case RWeaponType.TwoHandSword:
                    meleeWeapon.MeleeAttack(damage);
                    break;
                case RWeaponType.OneHandSword:
                    fireSystem.TryFire(target, damage);
                    break;
                case RWeaponType.Gun:
                    fireSystem.TryFire(target, damage);
                    break;
                case RWeaponType.Magic:
                    fireSystem.TryFire(target, damage);
                    break;

                    //case RWeaponType.OneHandSword;


            }


        }
     
    }


    }
