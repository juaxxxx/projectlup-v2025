using Roguelike.Define;
using System;
using System.Text;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

namespace LUP.RL
{
    public class RLTierColor
    {
        public static readonly Color Common = Color.gray;
        public static readonly Color Rare = new Color(0.3f, 0.6f, 1f);
        public static readonly Color Epic = new Color(0.8f, 0.3f, 1f);
    }


    public class EquipInfoPopupPanel : MonoBehaviour
    {
        public Action<EquipData> OnEquipSelected;
        public Action<EquipData> OnEquipReleased;

        private EquipData DisplayedEquipData;
        private bool bIsClickedInInventory;

        public Button ExitBtn;

        public DisplayableImageBox Top_EquipNameImageBox;
        public DisplayableImageBox Top_EquipTierImageBox;

        public DisplayableImageBox EquipInfo_EquipIconImageBox;
        public TextMeshProUGUI EquipInfo_TierNumText;
        public DisplayableImageBox EquipInfo_DescriptionImageBox;
        public GameObject MiddleInfo_StatVerticLayout;

        public GameObject EmptyTextBlock;

        public Button Button;
        public TextMeshProUGUI Button_Text;
        void Start()
        {
            ExitBtn.onClick.AddListener(OnExitBtnClicked);
            gameObject.SetActive(false);
        }

        public void PopupItemPanel(EquipData equipData, bool bIsClickedAtInventory, RLCharacterData currCharacter)
        {
            RWeaponType characterWeaponType = currCharacter.weaponType;
            DisplayedEquipData = equipData;
            bIsClickedInInventory = bIsClickedAtInventory;

            ClearVerticalScrollView();

            Top_EquipNameImageBox.SetText(equipData.GetDisplayableName());

            RLItemTier equipTier = (RLItemTier)equipData.GetExtraInfo();

            string equipDescription = equipData.equipDescription;
            string equipName = equipData.GetDisplayableName();
            Sprite equipIconSprite = equipData.GetDisplayableImage();

            string Text_Tier = "None";
            Color Color_Tier = RLTierColor.Common;
            switch (equipTier)
            {
                case RLItemTier.Common:
                    Text_Tier = "Common";
                    Color_Tier = RLTierColor.Common;
                    break;

                case RLItemTier.Rare:
                    Text_Tier = "Rare";
                    Color_Tier = RLTierColor.Rare;
                    break;

                case RLItemTier.Epic:
                    Text_Tier = "Epic";
                    Color_Tier = RLTierColor.Epic;
                    break;
            }

            Top_EquipTierImageBox.SetText(equipName);
            Top_EquipNameImageBox.SetDisplayableImageBackGroundColor(Color_Tier);

            Top_EquipTierImageBox.SetText(Text_Tier);
            Top_EquipTierImageBox.SetDisplayableImageBackGroundColor(Color_Tier);

            EquipInfo_EquipIconImageBox.SetDisplayableImageBackGroundColor(Color_Tier);
            EquipInfo_EquipIconImageBox.SetDisplayableImage(equipIconSprite);

            EquipInfo_TierNumText.SetText(((int)equipTier).ToString());

            EquipInfo_DescriptionImageBox.SetText(equipDescription);
            CreateStatTexts(equipData);

            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnClicked);

            Button_Text.SetText(bIsClickedAtInventory ? "Equip" : "Release");

            gameObject.SetActive(true);

            if (equipData.weaponType == RWeaponType.None)
            {
                Button.interactable = true;
                return;
            }


            if (characterWeaponType != equipData.weaponType)
                Button.interactable = false;

            else
                Button.interactable = true;

        }

        void ClearVerticalScrollView()
        {
            if (!MiddleInfo_StatVerticLayout)
                return;

            Transform verticLayoutTransform = MiddleInfo_StatVerticLayout.transform;

            for (int i = verticLayoutTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(verticLayoutTransform.GetChild(i).gameObject);
            }
        }

        void CreateStatTexts(EquipData equipData)
        {
            Transform verticLayoutTransform = MiddleInfo_StatVerticLayout.transform;

            foreach (EquipStat stat in equipData.equipStats)
            {
                GameObject statTextObject = Instantiate(EmptyTextBlock, verticLayoutTransform);

                TextMeshProUGUI tmp = statTextObject.GetComponent<TextMeshProUGUI>();

                StringBuilder sb = new StringBuilder(32);
                string StatName = stat.statName;
                int StatValue = stat.value;

                sb.Append(StatName);
                sb.Append(" : ");
                sb.Append(stat.value >= 0 ? "+" : "-");
                sb.Append(math.abs(stat.value));

                tmp.text = sb.ToString();
            }
        }

        void OnExitBtnClicked()
        {
            gameObject.SetActive(false);
        }

        void OnClicked()
        {
            if (bIsClickedInInventory == true)
            {
                OnEquipSelected?.Invoke(DisplayedEquipData);
            }

            else
            {
                OnEquipReleased?.Invoke(DisplayedEquipData);
            }

            gameObject.SetActive(false);
        }
    }
}

