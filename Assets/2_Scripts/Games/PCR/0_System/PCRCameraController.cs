using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace LUP.PCR
{

    using UnityEngine;

    public class PCRCameraController : MonoBehaviour
    {
        private Camera cam;

        [Range(1, 10)]
        [Header("줌 인/줌 아웃")]
        [SerializeField] private float minZoomDistance = 10f; // 가장 가까이 확대할 수 있는 거리 (최소 거리)
        [SerializeField] private float maxZoomDistance; // 맵 크기에 맞춰 자동으로 계산될 최대 거리
        [SerializeField] private float zoomSpeed = 5f;        // 줌 속도
        private float currentZoomDist;

        [Header("드래그")]
        public float dragSpeed = 1.0f;
        private Vector3 dragOrigin;
        private bool isDragging = false;

        private float mapWidth;
        private float mapHeight;
        private float mapZPos;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Start()
        {
            mapWidth = GridSize.x * GridSize.tileSize;
            mapHeight = GridSize.y * GridSize.tileSize;
            mapZPos = GridSize.mapZPos;

            CalculateMaxZoomDistance();

            // 맵과 카메라 사이의 거리
            currentZoomDist = Mathf.Abs(transform.position.z - mapZPos);

            ZoomOutMax();
        }

        private void ZoomOutMax()
        {
            currentZoomDist = maxZoomDistance;

            float centerX = mapWidth * 0.5f;
            float centerY = -mapHeight * 0.5f; // Y축이 아래로 생성되므로 음수
            float centerZ = mapZPos - currentZoomDist;

            transform.position = new Vector3(centerX, centerY, centerZ);
        }
        private void Update()
        {
            HandleInput();
        }

        private void LateUpdate()
        {
            // 이동 및 줌이 끝난 후 최종적으로 범위를 벗어나지 않게 고정
            ClampCameraPosition();
        }

        // 맵의 가로/세로 크기에 딱 맞는 카메라 거리를 계산
        private void CalculateMaxZoomDistance()
        {
            // 세로 기준으로 꽉 차는 거리
            float distHeight = (mapHeight * 0.5f) / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            // 가로 기준으로 꽉 차는 거리
            float distWidth = (mapWidth * 0.5f) / (Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * cam.aspect);

            // 둘 중 더 가까운 거리를 Max로 잡아야 맵 밖이 안 보임
            maxZoomDistance = Mathf.Min(distHeight, distWidth);
        }

        private void HandleInput()
        {
            float scrollDelta = 0f;

            // 모바일 핀치 줌 (두 손가락)
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // 벌리면(-), 오므리면(+) : 로직에 따라 부호 조정
                // 여기서는 벌리면(거리가 커지면) 줌인(거리 감소) 되도록 설정
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // 모바일은 감도를 좀 낮춤
                scrollDelta = deltaMagnitudeDiff * 0.01f * zoomSpeed;
            }
            // PC 마우스 휠 줌
            else
            {
                // 휠을 올리면(+) 줌인(거리 감소) -> 따라서 -를 붙여줌
                scrollDelta = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 5f;
            }

            // 줌 적용 (거리 값 변경)
            if (Mathf.Abs(scrollDelta) > 0.001f)
            {
                currentZoomDist += scrollDelta;
                // 줌 거리 제한 (최소 ~ 자동 계산된 최대값)
                currentZoomDist = Mathf.Clamp(currentZoomDist, minZoomDistance, maxZoomDistance);

                // 카메라 Z 위치 업데이트 (카메라가 -Z 쪽에 있다고 가정)
                Vector3 pos = transform.position;
                pos.z = mapZPos - currentZoomDist;
                transform.position = pos;
            }

            // Drag (Pan) 처리
            // 줌 도중에는 드래그 막기 (터치 2개일 때 튀는 현상 방지)
            if (Input.touchCount >= 2)
            {
                isDragging = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                dragOrigin = GetWorldPositionOnScreen(Input.mousePosition);
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 currentPos = GetWorldPositionOnScreen(Input.mousePosition);
                Vector3 difference = dragOrigin - currentPos;

                // 카메라 이동
                transform.position += new Vector3(difference.x, difference.y, 0);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        // 마우스 포인터가 맵 평면(Z depth)상의 어디를 찍었는지 계산
        private Vector3 GetWorldPositionOnScreen(Vector3 screenPos)
        {
            // 현재 카메라의 깊이(currentZoomDist)를 기준으로 변환해야 드래그가 정확함
            screenPos.z = currentZoomDist;
            return cam.ScreenToWorldPoint(screenPos);
        }

        // 카메라가 맵 밖으로 나가지 않도록 최종 위치 보정
        private void ClampCameraPosition()
        {
            // 현재 줌 거리에서 보이는 화면 반경 계산
            float halfFrustumHeight = currentZoomDist * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfFrustumWidth = halfFrustumHeight * cam.aspect;

            // 맵 경계 좌표 정의
            // Left: 0, Right: Width
            // Top: 0, Bottom: -Height
            float mapLeft = 0f;
            float mapRight = mapWidth;
            float mapBottom = -mapHeight;
            float mapTop = 0f;

            // 이동 가능한 좌표의 Min/Max 계산
            float minX = mapLeft + halfFrustumWidth;
            float maxX = mapRight - halfFrustumWidth;

            // Top(0)에서 반경만큼 내려오고(-), Bottom(-Height)에서 반경만큼 올라감(+)
            float minY = mapBottom + halfFrustumHeight;
            float maxY = mapTop - halfFrustumHeight;

            Vector3 newPos = transform.position;

            // X축 Clamp (만약 줌아웃을 너무 해서 화면이 맵보다 크면 중앙 고정)
            if (maxX < minX) newPos.x = mapWidth * 0.5f;
            else newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

            // Y축 Clamp
            if (maxY < minY) newPos.y = -mapHeight * 0.5f;
            else newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            // Z축 Clamp (줌) - 한번 더 확실하게 제한
            // (이미 HandleInput에서 했지만, 외부 요인 방지)
            // newPos.z는 mapZPos - currentZoomDist 로 설정됨

            transform.position = newPos;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float w = (mapWidth > 0) ? mapWidth : GridSize.x * GridSize.tileSize;
            float h = (mapHeight > 0) ? mapHeight : GridSize.y * GridSize.tileSize;
            float z = (mapZPos != 0) ? mapZPos : GridSize.mapZPos;

            Vector3 center = new Vector3(w * 0.5f, -h * 0.5f, z);
            Vector3 size = new Vector3(w, h, 1f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);
        }
#endif
    }
}