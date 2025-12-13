using UnityEngine;

namespace LUP.ES
{

    public class MeleeWeapon : Weapon
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 5; //임시
        public LayerMask targetLayer;
        public Transform playerTransform;
        private float nextAttackTime = 0f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            state = WeaponState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            MeleeWeaponItemData weaponData = itemData as MeleeWeaponItemData;
            weaponItem = new WeaponItem(weaponData);
        }

        // Update is called once per frame
        void Update()
        {
            


        }

        public override bool Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }

            nextAttackTime = Time.time + weaponItem.data.timeBetAttack;

            
            Collider[] colliders = Physics.OverlapSphere(playerTransform.position, weaponItem.data.range, targetLayer);

            foreach (Collider target in colliders)
            {
                Vector3 directionToTarget = (target.transform.position - playerTransform.position).normalized;

                float angle = Vector3.Angle(playerTransform.forward, directionToTarget);


                MeleeWeaponItemData data = weaponItem.data as MeleeWeaponItemData;
                if (angle < data.attackAngle * 0.5f)
                {
                    Debug.Log("In Angle");
                    HealthComponent healthComponent = target.GetComponent<HealthComponent>();
                    if (healthComponent)
                    {
                        Debug.Log("Melee Attack");
                        healthComponent.TakeDamage(data.damage);
                    }
                }
            }
            return true;

        }

        //private void OnDrawGizmos()
        //{
        //    if (Application.isPlaying)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawWireSphere(playerTransform.position, weaponItem.data.range);
        //    }
        //}
    }

}
