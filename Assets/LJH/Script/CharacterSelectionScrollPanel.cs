using NUnit.Framework;
using Roguelike.Define;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelectionScrollPanel : BaseScrollAblePanel
{
    private CharacterPriveiwPanel characterPriveiw;
    private CharacterSelectionButtonPanel characterSelectionButton;
    private List<RLCharacterData> displayedCharacterData = new List<RLCharacterData>();

    private CharacterType characterTypeFilter = CharacterType.None;

    private RLCharacterData previewCharacterData = null;

    private void Awake()
    {
        characterPriveiw = GetComponentInChildren<CharacterPriveiwPanel>();
        characterSelectionButton = GetComponentInChildren<CharacterSelectionButtonPanel>();
        if (characterPriveiw == null || characterSelectionButton == null)
        {
            UnityEngine.Debug.LogError("Main CharacterSeletPanel fail to Load Other Panel");
        }


    }

    public void InitPreviewData(RLCharacterData characterData)
    {
        previewCharacterData = characterData;
        characterPriveiw.SetCharacterPreview(previewCharacterData);
    }

    protected override void GenerateContent()
    {
        UpdateCharacaterSelectPannel();

        //setconstraintCount();
    }

    public void OnSelectedCharacter()
    {
        LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

        if (lobbyGameCenter == null)
        {
            UnityEngine.Debug.LogError("LRGameCenter Is Null", this.gameObject);
            return;
        }

        int index = 0;
        for(; index < displayedData.Length; index++)
        {
            if ((RLCharacterData)displayedData[index] == previewCharacterData)
                break;
        }

        lobbyGameCenter.SetSelectedData(DisplayableDataType.CharacterData, index);

        {
            PannelController pannelController = FindFirstObjectByType<PannelController>();
            pannelController.OnSubPannelErase();
        }
        


        ErasePanel();
    }

    public void SetCharacterFilter(CharacterType characterType)
    {
        characterTypeFilter = characterType;

        UpdateCharacaterSelectPannel();
    }

    void UpdateCharacaterSelectPannel()
    {
        base.EraseContents();
        displayedCharacterData.Clear();

        for (int i = 0; i < displayedData.Length; i++)
        {
            RLCharacterData characterData = (RLCharacterData)displayedData[i];

            if (characterTypeFilter == CharacterType.None || characterData.characterType == characterTypeFilter)
                displayedCharacterData.Add(characterData);
        }

        for (int i = 0; i < displayedCharacterData.Count; i++)
        {
            int index = i;

            GameObject characterBtn = Instantiate(displayedPrefab, contentParent);
            characterBtn.GetComponent<Button>().onClick.AddListener(()=> OnDisplayedCharacterBtnSelected(index));

            DisplayableImageBox displayAbleCharacterImageBox = characterBtn.GetComponent<DisplayableImageBox>();

            if (displayAbleCharacterImageBox == null)
            {
                UnityEngine.Debug.LogError("Cast ImageBox Fail!", this.gameObject);
                continue;
            }

            displayAbleCharacterImageBox.SetDisplayableImage(displayedCharacterData[index].GetDisplayableImage());

            Transform textTransform = characterBtn.transform.Find("LevelText");

            //string characterLevel = displayedData[index].GetExtraInfo().ToString();
            string characterLevel = "-1";

            textTransform.GetComponent<TextMeshProUGUI>().SetText(characterLevel);
        }
    }

    void OnDisplayedCharacterBtnSelected(int index)
    {
        previewCharacterData = displayedCharacterData[index];
        characterPriveiw.SetCharacterPreview(previewCharacterData);
    }

}
