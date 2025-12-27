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

        public AnimatorOverrideController overrideController;

        public WeaponHandType handType = WeaponHandType.None;

        public Vector3 weaponRightHandGrapPos;
        public Vector3 weaponRotate;

        public float ikWeight = 1.0f;
    }
}


