using UnityEngine;

namespace LUP.ES
{
    public class ThrowingWeapon : Weapon
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 10; //임시
        private float nextAttackTime = 0f;
        public GameObject projectilePrefab;
        private BulletObjectPool projectilePool;
        [HideInInspector]
        public Transform playerTransform;
        private Transform firePointTransform;
        [HideInInspector]
        public FixedJoystick rightJoystick;
        public float gravity = 20.0f;
        public float timeToTarget = 0.8f;

        private bool isAiming = false;
        private Vector3 currentTargetPos;
        private Rigidbody playerRigidbody;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

        }

        public override void Init(int id)
        {
            selectedWeaponId = id;
            projectilePool = GetComponent<BulletObjectPool>();
            state = WeaponState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            ThrowingWeaponData weaponData = itemData as ThrowingWeaponData;
            weaponItem = new WeaponItem(weaponData);
            if (weaponData != null)
            {
                projectilePool.Init(projectilePrefab);
            }
            PlayerBlackboard playerBlackboard = FindAnyObjectByType<PlayerBlackboard>();
            playerRigidbody = playerBlackboard.GetComponent<Rigidbody>();
            playerTransform = playerBlackboard.transform;
            firePointTransform = playerTransform.Find("Fire Point");
            GameObject rightObj = GameObject.Find("Right Fixed Joystick");
            if (rightObj != null)
                rightJoystick = rightObj.GetComponent<FixedJoystick>();
        }


        public override bool Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }

            nextAttackTime = Time.time + weaponItem.data.timeBetAttack;
            ThrowingWeaponData data = weaponItem.data as ThrowingWeaponData;
            Vector3 targetPos = CalculateTargetPosition(data);

            GameObject obj = projectilePool.Get();
            ThrowerProjectile projectile = obj.GetComponent<ThrowerProjectile>();
            if (projectile != null)
            {
                projectile.Init(projectilePool, firePointTransform.position, Quaternion.identity, data.damage, data.attackRadius);
            }

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;

                Vector3 velocity = CalculateParabolaVelocity(firePointTransform.position, targetPos, timeToTarget);

                rb.linearVelocity = velocity;

            }
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

        private Vector3 CalculateTargetPosition(ThrowingWeaponData data)
        {
            Vector2 input = rightJoystick.Direction;

            float inputPower = input.magnitude;

            

            inputPower = Mathf.Clamp01(inputPower);
            Vector3 aimDir = new Vector3(input.x, 0, input.y).normalized;
            float currentDistance = inputPower * data.range;
            Vector3 aimOffset = aimDir * currentDistance;

            Vector3 baseTargetPos = transform.position + aimOffset;

            if (playerRigidbody != null)
            {
                Vector3 playerVel = playerRigidbody.linearVelocity; // 구버전: velocity
                playerVel.y = 0; // 점프 등 상하 움직임은 반영 제외 (조준 안정성)

                // 이동 보정값 = 내 속도 * 체공 시간
                Vector3 movementPrediction = playerVel * timeToTarget;

                return baseTargetPos + movementPrediction;
            }

            return baseTargetPos;
        }

        private Vector3 CalculateParabolaVelocity(Vector3 origin, Vector3 target, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0; // 수평 거리

            // 수평 속도 = 수평 거리 / 시간
            float sXZ = distanceXZ.magnitude;
            float Vxz = sXZ / time;

            // 수직 속도 = (수직 거리 / 시간) + (0.5 * 중력 * 시간)
            float Vy = (distance.y / time) + (0.5f * Mathf.Abs(gravity) * time);

            Vector3 result = distanceXZ.normalized * Vxz;
            result.y = Vy;

            return result;
        }
    }
}


