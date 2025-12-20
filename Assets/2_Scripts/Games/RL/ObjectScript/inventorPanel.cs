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

        public void OnItemEquiped(EquipData equipData)
        {
            RLCharacterData characterData = pannelController.lobbyGameCenter.GetselectedCharacter();

            if (CheckCanEquipToSlot(characterData.EquipItems, equipData))
            {
                EquipItem(ref characterData.EquipItems, equipData);
            }

            InventoryCharacterEquipPanel.UpdateCharacterEquipSlot(characterData.EquipItems);


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

        bool CheckCanEquipToSlot(EquipmentData equips, EquipData newEquip)
        {
            switch (newEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    return equips.Weapon == null;

                case RLEquipPos.Body:
                    return equips.Armor == null;

                case RLEquipPos.Finger:
                    return equips.Ring1 == null || equips.Ring2 == null;

                case RLEquipPos.Arm:
                    return equips.Bracelet == null;

                case RLEquipPos.Neck:
                    return equips.Necklace == null;

                default:
                    return false;
            }
        }

        void EquipItem(ref EquipmentData equips, EquipData newEquip)
        {
            switch (newEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    equips.Weapon = newEquip;
                    break;

                case RLEquipPos.Body:
                    equips.Armor = newEquip;
                    break;

                case RLEquipPos.Finger:
                    if (equips.Ring1 == null)
                        equips.Ring1 = newEquip;
                    else
                        equips.Ring2 = newEquip;
                    break;

                case RLEquipPos.Arm:
                    equips.Bracelet = newEquip;
                    break;

                case RLEquipPos.Neck:
                    equips.Necklace = newEquip;
                    break;
            }
        }

        void RelaseItem(ref EquipmentData equips, EquipData targetEquip)
        {
            switch (targetEquip.equipPos)
            {
                case RLEquipPos.Hand:
                    if (equips.Weapon == targetEquip)
                        equips.Weapon = null;
                    break;

                case RLEquipPos.Body:
                    if (equips.Armor == targetEquip)
                        equips.Armor = null;
                    break;

                case RLEquipPos.Finger:
                    if (equips.Ring1 == targetEquip)
                        equips.Ring1 = null;
                    else if (equips.Ring2 == targetEquip)
                        equips.Ring2 = null;
                    break;

                case RLEquipPos.Arm:
                    if (equips.Bracelet == targetEquip)
                        equips.Bracelet = null;
                    break;

                case RLEquipPos.Neck:
                    if (equips.Necklace == targetEquip)
                        equips.Necklace = null;
                    break;
            }
        }

        public void UpdateEquipInventoryGridPanel()
        {
            InventoryItemGridLayout.LoadInventoryItemData();
        }
    }
}

