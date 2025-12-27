using UnityEngine;
using Roguelike.Define;

namespace LUP.RL
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "RLGame/WeaponData")]
    public class WeaponData : ScriptableObject
    {

        public int ItemID = 0;

        public GameObject weaponPrefab;

        public GameObject weaponProjecTile = null;
        public int projecTileSpeed = 0;

        public AnimatorOverrideController overrideController;

        public RWeaponType weaponType;
        public WeaponHandType handType = WeaponHandType.None;

        public Vector3 IdleweaponRightHandGrapPos;
        public Vector3 IdleweaponRotate;

        public float ikWeight = 1.0f;
    }
}


