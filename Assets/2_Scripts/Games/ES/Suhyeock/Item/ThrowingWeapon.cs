using UnityEngine;

namespace LUP.ES
{
    public class ThrowingWeapon : Weapon
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 10; //임시
        private float nextAttackTime = 0f;
        [HideInInspector]
        public Transform playerTransform;
        public float gravity = 9.81f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

        }

        public override void Init(int id)
        {
            selectedWeaponId = id;
            state = WeaponState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            ThrowingWeaponData weaponData = itemData as ThrowingWeaponData;
            weaponItem = new WeaponItem(weaponData);
            playerTransform = FindAnyObjectByType<PlayerBlackboard>().transform;
        }

        public override bool Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }

            nextAttackTime = Time.time + weaponItem.data.timeBetAttack;


            return true;
        }

        public override bool CanAttack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }
            return true;
        }
        public override void SetWeaponVisible(bool isVisible)
        {
            if (weaponRenderers != null)
            {
                foreach (Renderer r in weaponRenderers)
                {
                    r.enabled = isVisible;
                }
            }
        }
    }
}


