using UnityEngine;

namespace ES
{
    public class CopyPosition : MonoBehaviour
    {
        [SerializeField] private Transform target; // 플레이어
        [SerializeField] private bool followX = true, followY = false, followZ = true;

        [SerializeField] private Vector2 minBounds; // 맵 최소 X,Z
        [SerializeField] private Vector2 maxBounds; // 맵 최대 X,Z

        [SerializeField] private Camera minimapCamera; // 미니맵 카메라

        void Update()
        {
            if (!target) return;

            Vector3 pos = transform.position;

            // X, Z축은 플레이어 따라가되 경계 안으로 제한
            if (followX) pos.x = Mathf.Clamp(target.position.x, minBounds.x, maxBounds.x);
            if (followY) pos.y = target.position.y; // 필요 시 고정 가능
            if (followZ) pos.z = Mathf.Clamp(target.position.z, minBounds.y, maxBounds.y);

            float halfHeight = minimapCamera.orthographicSize;
            float halfWidth = halfHeight * minimapCamera.aspect;

            pos.x = Mathf.Clamp(pos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            pos.z = Mathf.Clamp(pos.z, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            transform.position = pos;
        }
    }
}