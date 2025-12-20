using LUP.RL;
using Roguelike.Define;
using Roguelike.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.RL
{
    public enum EquipSlotType
    {
        Weapon = 0,
        Armor = 1,
        Ring1 = 2,
        Ring2 = 3,
        Pet1 = 4,
        Pet2 = 5,
        Bracelet = 6,
        Necklace = 7
    }

    public class InventoryCharacterEquipPanel : MonoBehaviour, IPanelContentAble
    {
        private PannelController pannelController = null;
        private Vector2 parentViewportSize;
        private CharacterPreviewAnimation inventoryCharacterPreviewAnimImage;

        [SerializeField]
        private TextImageBtn[] slots;


        void Start()
        {
            inventoryCharacterPreviewAnimImage = gameObject.GetComponentInChildren<CharacterPreviewAnimation>();
        }

        public bool Init()
        {
            showPanel();
            return true;
        }

        void showPanel()
        {
            parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
            Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, parentViewportSize.y * 0.6f);
            gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;

            SetPastGameData();
        }

        void SetPastGameData()
        {
            LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();
            string characterName = lobbyGameCenter.GetselectedCharacter().Name;

            SetInventoryCharacterPrieViewAnimImage(characterName);

        }

        public void SetInventoryCharacterPrieViewAnimImage(string characterName)
        {
            inventoryCharacterPreviewAnimImage.ChangeSpriteSheet(characterName);
        }

        public void UpdateCharacterEquipSlot(EquipmentData characterequipsInfo)
        {
            EraseAllEquipSlot();

            if (characterequipsInfo.Weapon != null) { SetCharacterEquipSlot(characterequipsInfo.Weapon, EquipSlotType.Weapon); }
            if (characterequipsInfo.Armor != null) { SetCharacterEquipSlot(characterequipsInfo.Armor, EquipSlotType.Armor); }
            if (characterequipsInfo.Ring1 != null) { SetCharacterEquipSlot(characterequipsInfo.Ring1, EquipSlotType.Ring1); }
            if (characterequipsInfo.Ring2 != null) { SetCharacterEquipSlot(characterequipsInfo.Ring2, EquipSlotType.Ring2); }
            if (characterequipsInfo.Pet1 != null) { SetCharacterEquipSlot(characterequipsInfo.Pet1, EquipSlotType.Pet1); }
            if (characterequipsInfo.Pet2 != null) { SetCharacterEquipSlot(characterequipsInfo.Pet2, EquipSlotType.Pet2); }
            if (characterequipsInfo.Bracelet != null) { SetCharacterEquipSlot(characterequipsInfo.Bracelet, EquipSlotType.Bracelet); }
            if (characterequipsInfo.Necklace != null) { SetCharacterEquipSlot(characterequipsInfo.Necklace, EquipSlotType.Necklace); }
        }

        void SetCharacterEquipSlot(EquipData equipData, EquipSlotType slotType)
        {
            TextImageBtn targetTextImageBtn = slots[(int)slotType];

            RLItemTier equipTier = (RLItemTier)equipData.GetExtraInfo();
            Color Color_Tier = RLTierColor.Common;
            switch (equipTier)
            {
                case RLItemTier.Common:
                    Color_Tier = RLTierColor.Common;
                    break;

                case RLItemTier.Rare:
                    Color_Tier = RLTierColor.Rare;
                    break;

                case RLItemTier.Epic:
                    Color_Tier = RLTierColor.Epic;
                    break;
            }

            EquipData equipedData = equipData;

            targetTextImageBtn.Init();

            targetTextImageBtn.btnBackGroundImage.color = Color_Tier;
            targetTextImageBtn.btnIcon.sprite = equipData.GetDisplayableImage();
            targetTextImageBtn.btnIcon.color = Color.white;
            targetTextImageBtn.button.onClick.AddListener(() => OnValidEquipSlotClicked(equipedData));
        }

        void EraseAllEquipSlot()
        {

        }

        void OnValidEquipSlotClicked(EquipData equipData)
        {
            if(pannelController == null)
            {
                pannelController = FindFirstObjectByType<PannelController>();
            }

            pannelController.PopEquipPanel(equipData, false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

