using System.Collections.Generic;
using UnityEngine;

using Roguelike.Define;
using Roguelike.Util;

public class PlatformAdapter
{
    private Test_Flatform platform;

    public ChapterData[] chapterDatas { get; private set; }
    public RLCharacterData[] characterDatas { get; private set; }

    public ItemData[] spawnableItemDatas { get; private set; }

    public ItemData[] inventoryItmeDatas { get; private set; }

    public BuffData[] gainableBuffDatas { get; private set; }

    public ChapterData selectedChapter { get; set; }
    public RLCharacterData selectedCharacter { get; set; }

    public int LastSeletedChapter { get; set; }
    public int LastSeletedCharacter { get; set; }


    public void LinkToPlatform()
    {
        platform = GameObject.FindFirstObjectByType<Test_Flatform>();

        if(platform == null)
        {
            UnityEngine.Debug.LogError("Fail to Licnk Platform");
            return;
        }

        else
        {
            if(!Synchronizing())
            {
                UnityEngine.Debug.LogError("Fail to Sync Platform data");
            }
        }
    }

    bool Synchronizing()
    {
        chapterDatas = platform.chapterDatas;
        characterDatas = platform.characterDatas;

        LastSeletedChapter = platform.LastSeletedChapter;
        LastSeletedCharacter = platform.LastSeletedCharacter;

        gainableBuffDatas = platform.buffDatas;

        inventoryItmeDatas = platform.inventoryItmeDatas;

        if ((chapterDatas == null || chapterDatas.Length == 0) ||
            (characterDatas == null || characterDatas.Length == 0))
        {
            return false;
        }

        return true;
    }

    public void UploadSelectionData(ChapterData selectedChapter, RLCharacterData selectedCharacter)
    {
        platform.UploadSelectionDataToFlatform(selectedChapter, selectedCharacter);
    }

    public bool LoadSelectionData()
    {
        var (SelectedChapter, SelectedCharacter) = platform.GetSelectionData();

        if (SelectedChapter != null && SelectedCharacter != null)
        {
            selectedChapter = SelectedChapter;
            selectedCharacter = SelectedCharacter;
            return true;
        }
            

        return false;
    }

    public void LoadLobbyScene()
    {
        platform.LoadRogueLikeLobbyScene(RoguelikeScene.LobbyScene);
    }

    public void LoadGameScene()
    {
        platform.LoadRogueLikeGameScene(RoguelikeScene.GameScene);
    }

    public bool LoadSpawnableItemData()
    {
        spawnableItemDatas = platform.spawnableItemDatas;

        if(spawnableItemDatas.Length == 0)
        {
            return false;
        }

        return true;
    }

    public void ApplyGameResult(Dictionary<ItemData, int> gainItem, ChapterData resultCapter, RLCharacterData resultCharacter, bool stageCleared = true)
    {
        int chapterIndex = -1;
        int characterIndex = -1;

        for(int i = 0; i < chapterDatas.Length; i++)
        {
            if (chapterDatas[i] == resultCapter)
            {
                chapterIndex = i;
                break;
            }
                
        }

        for(int i = 0; i < characterDatas.Length; i++)
        {
            if (characterDatas[i] == resultCharacter)
            {
                characterIndex = i;
                break;
            }
                
        }

        if(chapterIndex == -1 || characterIndex == -1)
        {
            UnityEngine.Debug.LogError("Fail To Apply GameResult");
            return;
        }

        if(stageCleared)
        {
            if (chapterIndex < characterDatas.Length - 1)
                chapterIndex++;
        }

        platform.UploadGameResult(gainItem, chapterIndex, characterIndex);

        platform.LoadRogueLikeLobbyScene(RoguelikeScene.LobbyScene);
    }
}
