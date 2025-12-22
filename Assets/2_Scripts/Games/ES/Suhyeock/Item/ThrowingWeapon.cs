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
        //public float gravity = 20.0f;
        public float timeToTarget = 0.8f;

        private Rigidbody playerRigidbody;
        private bool isCharging = false;
        [HideInInspector]
        public float currentChargeTime = 0f;
        private Vector3 lastAimDirection;
        private ThrowingWeaponData weaponData;
        private Animator animator;

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
            weaponData = itemData as ThrowingWeaponData;
            weaponItem = new WeaponItem(weaponData);
            if (weaponData != null)
            {
                projectilePool.Init(projectilePrefab);
            }
            PlayerBlackboard playerBlackboard = FindAnyObjectByType<PlayerBlackboard>();
            animator = playerBlackboard.animator;
            playerRigidbody = playerBlackboard.GetComponent<Rigidbody>();
            playerTransform = playerBlackboard.transform;
            firePointTransform = playerBlackboard.GetComponentInChildren<ThrowTrajectoryVisualizer>().transform;
            GameObject rightObj = GameObject.Find("Right Fixed Joystick");
            if (rightObj != null)
                rightJoystick = rightObj.GetComponent<FixedJoystick>();
            ThrowTrajectoryVisualizer visualizer = playerBlackboard.GetComponentInChildren<ThrowTrajectoryVisualizer>();
            if (visualizer != null)
                visualizer.SetWeapon(this);

        }

        private void Update()
        {
            if (isCharging)
            {
                HandleChargeInput();
            }
        }

        private void HandleChargeInput()
        {
            if (rightJoystick.Direction.magnitude > 0.01f)
            {
                currentChargeTime += Time.deltaTime;
                currentChargeTime = Mathf.Clamp(currentChargeTime, 0, weaponData.maxChargeTime);

                Vector3 inputDir = new Vector3(rightJoystick.Direction.x, 0, rightJoystick.Direction.y).normalized;
                if (inputDir != Vector3.zero)
                {
                    lastAimDirection = inputDir;
                }
            }
            else if (isCharging)
            {
                isCharging = false;
                animator.SetFloat("ThrowSpeed", 1f);
            }
        }
        public override bool Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }
 
            Vector3 targetPos = CalculateTargetPosition(weaponData);

            GameObject obj = projectilePool.Get();
            ThrowerProjectile projectile = obj.GetComponent<ThrowerProjectile>();
            if (projectile != null)
            {
                projectile.Init(projectilePool, firePointTransform.position, Quaternion.identity, weaponData.damage, weaponData.attackRadius);
            }

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;

                Vector3 velocity = CalculateParabolaVelocity(firePointTransform.position, targetPos, timeToTarget);

                rb.linearVelocity = velocity;

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

        public Vector3 CalculateTargetPosition(ThrowingWeaponData data)
        {
            if (currentChargeTime <= 0f)
            {
                Vector3 defaultPos = transform.position + (lastAimDirection * data.minRange);
                defaultPos.y = 0;
                return defaultPos;
            }
            float chargeRatio = currentChargeTime / data.maxChargeTime;

            float currentDistance = Mathf.Lerp(data.minRange, data.range, chargeRatio);

            Vector3 aimOffset = lastAimDirection * currentDistance;

            Vector3 baseTargetPos = firePointTransform.position + aimOffset;

            baseTargetPos.y = 0;

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
            float Vy = (distance.y / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);
 
            Vector3 result = distanceXZ.normalized * Vxz;
            result.y = Vy;

            return result;
        }

        public void SetIsCharging(bool isCharging)
        {
            this.isCharging = isCharging;
            currentChargeTime = 0f;
        }
        public bool GetIsCharging()
        {
            return isCharging;
        }
        public void ThrowStart()
        {
            lastAimDirection = new Vector3(rightJoystick.Direction.x, 0, rightJoystick.Direction.y).normalized;
        }
    }
}


