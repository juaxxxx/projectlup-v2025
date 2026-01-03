using UnityEngine;

namespace LUP.ES
{
    public class ThrowTrajectoryVisualizer : MonoBehaviour
    {
        private ThrowingWeapon weapon;
        [SerializeField]
        private Transform impactIndicator;

        [SerializeField]
        private int resolution = 40;

        private LineRenderer lineRenderer;

        public void SetWeapon(ThrowingWeapon newWeapon)
        {
            weapon = newWeapon;
            HideTrajectory();
        }
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = resolution;
            lineRenderer.enabled = false;

            if (impactIndicator != null)
                impactIndicator.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (weapon == null)
                return;
            if (weapon.GetIsCharging())
            {
                ShowTrajectory();
            }
            else
            {
                HideTrajectory();
            }
        }

        private void ShowTrajectory()
        {
            lineRenderer.enabled = true;
            impactIndicator.gameObject.SetActive(true);
            ThrowingWeaponData data = weapon.weaponItem.data as ThrowingWeaponData;

            //Vector3 aimDir = weapon.playerTransform.forward;
            //if (weapon.rightJoystick.Direction != Vector2.zero)
            //{
            //    aimDir = new Vector3(weapon.rightJoystick.Direction.x, 0, weapon.rightJoystick.Direction.y);
            //}

            //float chargeRatio = weapon.currentChargeTime / data.maxChargeTime;
            //chargeRatio = Mathf.Clamp01(chargeRatio);

            //float currentDistance = Mathf.Lerp(data.minRange, data.range, chargeRatio);
            //Vector3 targetPos = weapon.playerTransform.position + (aimDir * currentDistance);
            Vector3 targetPos = weapon.CalculateTargetPosition(data);
            targetPos.y = 0f;

            DrawParabola(transform.position, targetPos, weapon.timeToTarget);

            if (impactIndicator != null)
            {
                impactIndicator.position = targetPos + Vector3.up * 0.05f;

                float diameter = data.attackRadius * 2f;
                impactIndicator.localScale = new Vector3(diameter, diameter, 1f);
            }
        }

        private void HideTrajectory()
        {
            lineRenderer.enabled = false;
            if (impactIndicator != null)
                impactIndicator.gameObject.SetActive(false);
        }

        private void DrawParabola(Vector3 start, Vector3 end, float time)
        {
            Vector3 distance = end - start;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            float sXZ = distanceXZ.magnitude;
            float Vxz = sXZ / time;
            float Vy = (distance.y / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

            Vector3 velocity = distanceXZ.normalized * Vxz;
            velocity.y = Vy;

            for (int i = 0; i < resolution; i++)
            {
                float simulationTime = i / (float)(resolution - 1) * time;
                Vector3 displacement = velocity * simulationTime;
                displacement.y -= 0.5f * Mathf.Abs(Physics.gravity.y) * simulationTime * simulationTime;

                Vector3 drawPoint = start + displacement;
                lineRenderer.SetPosition(i, drawPoint);
            }
        }
    }

}

