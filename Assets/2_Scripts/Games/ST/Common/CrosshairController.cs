using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LUP.ST
{
    public class CrosshairController : MonoBehaviour
    {
        [Header("크로스헤어")]
        [SerializeField] private Image normalCrosshair;
        [SerializeField] private Image scopeImage;
        [SerializeField] private GameObject scopePanel;

        [Header("줌 UI")]
        [SerializeField] private Button zoomButton;
        [SerializeField] private Button zoomExitButton;

        [Header("숨길 UI들")]
        [SerializeField] private GameObject[] hideOnZoom;

        [Header("카메라")]
        [SerializeField] private CameraController cameraController;

        private RectTransform normalRect;
        private Canvas canvas;

        private bool isZooming = false;
        private bool isActive = false;

        public bool IsZooming => isZooming;

        void Awake()
        {
            if (normalCrosshair != null)
                normalRect = normalCrosshair.GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        void Start()
        {
            if (zoomButton != null)
                zoomButton.onClick.AddListener(StartZoom);
            if (zoomExitButton != null)
                zoomExitButton.onClick.AddListener(EndZoom);

            HideAll();
        }

        void Update()
        {
            if (!isActive) return;

            // 줌 아닐 때만 크로스헤어 이동
            if (!isZooming)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Input.touchCount > 0)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        return;
                    MoveCrosshairToScreenPosition(Input.GetTouch(0).position);
                }
                else if (Input.GetMouseButton(0))
                {
                    MoveCrosshairToScreenPosition(Input.mousePosition);
                }
            }
        }

        private void MoveCrosshairToScreenPosition(Vector2 screenPos)
        {
            if (normalRect == null || canvas == null) return;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.worldCamera,
                out localPoint
            );

            normalRect.anchoredPosition = localPoint;
        }

        public void Show(STCharacterData characterData)
        {
            if (characterData == null) return;

            isActive = true;

            if (characterData.normalCrosshair != null)
                normalCrosshair.sprite = characterData.normalCrosshair;

            if (characterData.scopeCrosshair != null)
                scopeImage.sprite = characterData.scopeCrosshair;

            normalCrosshair.gameObject.SetActive(true);
            if (scopePanel != null) scopePanel.SetActive(false);
            if (zoomButton != null) zoomButton.gameObject.SetActive(true);
            if (zoomExitButton != null) zoomExitButton.gameObject.SetActive(false);
        }

        public void HideAll()
        {
            isActive = false;
            isZooming = false;

            if (normalCrosshair != null) normalCrosshair.gameObject.SetActive(false);
            if (scopePanel != null) scopePanel.SetActive(false);
            if (zoomButton != null) zoomButton.gameObject.SetActive(false);
            if (zoomExitButton != null) zoomExitButton.gameObject.SetActive(false);

            foreach (var ui in hideOnZoom)
            {
                if (ui != null) ui.SetActive(true);
            }

            cameraController?.EndZoom();
        }

        public void StartZoom()
        {
            if (!isActive) return;

            isZooming = true;
            if (normalCrosshair != null) normalCrosshair.gameObject.SetActive(false);
            if (scopePanel != null) scopePanel.SetActive(true);
            if (zoomButton != null) zoomButton.gameObject.SetActive(false);
            if (zoomExitButton != null) zoomExitButton.gameObject.SetActive(true);

            foreach (var ui in hideOnZoom)
            {
                if (ui != null) ui.SetActive(false);
            }

            cameraController?.StartZoom();
        }

        public void EndZoom()
        {
            isZooming = false;
            if (normalCrosshair != null) normalCrosshair.gameObject.SetActive(true);
            if (scopePanel != null) scopePanel.SetActive(false);
            if (zoomButton != null) zoomButton.gameObject.SetActive(true);
            if (zoomExitButton != null) zoomExitButton.gameObject.SetActive(false);

            foreach (var ui in hideOnZoom)
            {
                if (ui != null) ui.SetActive(true);
            }

            cameraController?.EndZoom();
        }
    }
}