using UnityEngine;
using Roguelike.Define;

namespace LUP.RL
{
    public class WeaponHand : MonoBehaviour
    {
        public RWeaponType weaponType = RWeaponType.None;
        public Transform weaponHandPos;
        public Vector3 weaponPos;
        public Vector3 rotate;
    }
}

