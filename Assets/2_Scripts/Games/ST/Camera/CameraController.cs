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

        [Header("줌 설정 (FOV 기반)")]
        [SerializeField] private float defaultFov = 60f;
        [SerializeField] private float zoomedFov = 30f;
        [SerializeField] private float zoomLerpSpeed = 10f;

        [Header("조준경 방향 설정")]
        [SerializeField] private float zoomLookDistance = 30f;  // 마우스 방향으로 얼마나 앞을 볼지

        private bool zoomEnabled = false;
        private bool isZoomLocked = false;  
        private Vector3 zoomLockedPoint;

        // 현재 카메라가 향해야 할 목표 포인트 (overview or 슬롯 포인트)
        private Transform currentPoint;
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            if (cam != null)
            {
                cam.fieldOfView = defaultFov;
            }

            currentPoint = overviewPoint;
        }

        private void LateUpdate()
        {
            if (currentPoint == null) return;
            float dt = Time.deltaTime;

            // 1) 위치는 항상 currentPoint 기준으로 보간 이동 (기존 그대로)
            Vector3 targetPos = currentPoint.position;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                moveLerpSpeed * dt
            );

            // 2) 입력 처리: 줌 락/해제
            if (cam != null)
            {
                // zoomEnabled == true (원거리 선택 상태) 에서만 조준 허용
                if (zoomEnabled)
                {
                    // ★ 우클릭 "누르는 순간" → 그때의 방향/위치로 락
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        Vector3 origin = ray.origin;
                        Vector3 dir = ray.direction;

                        // 마우스 방향으로 일정 거리 앞을 "고정 조준점"으로 저장
                        zoomLockedPoint = origin + dir * zoomLookDistance;
                        isZoomLocked = true;
                    }

                    // 우클릭 "뗀 순간" → 락 해제
                    if (Input.GetMouseButtonUp(1))
                    {
                        isZoomLocked = false;
                    }
                }
                else
                {
                    // 줌 불가 상태면 락도 강제로 해제
                    isZoomLocked = false;
                }
            }

            // 3) 회전 처리
            if (cam != null && isZoomLocked)
            {
                // 조준 락 상태: 처음 우클릭 시 고정한 zoomLockedPoint 를 계속 바라봄
                Vector3 lookDir = zoomLockedPoint - transform.position;
                if (lookDir.sqrMagnitude > 0.0001f)
                {
                    Quaternion zoomRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        zoomRot,
                        rotLerpSpeed * dt
                    );
                }
            }
            else
            {
                // 조준 락이 아니면: 항상 currentPoint 의 회전 방향으로
                Quaternion targetRot = currentPoint.rotation;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotLerpSpeed * dt
                );
            }

            // 4) FOV 줌 처리
            if (cam != null)
            {
                float targetFov = (isZoomLocked ? zoomedFov : defaultFov);

                cam.fieldOfView = Mathf.Lerp(
                    cam.fieldOfView,
                    targetFov,
                    zoomLerpSpeed * dt
                );
            }
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

        public void SetZoomEnabled(bool enabled)
        {
            zoomEnabled = enabled;

            if (!zoomEnabled)
            {
                // 줌 끄면 락 해제 + FOV 복구
                isZoomLocked = false;
                if (cam != null)
                    cam.fieldOfView = defaultFov;
            }
        }
    }
}
