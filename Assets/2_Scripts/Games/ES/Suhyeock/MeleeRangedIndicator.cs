using UnityEngine;

namespace LUP.ES
{
    public class MeleeRangedIndicator : MonoBehaviour
    {
        private MeleeWeapon meleeWeapon;
        private Transform playerTransform;
        public float heightOffset = 0.1f;
        public int segments = 50;        // µÕ±Ù Á¤µµ
        public Color meshColor = new Color(0, 0.5f, 0, 0.5f);

        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            meleeWeapon = GetComponentInParent<MeleeWeapon>();
            GameObject player = GameObject.FindWithTag("Player");
            playerTransform = player.transform;
            meshRenderer = GetComponent<MeshRenderer>();

            mesh = new Mesh();
            meshFilter.mesh = mesh;

            Material mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mat.color =  meshColor;
            meshRenderer.material = mat;
            //DrawViewField();
        }

        private void LateUpdate()
        {
            DrawMesh();
        }

        public void DrawMesh()
        {
            int vertexCount = segments + 2;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[segments * 3];

            vertices[0] = Vector3.zero;

            MeleeWeaponItemData data = meleeWeapon.weaponItem.data as MeleeWeaponItemData;

            float startAngle = -data.attackAngle / 2f;
            float angleStep = data.attackAngle / segments;

            for (int i = 0; i <= segments; i++)
            {
                float currentAngleRed = Mathf.Deg2Rad * (startAngle + (angleStep * i));

                float x = Mathf.Sin(currentAngleRed) * data.range;
                float z = Mathf.Cos(currentAngleRed) * data.range;

                vertices[i + 1] = new Vector3(x, 0.05f, z);

                if (i < segments)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();


        }
    }
}

