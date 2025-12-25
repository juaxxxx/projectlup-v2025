using LUP.DSG;
using Roguelike.Define;
using Roguelike.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class InventorPanel : LobbyContentAblePannel
    {
        public GameObject ItemInventoryPanel;
        public CharacterSelectionScrollPanel CharacterSelectPanel;

        public InventoryCharacterEquipPanel InventoryCharacterEquipPanel;
        public InventoryItemAlliner InventoryItemAlliner;
        public InventoryItemGridLayout InventoryItemGridLayout;

        public Scrollbar[] scrollbars;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private List<Scrollbar> inventoryMovingVerticScrollBar = new List<Scrollbar>();
        new void Start()
        {
            base.Start();
        }

        public override void InitPanel()
        {
            if (ItemInventoryPanel == null || CharacterSelectPanel == null)
            {
                UnityEngine.Debug.LogError("Fail To Find Inventory's Panel");
            }

            {
                InventoryCharacterEquipPanel.Init();
                InventoryItemAlliner.Init();
                InventoryItemGridLayout.Init();
            }

            activatedVecticScrollbar = scrollbars[0];

            CharacterSelectPanel.gameObject.SetActive(false);
        }

        public void ReciveBtnActioFromSelectPanel(int index)
        {
            CharacterSelectPanel.gameObject.SetActive(true);

            if (index == 0)
            {
                activatedVecticScrollbar = scrollbars[1];


                OnCharacterSelect();
            }
            pannelController.SetAllMainScrollActive(false);
            pannelController.SetActiveVerticScroll(activatedVecticScrollbar);
        }

        private void OnCharacterSelect()
        {
            LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

            CharacterSelectPanel.gameObject.SetActive(true);
            CharacterSelectPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid);
            CharacterSelectPanel.OpenPanel(lobbyGameCenter.characterDatas, DisplayableDataType.CharacterData);
            CharacterSelectPanel.InitPreviewData(lobbyGameCenter.GetselectedCharacter());
        }

        public override void OnSubPanelErase()
        {
            activatedVecticScrollbar = scrollbars[0];
            pannelController.SetActiveVerticScroll(activatedVecticScrollbar);
            pannelController.SetAllMainScrollActive(true);
        }

        public override void MoveTo()
        {
            activatedVecticScrollbar = scrollbars[0];
            activatedVecticScrollbar.value = 1;
            pannelController.SetActiveVerticScroll(activatedVecticScrollbar);
            pannelController.SetAllMainScrollActive(true);

        }

        public bool OnItemEquiped(EquipData equipData)
        {
            RLCharacterData characterData = pannelController.lobbyGameCenter.GetselectedCharacter();

            if (CheckCanEquipToSlot(characterData.EquipItems, equipData))
            {
                EquipItem(ref characterData.EquipItems, equipData);
                InventoryCharacterEquipPanel.UpdateCharacterEquipSlot(characterData.EquipItems);

                return true;
            }

            return false;


        }

        public void OnItemReleased(EquipData equipData)
        {
            RLCharacterData characterData = pannelController.lobbyGameCenter.GetselectedCharacter();
            RelaseItem(ref characterData.EquipItems, equipData);


            InventoryCharacterEquipPanel.UpdateCharacterEquipSlot(characterData.EquipItems);
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool CheckCanEquipToSlot(CharacterEquipsID equips, EquipData newEquip)
        {
            switch (newEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    return equips.Weapon == 0;

                case RLEquipPos.Body:
                    return equips.Armor == 0;

                case RLEquipPos.Finger:
                    return equips.Ring1 == 0 || equips.Ring2 == 0;

                case RLEquipPos.Arm:
                    return equips.Bracelet == 0;

                case RLEquipPos.Neck:
                    return equips.Necklace == 0;

                default:
                    return false;
            }
        }

        void EquipItem(ref CharacterEquipsID equips, EquipData newEquip)
        {
            int itemID = ItemManager.Instance.GetItem(newEquip.GetDisplayableName()).ItemID;

            switch (newEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    equips.Weapon = itemID;
                    break;

                case RLEquipPos.Body:
                    equips.Armor = itemID;
                    break;

                case RLEquipPos.Finger:
                    if (equips.Ring1 == 0)
                        equips.Ring1 = itemID;
                    else
                        equips.Ring2 = itemID;
                    break;

                case RLEquipPos.Arm:
                    equips.Bracelet = itemID;
                    break;

                case RLEquipPos.Neck:
                    equips.Necklace = itemID;
                    break;
            }
        }

        void RelaseItem(ref CharacterEquipsID equips, EquipData targetEquip)
        {
            int itemID = ItemManager.Instance.GetItem(targetEquip.GetDisplayableName()).ItemID;

            switch (targetEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    if (equips.Weapon == itemID)
                        equips.Weapon = 0;
                    break;

                case RLEquipPos.Body:
                    if (equips.Armor == itemID)
                        equips.Armor = 0;
                    break;

                case RLEquipPos.Finger:
                    if (equips.Ring1 == itemID)
                        equips.Ring1 = 0;
                    else if (equips.Ring2 == itemID)
                        equips.Ring2 = 0;
                    break;

                case RLEquipPos.Arm:
                    if (equips.Bracelet == itemID)
                        equips.Bracelet = 0;
                    break;

                case RLEquipPos.Neck:
                    if (equips.Necklace == itemID)
                        equips.Necklace = 0;
                    break;
            }
        }

        public void UpdateEquipInventoryGridPanel()
        {
            InventoryItemGridLayout.LoadInventoryItemData();
        }
    }
}

