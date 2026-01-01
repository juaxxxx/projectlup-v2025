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
        [SerializeField] Text productionToggleText;

        [Header("건물정보 텍스트")]
        [SerializeField] Text buildingNameText;

        [Header("업그레이드 패널 UI")]
        [SerializeField] private Text levelChangeText;   // "Lv.5 >> Lv.6"

        [Header("업그레이드 효과")]
        [SerializeField] private Text effectNameText;    // "저장 용량 증가"
        [SerializeField] private Text effectValueText;   // "35 + 5"

        [Header("필요 자원")]
        [SerializeField] private GameObject costSlot1;
        [SerializeField] private Image costIcon1;
        [SerializeField] private Text costText1;

        [SerializeField] private GameObject costSlot2;
        [SerializeField] private Image costIcon2;
        [SerializeField] private Text costText2;

        // Event
        public event Action OnClickWorkRequest;
        public event Action OnClickUpgrade;
        public event Action OnClickBack;
        public event Action<FarmUIBtnType> OnChangeTab;

        private void Awake()
        {
            btnProductionToggle.onClick.AddListener(() => OnClickWorkRequest?.Invoke());
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

           if (data.isWorkRequested)
           {
               productionToggleText.text = "요청 취소";
               btnProductionToggle.image.color = Color.gray;
           }
           else
           {
               productionToggleText.text = "작업 요청";
               btnProductionToggle.image.color = Color.white;
           }

            if (data.isConstructing)
            {
                upgradeTab.interactable = false; // 버튼 클릭 불가
                // 현재 업그레이드 패널이 열려있다면 강제로 생산 패널로 전환
                if (upgradePanel.activeSelf) ChangeOptionBtn(FarmUIBtnType.Product);
            }
            else
            {
                upgradeTab.interactable = true;
            }

            UpdateUpgradePanel(data);
        }
        private void UpdateUpgradePanel(FarmUIData data)
        {
            // 만렙 처리
            if (data.isMaxLevel)
            {
                levelChangeText.text = "Max Level";
                effectNameText.text = "";
                effectValueText.text = "";
                costSlot1.SetActive(false);
                costSlot2.SetActive(false);
                btnUpgrade.interactable = false;
                return;
            }

            // 레벨 텍스트
            levelChangeText.text = $"Lv.{data.level} >> Lv.{data.level + 1}";

            // 효과 텍스트
            effectNameText.text = data.effectName;
            effectValueText.text = $"{data.currentStatValue} <color=green>+{data.nextStatAddedValue}</color>";

            // 비용 슬롯 1
            if (data.costAmount1 > 0)
            {
                costSlot1.SetActive(true);
                costText1.text = data.costAmount1.ToString();
                // costIcon1.sprite = ResourceManager.GetIcon(data.costType1); // 아이콘 연결 필요
            }
            else costSlot1.SetActive(false);

            // 비용 슬롯 2
            if (data.costAmount2 > 0)
            {
                costSlot2.SetActive(true);
                costText2.text = data.costAmount2.ToString();
                // costIcon2.sprite = ResourceManager.GetIcon(data.costType2); // 아이콘 연결 필요
            }
            else costSlot2.SetActive(false);

            // 버튼 활성화 (공사중이 아니면 가능)
            btnUpgrade.interactable = !data.isConstructing;
        }
    }


}
