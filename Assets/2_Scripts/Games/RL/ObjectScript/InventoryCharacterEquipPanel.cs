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

        private LobbyGameCenter lobbyGameCenter;


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
            if (lobbyGameCenter == null)
                lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

            RLCharacterData LastSelectedCharacter = lobbyGameCenter.GetselectedCharacter();

            SetInventoryCharacterPrieViewAnimImage(LastSelectedCharacter.Name);
            UpdateCharacterEquipSlot(LastSelectedCharacter.EquipItems);

        }

        public void SetInventoryCharacterPrieViewAnimImage(string characterName)
        {
            inventoryCharacterPreviewAnimImage.ChangeSpriteSheet(characterName);
        }

        public void UpdateCharacterEquipSlot(CharacterEquipsID characterequipsInfo)
        {
            EraseAllEquipSlot();

            PlatformAdapter adapter = lobbyGameCenter.platformAdapter;

            if (characterequipsInfo.Weapon != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Weapon), EquipSlotType.Weapon); }
            if (characterequipsInfo.Armor != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Armor), EquipSlotType.Armor); }
            if (characterequipsInfo.Ring1 != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Ring1), EquipSlotType.Ring1); }
            if (characterequipsInfo.Ring2 != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Ring2), EquipSlotType.Ring2); }
            if (characterequipsInfo.Pet1 != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Pet1), EquipSlotType.Pet1); }
            if (characterequipsInfo.Pet2 != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Pet2), EquipSlotType.Pet2); }
            if (characterequipsInfo.Bracelet != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Bracelet), EquipSlotType.Bracelet); }
            if (characterequipsInfo.Necklace != 0) { SetCharacterEquipSlot(adapter.GetEquipDataByID(characterequipsInfo.Necklace), EquipSlotType.Necklace); }
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
            for (int i = 0; i < slots.Length; i++)
            {
                TextImageBtn slot = slots[i];

                if (slot == null)
                    continue;

                if(slot.Init())
                {
                    slot.btnBackGroundImage.color = new Color(159f / 255f, 151f / 255f, 151f / 255f, 1f);
                    slot.btnIcon.sprite = null;
                    slot.btnIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    slot.button.onClick.RemoveAllListeners();
                }
                
            }
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

