using LUP.RL;
using OpenCvSharp.Flann;
using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class LobbyGameCenter : MonoBehaviour
    {
        [HideInInspector]
        public PlatformAdapter platformAdapter;
        //private RoguelikeStage stage;

        [SerializeField]
        private Canvas mainCanvas;

        public Button ChapterSelectBtn;

        public GameObject ChapterInfoText;
        public ChapterData[] chapterDatas { get; private set; }

        public RLCharacterData[] characterDatas { get; private set; }

        [SerializeField]
        private ChapterData selectedChapter;

        [SerializeField]
        private RLCharacterData selectedCharacter;

        [HideInInspector]
        private int ChapterDisplayedOffset = 0;

        private Button gameStartBtn;

        [HideInInspector]
        public bool IsInitializeReady = false;

        PannelController pannelController;

        public TextMeshProUGUI InventoryWood;
        public TextMeshProUGUI InventoryMeat;
        public TextMeshProUGUI InventoryCoin;

        public void Start()
        {
            //stage = FindFirstObjectByType<RoguelikeStage>();

            //if (stage == null)
            //    Debug.Log("Fail To Find Stage Object");
        }


        public void InitializeCenter()
        {
            IsInitializeReady = false;

            platformAdapter = new PlatformAdapter();

            if (platformAdapter != null)
            {
                platformAdapter.LinkToPlatform();
                //platformAdapter.LoadCharacterEquips();

                chapterDatas = platformAdapter.chapterDatas;
                characterDatas = platformAdapter.characterDatas;

                int savedLastSeletedChapter = platformAdapter.LastSeletedChapter;
                int savedLastSeletedCharacter = platformAdapter.LastSeletedCharacter;

                if ((savedLastSeletedChapter >= 0 && savedLastSeletedCharacter >= 0) &&
                    savedLastSeletedChapter < chapterDatas.Length && savedLastSeletedCharacter < characterDatas.Length)
                {
                    SetPastGameData(savedLastSeletedChapter, savedLastSeletedCharacter);
                    ChapterDisplayedOffset = savedLastSeletedChapter < 0 ? 0 : savedLastSeletedChapter;
                }

                else
                {
                    //이상현상 감지시 0으로 설정
                    SetPastGameData(0, 0);
                    ChapterDisplayedOffset = 0;
                }
            }

            if (mainCanvas == null)
            {
                UnityEngine.Debug.LogError("Bind Main Canvas!");
            }

            InitLobbyUIElement();

            IsInitializeReady = true;

            Time.timeScale = 1;
        }

        void Update()
        {

        }

        public void SetSelectedData(DisplayableDataType dataType, int index)
        {
            switch (dataType)
            {
                case DisplayableDataType.None:
                    UnityEngine.Debug.LogError("Set SelectedData Fail!!", this.gameObject);
                    return;

                case DisplayableDataType.ChapterData:
                    selectedChapter = chapterDatas[index];
                    ChapterSelectBtn.image.sprite = chapterDatas[index].GetDisplayableImage();
                    ChapterDisplayedOffset = index;

                    UpdateLobbyStageInfo(selectedChapter);
                    break;

                case DisplayableDataType.CharacterData:
                    selectedCharacter = characterDatas[index];
                    //CharacterSelectBtn.buttonImage.sprite = characterDatas[index].GetDisplayableImage();
                    break;
            }

        }

        private void OnGameStart()
        {
            Debug.Log("게임 시작 버튼 클릭!");

            if (selectedChapter == null || selectedCharacter == null)
            {
                Debug.Log("Selection Not Ready!");
                return;
            }

            platformAdapter.UploadSelectionData(selectedChapter, selectedCharacter);

            platformAdapter.LoadGameScene();
        }

        void InitLobbyUIElement()
        {

            {
                ButtonRule[] buttonRules = mainCanvas.GetComponentsInChildren<ButtonRule>();

                for (int i = 0; i < buttonRules.Length; i++)
                {
                    if (buttonRules[i].buttonRole == ButtonRole.GameStartBtn)
                    {
                        gameStartBtn = buttonRules[i].gameObject.GetComponent<Button>();
                        break;
                    }

                }

                gameStartBtn.onClick.AddListener(OnGameStart);
            }

            {
                pannelController = FindFirstObjectByType<PannelController>();

                if (pannelController == null)
                {
                    UnityEngine.Debug.LogError("Pannel Controller Not Found!");
                }

                pannelController.InitPannelCntroller();
            }

            //계정 재화  표시
            {
                int Num_Wood = platformAdapter.GetItemAmountInInventory(RLItemID.Wood);
                int Num_Meat = platformAdapter.GetItemAmountInInventory(RLItemID.Meat);
                int Num_Coin = platformAdapter.GetItemAmountInInventory(RLItemID.Coin);

                InventoryWood.SetText(Num_Wood.ToString());
                InventoryMeat.SetText(Num_Meat.ToString());
                InventoryCoin.SetText(Num_Coin.ToString());
            }
        }

        void SetPastGameData(int savedLastSeletedChapter, int savedLastSeletedCharacter)
        {
            selectedChapter = chapterDatas[savedLastSeletedChapter];
            selectedCharacter = characterDatas[savedLastSeletedCharacter];

            SetSelectedData(DisplayableDataType.ChapterData, savedLastSeletedChapter);
        }

        void UpdateLobbyStageInfo(ChapterData chapterData)
        {
            TextMeshProUGUI[] textInfos = ChapterInfoText.GetComponentsInChildren<TextMeshProUGUI>();

            textInfos[0].SetText(chapterData.GetDisplayableName());
        }

        public ChapterData GetselectedChapter()
        {
            return selectedChapter;
        }

        public RLCharacterData GetselectedCharacter()
        {
            return selectedCharacter;
        }

        public int GetChapterDisplayedOffset()
        {
            return ChapterDisplayedOffset;
        }
    }

}

