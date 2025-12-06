using UnityEngine;

namespace LUP.ES
{
    public class MeleeRangedIndicator : MonoBehaviour
    {
        private MeleeWeapon meleeWeapon;
        private Transform playerTransform;
        private LineRenderer lineRenderer;
        public float heightOffset = 0.1f;
        public int segments = 50;        // 곡선의 부드러움 정도

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            meleeWeapon = GetComponent<MeleeWeapon>();
            GameObject player = GameObject.FindWithTag("Player");
            playerTransform = player.transform;
            lineRenderer = GetComponent<LineRenderer>(); 
            lineRenderer.useWorldSpace = true;
            lineRenderer.alignment = LineAlignment.TransformZ;
            lineRenderer.loop = false;
            //DrawViewField();
        }

        private void LateUpdate()
        {
            DrawViewField();
        }

        public void DrawViewField()
        {
            int pointCount = segments + 1;
            Vector3[] points = new Vector3[pointCount + 1];

            MeleeWeaponItemData data = meleeWeapon.weaponItem.data as MeleeWeaponItemData;
            Vector3 centerPos = playerTransform.position;
            Quaternion currentRot = playerTransform.rotation;

            float startAngle = -data.attackAngle / 2f;
            float angleStep = data.attackAngle / segments;

            for (int i = 0; i < pointCount; i++)
            {
                float currentAngelRad = Mathf.Deg2Rad * (startAngle + (angleStep * i));
                float x = Mathf.Sin(currentAngelRad) * data.range;
                float z = Mathf.Cos(currentAngelRad) * data.range;

                Vector3 localOffset = new Vector3(x, 0, z);
                Vector3 WorldPos = centerPos + (currentRot * localOffset);

                WorldPos.y = centerPos.y + heightOffset;
                points[i] = WorldPos;
            }

            points[pointCount] = centerPos;
            points[pointCount].y += heightOffset;

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    }
}

