using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public enum FarmUIBtnType
    {
        Product,
        Upgrade,
    }

    public class FarmTaskUIView : MonoBehaviour, IFarmTaskUIView
    {
        [Header("탭")]
        [SerializeField] private Button productionTab;
        [SerializeField] private Button upgradeTab;
        [SerializeField] private Button backBtn;

        [Header("패널")]
        [SerializeField] private GameObject productionPanel;
        [SerializeField] private GameObject upgradePanel;

        [Header("실행 버튼")]
        [SerializeField] private Button btnProductionToggle;
        [SerializeField] private Button btnUpgrade;

        [SerializeField]
        Text buildingNameText;

        //[SerializeField]
        //TextMeshProUGUI productionTimeText;
        //[SerializeField]
        //TextMeshProUGUI powerText;

        // Event
        public event Action OnClickProduction;
        public event Action OnClickUpgrade;
        public event Action OnClickBack;
        public event Action<FarmUIBtnType> OnChangeTab;

        private void Awake()
        {
            btnProductionToggle.onClick.AddListener(() => OnClickProduction?.Invoke());
            btnUpgrade.onClick.AddListener(() => OnClickUpgrade?.Invoke());
            backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());

            productionTab?.onClick.AddListener(() => OnChangeTab?.Invoke(FarmUIBtnType.Product));
            upgradeTab?.onClick.AddListener(() => OnChangeTab?.Invoke(FarmUIBtnType.Upgrade));

            OnChangeTab += ChangeOptionBtn;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ChangeOptionBtn(FarmUIBtnType.Product);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ChangeOptionBtn(FarmUIBtnType type)
        {
            productionTab.image.color = new Color(1f, 1f, 1f, 0f);
            upgradeTab.image.color = new Color(1f, 1f, 1f, 0f);

            switch (type)
            {
                case FarmUIBtnType.Product:
                    productionTab.image.color = new Color(1f, 1f, 1f, 1f);
                    upgradePanel.SetActive(false);
                    productionPanel.SetActive(true);

                    break;
                case FarmUIBtnType.Upgrade:
                    upgradeTab.image.color = new Color(1f, 1f, 1f, 1f);
                    productionPanel.SetActive(false);
                    upgradePanel.SetActive(true);
                    break;
            }
        }

        // 늘어날 때마다 갱신
        public void UpdateUIStats(FarmUIData data)
        {
            buildingNameText.text = data.buildingName;
            //productionTimeText.SetText("{0}", data.productionTime);
            //powerText.SetText("{0}", data.power);
        }
    }


}
