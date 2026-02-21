using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

namespace LUP.ES
{
    public class MeleeWeaponRange : MonoBehaviour
    {
        [Header("사거리 설정")]
        public float range = 2f;
        [Range(0, 360)]
        public float angle = 30f;
        public int segments = 20;

        [Header("색상 설정")]
        public Color rangeColor = Color.yellow;
        public float lineWidth = 0.05f;

        private MeleeWeapon axe;
        private LineRenderer rangeLine;
        private Transform playerTransform;
        private bool isVisible = true;

        void Start()
        {
            axe = GetComponent<MeleeWeapon>();

            var player = GetComponentInParent<CharacterController>();

            if (player != null)
            {
                playerTransform = player.transform;
            }

            rangeLine = CreateArcLine("MeleeRangeLine", rangeColor);
        }

        LineRenderer CreateArcLine(string name, Color color)
        {
            GameObject obj = new GameObject(name);
            obj.transform.parent = playerTransform;
            obj.transform.localPosition = Vector3.zero;

            var lr = obj.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.startWidth = lr.endWidth = lineWidth;
            lr.positionCount = segments + 2;

            var shader = Shader.Find("Hidden/Internal-Colored");
            var mat = new UnityEngine.Material(shader);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            mat.color = new Color(color.r, color.g, color.b, 0.4f);
            lr.material = mat;
            lr.startColor = lr.endColor = color;

            return lr;
        }
        void Update()
        {
            if (!isVisible || rangeLine == null || playerTransform == null) return;

            if (axe != null && axe.weaponItem != null)
            {
                range = axe.weaponItem.data.range;
                MeleeWeaponItemData  meleeWeaponItemData = axe.weaponItem.data as MeleeWeaponItemData;
                angle = meleeWeaponItemData.attackAngle;
            }

            DrawArc();
        }

        void DrawArc()
        {
            int pointCount = segments + 3;
            rangeLine.positionCount = pointCount;

            Vector3 origin = playerTransform.position;

            origin.y -= 1.0f;
            origin.y += 0.05f; 

            Vector3 forwardDir = playerTransform.forward;

            rangeLine.SetPosition(0, origin);

            float startAngle = -angle / 2f;

            for (int i = 0; i <= segments; i++)
            {
                float currentAngle = startAngle + (angle / segments) * i;

                Vector3 dir = Quaternion.Euler(0, currentAngle, 0) * forwardDir;
                Vector3 point = origin + dir * range;

                rangeLine.SetPosition(i + 1, point);
            }

            rangeLine.SetPosition(pointCount - 1, origin);
        }

        public void SetIsVisible(bool visible)
        {
            isVisible = visible;
            if (rangeLine != null) rangeLine.enabled = visible;
        }
    }
}