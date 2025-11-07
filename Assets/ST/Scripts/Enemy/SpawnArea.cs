using UnityEngine;
namespace ST
{

    public class SpawnArea : MonoBehaviour
    {
        [Header("영역 크기")]
        [SerializeField] private Vector2 areaSize = new Vector2(20f, 2f); // 가로x세로

        [Header("높이 설정")]
        [SerializeField] private float spawnHeight = 0f;

        [Header("디버그")]
        [SerializeField] private Color gizmoColor = Color.green;

        // 영역 내 랜덤 포지션
        public Vector3 GetRandomPosition()
        {
            float randomX = UnityEngine.Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomZ = UnityEngine.Random.Range(-areaSize.y / 2, areaSize.y / 2);

            Vector3 localPos = new Vector3(randomX, spawnHeight, randomZ);
            return transform.TransformPoint(localPos); // 월드 좌표로 변환
        }

        public Quaternion GetSpawnRotation()
        {
            return transform.rotation;
        }

        // Gizmo로 영역 표시
        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            // 바닥 사각형
            Vector3 center = transform.position + Vector3.up * spawnHeight;
            Vector3 size = new Vector3(areaSize.x, 0.1f, areaSize.y);

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.up * spawnHeight, size);

            // 모서리 표시
            Vector3 halfSize = new Vector3(areaSize.x / 2, 0, areaSize.y / 2);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(-halfSize.x, spawnHeight, -halfSize.z), 0.3f);
            Gizmos.DrawSphere(new Vector3(halfSize.x, spawnHeight, -halfSize.z), 0.3f);
            Gizmos.DrawSphere(new Vector3(-halfSize.x, spawnHeight, halfSize.z), 0.3f);
            Gizmos.DrawSphere(new Vector3(halfSize.x, spawnHeight, halfSize.z), 0.3f);
        }

        void OnDrawGizmosSelected()
        {
            // 선택 시 더 진하게
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.3f);

            Vector3 center = transform.position + Vector3.up * spawnHeight;
            Vector3 size = new Vector3(areaSize.x, 0.1f, areaSize.y);

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.up * spawnHeight, size);
        }
    }
}