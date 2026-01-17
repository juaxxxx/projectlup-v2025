using UnityEngine;
using UnityEngine.EventSystems;

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

        [Header("줌 드래그 설정")]
        [SerializeField] private float dragSensitivity = 0.2f;

        [Header("조준 설정")]
        [SerializeField] private float aimSensitivity = 1f; // 조준 감도
        [SerializeField] private float smoothSpeed = 15f;   // 카메라 회전 부드러움 정도
        [SerializeField] private Vector2 verticalRotationLimit = new Vector2(-20f, 30f); // 상하 각도 제한
        [SerializeField] private Vector2 horizontalRotationLimit = new Vector2(-45f, 45f); // 좌우 각도 제한

        private float yaw;   // 좌우 회전 누적값
        private float pitch; // 상하 회전 누적값

        private Quaternion targetRotation;

        private bool zoomEnabled = false;
        private bool isZoomMode = false;  // 외부에서 StartZoom/EndZoom으로 제어

        private Transform currentPoint;
        private Camera cam;

        private void Start()
        {
            Vector3 currentRotation = transform.eulerAngles;
            yaw = currentRotation.y;
            pitch = currentRotation.x;
        }

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
            transform.position = Vector3.Lerp(transform.position, currentPoint.position, dt * moveLerpSpeed);

            // --- [회전 및 줌 처리] ---
            if (isZoomMode)
            {
                // 줌 모드일 때는 드래그로 계산된 자유 조준 회전 사용
                HandleZoomAiming();

                // FOV 줌 인 효과
                if (cam != null)
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFov, dt * zoomLerpSpeed);
            }
            else
            {
                // 일반 모드일 때는 캐릭터가 바라보는 방향(Rotation)을 부드럽게 따라감
                transform.rotation = Quaternion.Slerp(transform.rotation, currentPoint.rotation, dt * rotLerpSpeed);

                // FOV 줌 아웃 효과
                if (cam != null)
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, dt * zoomLerpSpeed);

                // 일반 모드로 돌아왔을 때 누적된 조준값 초기화 (다시 줌 켰을 때 튀지 않게)
                Vector3 currentRot = transform.eulerAngles;
                yaw = currentRot.y;
                pitch = currentRot.x;
            }
        }
        private void HandleZoomAiming()
        {
            // 화면을 누르고 있을 때만 회전 계산
            if (Input.GetMouseButton(0))
            {
                // 1. 마우스/터치 이동량(Delta) 가져오기
                // Input.GetAxis는 마우스의 움직임 변화량을 직접 가져오므로 더 직관적입니다.
                float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * (cam.fieldOfView / defaultFov);
                float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * (cam.fieldOfView / defaultFov);

                // 2. 누적값 계산
                yaw += mouseX * 10f;  // 감도 조절을 위해 10 배수 사용
                pitch -= mouseY * 10f;

                // 3. 회전 제한 (Clamp) - 이 부분이 화면이 확 돌아가는 걸 막아줍니다.
                // 기준점(currentPoint)의 초기 회전값 기준으로 제한하고 싶다면 아래처럼 계산합니다.
                pitch = Mathf.Clamp(pitch, verticalRotationLimit.x, verticalRotationLimit.y);
                yaw = Mathf.Clamp(yaw, horizontalRotationLimit.x, horizontalRotationLimit.y);

                // 4. 목표 회전 생성
                Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);

                // 5. 부드럽게 회전 적용 (Slerp)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothSpeed);
            }
        }

        private void HandleZoomDrag()
        {
            // 모바일 터치
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // UI 위면 무시
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (touch.phase == TouchPhase.Began)
                {
                    // 클릭한 지점으로 카메라 회전
                    RotateCameraToScreenPoint(touch.position);
                }
            }
            // 에디터 마우스 테스트
            else if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                RotateCameraToScreenPoint(Input.mousePosition);
            }
        }
        private void RotateCameraToScreenPoint(Vector2 screenPos)
        {
            if (cam == null) return;

            // 1. 화면 중앙과 클릭 지점의 비율 계산 (-0.5 ~ 0.5 범위)
            float viewportX = (screenPos.x / Screen.width) - 0.5f;
            float viewportY = (screenPos.y / Screen.height) - 0.5f;

            // 2. 현재 카메라의 FOV를 고려하여 회전해야 할 실제 각도 계산
            // 줌 상태일 때(zoomedFov) 더 예민하게 반응하도록 실제 시야각(cam.fieldOfView) 사용
            float horizontalFov = 2f * Mathf.Atan(Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * cam.aspect) * Mathf.Rad2Deg;
            float verticalFov = cam.fieldOfView;

            float angleY = viewportX * horizontalFov;
            float angleX = -viewportY * verticalFov;

            // 3. 부드럽게 이동하고 싶다면 목표 회전값을 설정하고 Slerp로 돌리는 것이 좋지만,
            // 즉각적인 반응을 원하신다면 아래처럼 직접 회전값을 더해줍니다.
            transform.Rotate(Vector3.up, angleY, Space.World);
            transform.Rotate(Vector3.right, angleX, Space.Self);
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
                isZoomMode = false;
                if (cam != null)
                    cam.fieldOfView = defaultFov;
            }
        }

        // CrosshairManager에서 호출
        public void StartZoom()
        {
            if (!zoomEnabled) return;
            isZoomMode = true;
        }

        public void EndZoom()
        {
            isZoomMode = false;
        }

        public bool IsZoomMode => isZoomMode;
    }
}