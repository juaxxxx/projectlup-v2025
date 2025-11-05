using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.VisualScripting;

using Roguelike.Define;
using Roguelike.Util;
using System.Collections;

public class LobbyGameCenter : MonoBehaviour
{
    PlatformAdapter platformAdapter;

    //[SerializeField]
    //private Canvas mainCanvas;

    //private CircleButton CharacterSelectBtn;
    //private CircleButton QuestListBtn;
    //private CircleButton ReturnMainGame;
    public Button ChapterSelectBtn;
    //private Button GameStartBtn;

    public GameObject ChapterInfoText;
    public ChapterData[] chapterDatas { get; private set; }

    public RLCharacterData[] characterDatas { get; private set; }

    [SerializeField]
    private ChapterData selectedChapter;

    [SerializeField]
    private RLCharacterData selectedCharacter;

    [HideInInspector]

    private int ChapterDisplayedOffset = 0;
    //private int CharacterScrollSelectionOffset = 0;

    //private void OnEnable()
    //{
    //    InitLobbyUIElement();
    //}

    

    void Start()
    {
        platformAdapter = new PlatformAdapter();

        if (platformAdapter != null)
        {
            platformAdapter.LinkToPlatform();

            chapterDatas = platformAdapter.chapterDatas;
            characterDatas = platformAdapter.characterDatas;

            int savedLastSeletedChapter = platformAdapter.LastSeletedChapter;
            int savedLastSeletedCharacter = platformAdapter.LastSeletedCharacter;

            if ((savedLastSeletedChapter > 0 && savedLastSeletedCharacter > 0) &&
                savedLastSeletedChapter < chapterDatas.Length && savedLastSeletedCharacter < characterDatas.Length)
            {
                SetPastGameData(savedLastSeletedChapter, savedLastSeletedCharacter);
            }

            else
            {
                //이상현상 감지시 0으로 설정
                SetPastGameData(0, 0);
            }

            ChapterDisplayedOffset = savedLastSeletedChapter < 0 ? 0 : savedLastSeletedChapter;
            //CharacterScrollSelectionOffset = savedLastSeletedCharacter < 0 ? 0 : savedLastSeletedCharacter;
        }

        //if (mainCanvas == null)
        //{
        //    GameObject obj = GameObject.FindWithTag("Lobby_MainCanvas");
        //    mainCanvas = obj.GetComponent<Canvas>();
        //}

        //InitLobbyUIElement();

        Time.timeScale = 1;
    }

    // Update is called once per frame
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    //private void OnCharacterSelect()
    //{
    //    CharacterSelectionScrollPanel scrollAblePanel = mainCanvas.GetComponentInChildren<CharacterSelectionScrollPanel>(true);
    //    scrollAblePanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid);
    //    scrollAblePanel.OpenPanel(characterDatas, DisplayableDataType.CharacterData);
    //    scrollAblePanel.InitPreviewData(selectedCharacter);
    //}

    private void OnChapterSelect()
    {
        //ChapterSelectionScrollPanel scrollAblePanel = mainCanvas.GetComponentInChildren<ChapterSelectionScrollPanel>(true);
        //scrollAblePanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Horizontal);
        //scrollAblePanel.OpenPanel(chapterDatas, DisplayableDataType.ChapterData, ChapterDisplayedOffset);
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

    //void InitLobbyUIElement()
    //{
    //    {
    //        ButtonRule[] uiButtons = mainCanvas.GetComponentsInChildren<ButtonRule>();

    //        for (int i = 0; i < uiButtons.Length; i++)
    //        {
    //            ButtonRole buttonRole = uiButtons[i].buttonRole;

    //            switch (buttonRole)
    //            {
    //                case ButtonRole.None:
    //                    UnityEngine.Debug.LogError("ButtonRole Is Not Assine", this.gameObject);
    //                    break;

    //                case ButtonRole.BackToMainBtn:
    //                    ReturnMainGame = uiButtons[i].GetComponent<CircleButton>();
    //                    break;

    //                case ButtonRole.ChapterSelectionBtn:
    //                    ChapterSelectBtn = uiButtons[i].GetComponent<Button>();
    //                    break;

    //                case ButtonRole.CharacterSelectionBtn:
    //                    CharacterSelectBtn = uiButtons[i].GetComponent<CircleButton>();
    //                    break;

    //                case ButtonRole.QuestselectionBtn:
    //                    QuestListBtn = uiButtons[i].GetComponent<CircleButton>();
    //                    break;

    //                case ButtonRole.GameStartBtn:
    //                    GameStartBtn = uiButtons[i].GetComponent<Button>();
    //                    break;
    //            }
    //        }
    //    }

    //    {
    //        if (CharacterSelectBtn && ChapterSelectBtn && GameStartBtn)
    //        {
    //            CheckCircleBtnValid();

    //            CharacterSelectBtn.button.onClick.AddListener(OnCharacterSelect);
    //            ChapterSelectBtn.onClick.AddListener(OnChapterSelect);
    //            GameStartBtn.onClick.AddListener(OnGameStart);
    //        }

    //        else
    //        {
    //            UnityEngine.Debug.LogWarning("Check Selection Btn", this.gameObject);
    //        }
    //    }
    //}

    void SetPastGameData(int savedLastSeletedChapter, int savedLastSeletedCharacter)
    {
        //SelectedChapterImage.sprite = chapterDatas[savedLastSeletedChapter].GetDisplayableImage();
        //SelectedCharacterImage.sprite = characterDatas[savedLastSeletedCharacter].GetDisplayableImage();

        //ChapterSelectBtn.image.sprite = chapterDatas[savedLastSeletedChapter].GetDisplayableImage();
        //CharacterSelectBtn.buttonImage.sprite = characterDatas[savedLastSeletedCharacter].GetDisplayableImage();

        selectedChapter = chapterDatas[savedLastSeletedChapter];
        selectedCharacter = characterDatas[savedLastSeletedCharacter];

        //UpdateLobbyStageInfo(selectedChapter);
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

    //bool CheckCircleBtnValid()
    //{
    //    if(CharacterSelectBtn.button == null)
    //    {
    //        CharacterSelectBtn.ManualAwkae();
    //    }

    //    return true;
    //}
}
