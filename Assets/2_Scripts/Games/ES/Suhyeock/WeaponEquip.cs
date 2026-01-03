using UnityEngine;
using static LUP.ES.PrefabDataBase;


namespace LUP.ES
{
    public class WeaponEquip : MonoBehaviour
    {
        public PrefabDataBase prefabDataBase;
        public bool isRightHand = true;
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
            Transform handTransform;
            if (isRightHand)
            {
                handTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
            }
            else
            {
                handTransform = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            }

            ItemPrefabEntry weaponEntry = prefabDataBase.GetEntry(blackboard.CurrentWeaponID);
            GameObject newWeapon = Instantiate(weaponEntry.prefab);
            newWeapon.transform.SetParent(handTransform);

            newWeapon.transform.localPosition = weaponEntry.positionOffset;
            newWeapon.transform.localRotation = Quaternion.Euler(weaponEntry.rotationOffset);
            newWeapon.transform.localScale = weaponEntry.prefab.transform.localScale;
            Weapon newWeaponComp = newWeapon.GetComponent<Weapon>();
            if (newWeaponComp != null)
            {
                newWeaponComp.Init(blackboard.CurrentWeaponID);
                blackboard.weapon = newWeaponComp;
            }
            AnimationBridge animationBridge = GetComponentInChildren<AnimationBridge>();
            if (animationBridge != null)
                animationBridge.SetWeapon();
        }

    }
}
