using UnityEngine;

namespace LUP.PCR
{
    public class Tile : MonoBehaviour
    {
        public TileInfo tileInfo;

        [SerializeField]
        private GameObject tileVisualObject;
        [SerializeField]
        private GameObject canActMark;
        [SerializeField]
        private GameObject canNotActMark;
        [SerializeField]
        private GameObject darkVisionMark;

        private void Start()
        {
            HideCanDigWallMark();
            HideCanNotDigWallMark();
        }

        public void SetTileInfo(TileInfo tileInfo)
        {
            this.tileInfo = tileInfo;
        }
        public void SetTileVisualActive(bool isActive)
        {
            if (tileVisualObject != null)
            {
                tileVisualObject.SetActive(isActive);
            }
        }
        public void ShowCanDigWallMark()
        {
            if (canActMark)
            {
                canActMark.SetActive(true);
            }
        }
        public void HideCanDigWallMark()
        {
            if (canActMark)
            {
                canActMark.SetActive(false);
            }
        }
        public void ShowCanNotDigWallMark()
        {
            if (canNotActMark)
            {
                canNotActMark.SetActive(true);
            }
        }
        public void HideCanNotDigWallMark()
        {
            if (canNotActMark)
            {
                canNotActMark.SetActive(false);
            }
        }

        public void ShowDarkVisionMark()
        {
            if (darkVisionMark)
            {
                darkVisionMark.SetActive(true);
            }
        }
        public void HideDarkVisionMark()
        {
            if (darkVisionMark)
            {
                darkVisionMark.SetActive(false);
            }
        }
    }


}
