using UnityEngine;

namespace LUP.ST
{
    public class CameraController : MonoBehaviour
    {
        [Header("전체 보기(Overview) 기준점")]
        [SerializeField] private Transform overviewPoint;

        [Header("보간 설정")]
        [SerializeField] private float moveLerpSpeed = 5f;
        [SerializeField] private float rotLerpSpeed = 10f;

        // 현재 카메라가 향해야 할 목표 포인트 (overview or 슬롯 포인트)
        private Transform currentPoint;

        private void Awake()
        {
            currentPoint = overviewPoint;
        }

        private void LateUpdate()
        {
            if (currentPoint == null) return;

            Vector3 targetPos = currentPoint.position;
            Quaternion targetRot = currentPoint.rotation;

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                moveLerpSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotLerpSpeed * Time.deltaTime
            );
        }

        public void FocusOnPoint(Transform focusPoint)
        {
            if (focusPoint == null)
            {
                SetOverviewMode();
                return;
            }

            currentPoint = focusPoint;
        }

        public void SetOverviewMode()
        {
            currentPoint = overviewPoint;
        }
    }
}
