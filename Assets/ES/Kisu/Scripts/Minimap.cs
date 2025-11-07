using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

namespace ES
{
    public class Minimap : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Camera minimapCamera;

        [SerializeField]
        private float zoomMin = 5; // 카메라 최소 크기
        [SerializeField]
        private float zoomMax = 20; // 카메라 최대 크기
        [SerializeField]
        private float zoomOneStep = 5; // 1회 줌 할때 증가/ 감소되는 수치


        [SerializeField]
        private GameObject fullmapPanel;
        [SerializeField]
        private GameObject minimapPanel;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ZoomIn()
        {
            minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
        }

        public void ZoomOut()
        {
            minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            fullmapPanel.SetActive(true);   // 전체 지도 표시
            minimapPanel.SetActive(false);  // 미니맵 숨기기
        }
    }
}