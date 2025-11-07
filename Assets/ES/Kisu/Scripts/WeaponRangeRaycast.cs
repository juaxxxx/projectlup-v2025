using UnityEngine;

namespace ES
{
    public class WeaponRangeRaycast : MonoBehaviour
    {
        [Header("사거리 설정")]
        public float maxRange = 10f;
        public LayerMask obstacleMask;
        public Transform firePoint;
        //public GunData gunData; // 현재 총의 데이터
        private Gun gun;

        [Header("색상 설정")]
        public Color visibleColor = Color.green;
        public Color blockedColor = Color.red;
        public float lineWidth = 0.5f;

        [Header("총알 프리팹 (선택 사항)")]
        public GameObject bulletPrefab;

        private LineRenderer visibleLine;
        private LineRenderer blockedLine;

        Transform origin => firePoint != null ? firePoint : transform;

        void Start()
        {
            FixBulletRenderSettings();
            
            gun = GetComponent<Gun>();

            visibleLine = CreateLine("VisibleLine", visibleColor);
            blockedLine = CreateLine("BlockedLine", blockedColor);
        }

        void FixBulletRenderSettings()
        {
            if (bulletPrefab == null) return;

            Renderer[] renderers = bulletPrefab.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
            {
                if (r == null || r.sharedMaterial == null) continue;

                UnityEngine.Material mat = r.sharedMaterial;

                if (mat == null) continue;

                mat.renderQueue = 3100;
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

                r.sharedMaterial = mat;
            }
        }

        LineRenderer CreateLine(string name, Color color)
        {
            GameObject obj = new GameObject(name);
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;

            var lr = obj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startWidth = lr.endWidth = lineWidth;
            lr.useWorldSpace = true;
            lr.numCapVertices = 8;

            var shader = Shader.Find("Hidden/Internal-Colored");
            var mat = new UnityEngine.Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            mat.renderQueue = 3000;

            mat.color = new Color(color.r, color.g, color.b, 0.2f);
            lr.material = mat;
            lr.startColor = lr.endColor = color;

            return lr;
        }

        void Update()
        {
            if (gun != null)
            {
                maxRange = gun.weapon.range;
            }

            Vector3 start = origin.position;
            Vector3 dir = origin.forward;
            Vector3 end = start + dir * maxRange;

            visibleLine.SetPosition(0, start);
            visibleLine.SetPosition(1, end);

            if (Physics.Raycast(start, dir, out RaycastHit hit, maxRange, obstacleMask))
            {
                visibleLine.SetPosition(1, hit.point);

                blockedLine.enabled = true;
                blockedLine.SetPosition(0, hit.point);
                blockedLine.SetPosition(1, end);
            }
            else
            {
                blockedLine.enabled = false;
            }
        }
    }
}