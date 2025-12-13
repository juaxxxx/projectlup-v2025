using UnityEngine;
using static LUP.ES.PrefabDataBase;


namespace LUP.ES
{
    public class WeaponEquip : MonoBehaviour
    {
        public PrefabDataBase prefabDataBase;
        private Animator animator;
        private PlayerBlackboard blackboard;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init()
        {
            blackboard = GetComponent<PlayerBlackboard>();
            animator = blackboard.animator;
            EqipWeapon();
        }
        void EqipWeapon()
        {
            Transform handTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
            ItemPrefabEntry weaponEntry = prefabDataBase.GetEntry(blackboard.CurrentWeaponID);
            GameObject newWeapon = Instantiate(weaponEntry.prefab);
            newWeapon.transform.SetParent(handTransform);

            newWeapon.transform.localPosition = weaponEntry.positionOffset;
            newWeapon.transform.localRotation = Quaternion.Euler(weaponEntry.rotationOffset);
            newWeapon.transform.localScale = Vector3.one;
            Gun newWeaponComp = newWeapon.GetComponent<Gun>();
            newWeaponComp.Init(blackboard.CurrentWeaponID);
            blackboard.weapon = newWeaponComp;
        }

    }
}
