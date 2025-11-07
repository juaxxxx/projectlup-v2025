using Roguelike.Define;
using Roguelike.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RL
{
    public class InventorPanel : LobbyContentAblePannel
    {
        public GameObject ItemInventoryPanel;
        public CharacterSelectionScrollPanel CharacterSelectPanel;

        public Scrollbar[] scrollbars;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private List<Scrollbar> inventoryMovingVerticScrollBar = new List<Scrollbar>();
        new void Start()
        {
            base.Start();

            StartCoroutine(RoguelikeUtil.DelayOneFrame(() => PostPanelInitShop()));
        }

        void PostPanelInitShop()
        {
            Init();
        }

        void Init()
        {
            if (ItemInventoryPanel == null || CharacterSelectPanel == null)
            {
                UnityEngine.Debug.LogError("Fail To Find Inventory's Panel");
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
            //pannelController.SetActiveInputcacher(false);
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}

