using DG.Tweening;
using UnityEngine;

namespace LUP.ES
{
    public class FullMapClose : MonoBehaviour
    {
        [SerializeField] private GameObject minimapPanel;
        [SerializeField] private GameObject fullmapPanel;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnCloseFullMap()
        {
            fullmapPanel.transform.DOKill();
            fullmapPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    fullmapPanel.SetActive(false); // 전체 지도 숨김
                    minimapPanel.SetActive(true);  // 미니맵 다시 표시
                });
           
        }
    }
}