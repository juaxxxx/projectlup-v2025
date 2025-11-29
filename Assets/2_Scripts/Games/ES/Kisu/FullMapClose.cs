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
            fullmapPanel.SetActive(false); // ÀüÃ¼ Áöµµ ¼û±è
            minimapPanel.SetActive(true);  // ¹Ì´Ï¸Ê ´Ù½Ã Ç¥½Ã
        }
    }
}