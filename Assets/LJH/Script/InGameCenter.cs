using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Roguelike.Define;
using Roguelike.Util;

namespace RL
{
    public class InGameCenter : MonoBehaviour
    {
        [Header("레벨데이터")]
        public LevelDataTable levelTable;

        [SerializeField]
        private GameObject mainCanvas;

        public GameObject gameResultPanel;
        public GameObject gamePausePanel;

        PlatformAdapter platformAdapter;

        [SerializeField]
        private ChapterData chapterData;

        [SerializeField]
        private RLCharacterData characterData;

        private ItemData[] spawnableItemDatas;
        private Dictionary<ItemData, int> gainItem = new Dictionary<ItemData, int>();

        private StageController stageController;

        public bool gameClear = false;

        //------Temp Test Button
        public Button AddItem1Btn;
        public Button AddItem2Btn;
        public Button AddItem3Btn;
        public Button AddTestItemBtn;

        public Button ClearGameBtn;
        public Button DebugBtn;

        private bool debugMode = false;

        public GameObject DebugPanel;
        //------------------------

        private CircleButton pauseBtn;
        public Button Confirm;

        private Archer controlledPlayer = null;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            platformAdapter = new PlatformAdapter();

            if (platformAdapter != null)
            {
                platformAdapter.LinkToPlatform();
                platformAdapter.LoadSpawnableItemData();

                LoadInGameData();

            }

            {
                stageController = GameObject.FindFirstObjectByType<StageController>();
                if (stageController == null)
                {
                    UnityEngine.Debug.LogError("Fail To Find StageController!", this.gameObject);
                }

                else
                {
                    stageController.onStageClear.AddListener(GameClear);
                    controlledPlayer = stageController.player.gameObject.GetComponent<Archer>();

                    if (controlledPlayer == null)
                        UnityEngine.Debug.LogError("Fail to Find Player!!");
                }
            }

            InitInGameUIElement();

            //Temp
            debugMode = false;

            AddItem1Btn.onClick.AddListener(AddItem1);
            AddItem2Btn.onClick.AddListener(AddItem2);
            AddItem3Btn.onClick.AddListener(AddItem3);

            AddTestItemBtn.onClick.AddListener(AddTestItem);

            ClearGameBtn.onClick.AddListener(GameClear);
            DebugBtn.onClick.AddListener(ChangeDebugMode);

            SetDebugMode(debugMode);

            DebugPanel.SetActive(false);
            ///////////

            Confirm.onClick.AddListener(UploadGameResult);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LoadInGameData()
        {
            LoadSelectionData();
            LoadSpawnableItemData();
        }

        void LoadSelectionData()
        {
            if (platformAdapter.LoadSelectionData())
            {
                chapterData = platformAdapter.selectedChapter;
                characterData = platformAdapter.selectedCharacter;
            }

            else
            {
                UnityEngine.Debug.LogError("Fail To Load Selection Data", this.gameObject);
            }
        }

        void LoadSpawnableItemData()
        {
            if (platformAdapter.LoadSpawnableItemData())
            {
                spawnableItemDatas = platformAdapter.spawnableItemDatas;
            }

            else
            {
                UnityEngine.Debug.LogWarning("SpawnableItemData is Empty!", this.gameObject);
            }
        }

        void InitInGameUIElement()
        {
            {
                //gameResultPanel = GameObject.FindWithTag("InGame_GameResultPanel");
                //if (gameResultPanel == null)
                //{
                //    UnityEngine.Debug.LogError("Can't Find InGame ResultPanel by Tag", this.gameObject);
                //    return;
                //}
                if (gameResultPanel != null)
                {
                    gameResultPanel.SetActive(false);
                }

            }


            //{
            //    gamePausePanel = GameObject.FindWithTag("GamePausePanel");

            //    if (gamePausePanel == null)
            //        UnityEngine.Debug.LogError("Can't Find InGame GamePausePanel by Tag");

            //    //pauseBtn.gameObject.SetActive(true);
            //}

            {
                ButtonRule[] circleButtons = mainCanvas.GetComponentsInChildren<ButtonRule>();

                for (int i = 0; i < circleButtons.Length; i++)
                {
                    ButtonRole buttonRole = circleButtons[i].buttonRole;

                    switch (buttonRole)
                    {
                        case ButtonRole.PauseGameBtn:
                            pauseBtn = circleButtons[i].gameObject.GetComponent<CircleButton>();
                            pauseBtn.button.onClick.AddListener(OnPauseBtnClicked);
                            break;
                    }
                }
            }

        }

        void OnPauseBtnClicked()
        {
            Time.timeScale = 0f;

            ShowPausePanel();
        }

        void ShowPausePanel()
        {
            OwningBuffListScrollPanel buffScrollPanel = mainCanvas.GetComponentInChildren<OwningBuffListScrollPanel>(true);
            buffScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid, TextAnchor.UpperLeft);

            IDisplayable[] buffs = controlledPlayer.GetActiveBufflist().ToArray();

            gamePausePanel.SetActive(true);
            buffScrollPanel.OpenPanel(buffs, DisplayableDataType.ItemData);
        }

        public void AddItem(ItemData pickedItem)
        {
            if (!gainItem.ContainsKey(pickedItem))
            {
                gainItem[pickedItem] = 1;
            }

            else
            {
                gainItem[pickedItem]++;
            }
        }

        void AddItem1()
        {
            AddItem(spawnableItemDatas[0]);
        }

        void AddItem2()
        {
            AddItem(spawnableItemDatas[1]);
        }

        void AddItem3()
        {
            AddItem(spawnableItemDatas[2]);
        }

        void AddTestItem()
        {
            ItemData TestItem = ScriptableObject.CreateInstance<ItemData>();
            TestItem.SetDisplayableImage(spawnableItemDatas[2].GetDisplayableImage());

            AddItem(TestItem);
        }

        void GameClear()
        {
            Debug.Log("클리어 버튼");
            gameResultPanel.SetActive(true);

            ShowGameResult();


            gameClear = true;
            //pauseBtn.gameObject.SetActive(false);

        }

        void ChangeDebugMode()
        {
            debugMode = !debugMode;

            SetDebugMode(debugMode);
        }

        void SetDebugMode(bool enable)
        {
            if(enable)
            {
                Time.timeScale = 0f;
            }

            else
            {
                Time.timeScale = 1f;
            }

                

            DebugPanel.SetActive(enable);

            AddItem1Btn.gameObject.SetActive(enable);
            AddItem2Btn.gameObject.SetActive(enable);
            AddItem3Btn.gameObject.SetActive(enable);
            AddTestItemBtn.gameObject.SetActive(enable);

            ClearGameBtn.gameObject.SetActive(enable);
        }

        void ShowGameResult()
        {
            Time.timeScale = 0;

            ResultGainItemScrollPanel resultIteScrollPanel = mainCanvas.GetComponentInChildren<ResultGainItemScrollPanel>(true);
            resultIteScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid);

            IDisplayable[] gainedItems = MakeGainItemArray();

            resultIteScrollPanel.OpenPanel(gainedItems, DisplayableDataType.ItemData);
        }

        void UploadGameResult()
        {
            platformAdapter.ApplyGameResult(gainItem, chapterData, characterData, gameClear);
        }

        IDisplayable[] MakeGainItemArray()
        {
            int count = gainItem.Count;
            IDisplayable[] gainedItem = new IDisplayable[count];

            int index = 0;
            foreach (var pair in gainItem)
            {
                var itemData = pair.Key;
                var value = pair.Value;

                gainedItem[index] = itemData;
                gainedItem[index].SetExtraInfo(value);

                index++;
            }

            return gainedItem;
        }
    }
}

