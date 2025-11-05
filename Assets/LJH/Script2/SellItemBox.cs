using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Roguelike.Util;

namespace RL
{
    public class SellItemBox : MonoBehaviour, IPanelContentAble
    {
        public Image ItemBannerImage;
        private GameObject ItemLayoutContent;

        //[SerializeField]
        //public sellingContentData[] sellingContetsData;

        //[SerializeField]
        //public sellingContent sellingContetnPrefab;

        [SerializeField]
        public ShopbuyContent[] sellingContentPrefab;

        public float spacingRate = 0.05f;

        private Vector2 parentViewportSize;

        void Start()
        {
            Init();

            StartCoroutine(RoguelikeUtil.DelayOneFrame(() =>
            {
                showPanel();
            }
            ));
        }

        public bool Init()
        {

            ItemBannerImage = gameObject.GetComponentInChildren<Image>();

            if (ItemBannerImage == null)
                return false;

            ItemLayoutContent = gameObject.GetComponentInChildren<tempConfigureScript>().gameObject;

            if (ItemLayoutContent == null)
                return false;

            return true;
        }

        public void showPanel()
        {
            parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
            Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, parentViewportSize.y * 0.3f);
            gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;

            int count = sellingContentPrefab.Length;
            float spacing = spacingRate / (count + 1);
            float itemWidth = (1f - spacingRate) / count;


            for (int i = 0; i < sellingContentPrefab.Length; i++)
            {
                ShopbuyContent sellingContent = Instantiate(sellingContentPrefab[i], ItemLayoutContent.transform, false);

                if (sellingContent.Init())
                {
                    float xMin = spacing + i * (itemWidth + spacing);
                    float xMax = xMin + itemWidth;

                    Vector2 minimalAnchor = new Vector2(xMin, 0);
                    Vector2 maxiaumAnchor = new Vector2(xMax, 1);

                    sellingContent.SetRatio(minimalAnchor, maxiaumAnchor);
                }

                else
                {
                    UnityEngine.Debug.LogError("Fail to Create Selling Contetn");
                }

            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created


        // Update is called once per frame
        void Update()
        {

        }
    }
}

