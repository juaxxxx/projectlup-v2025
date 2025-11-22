using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class sellingContent : MonoBehaviour, IPanelContentAble
    {
        [HideInInspector]
        public Image sellImage;

        [HideInInspector]
        public List<Button> btnList;

        [SerializeField]
        private TextImageBtn buttonPrefab;

        private GameObject ItemLayoutContent;
        public GameObject SellingItemBtnBuyPanel;


        public bool Init()
        {
            ItemLayoutContent = gameObject.transform.parent.gameObject;

            if (ItemLayoutContent == null)
            {
                return false;
            }

            SellingItemBtnBuyPanel = gameObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;

            if (SellingItemBtnBuyPanel == null)
            {
                return false;
            }

            return true;
        }

        public void SetContent(Vector2 size, int buttonNum)
        {
            foreach (var btn in btnList)
            {
                if (btn != null)
                    Destroy(btn.gameObject);
            }
            btnList.Clear();


            for (int i = 0; i < buttonNum; i++)
            {
                int index = i;
                TextImageBtn newBtn = Instantiate(buttonPrefab, SellingItemBtnBuyPanel.transform);

                if (newBtn.Init())
                {
                    btnList.Add(newBtn.button);

                    newBtn.button.onClick.AddListener(() => OnBtnClicked(index));
                }

                else
                {
                    UnityEngine.Debug.LogError("Fail To Init Buy Btn");
                }


            }
        }

        public void OnBtnClicked(int btnIndex)
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

