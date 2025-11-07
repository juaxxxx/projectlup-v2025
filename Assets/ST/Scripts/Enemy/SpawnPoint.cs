using UnityEngine;
namespace ST
{

    public class SpawnPoint : MonoBehaviour
    {
        [Header("디버그")]
        public Color gizmoColor = Color.red;
        public float gizmoRadius = 0.5f;

        public Vector3 GetSpawnPosition()
        {
            return transform.position;
        }

        public Quaternion GetSpawnRotation()
        {
            return transform.rotation;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, gizmoRadius);

            // 방향 표시
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * 2f);
        }
    }
}