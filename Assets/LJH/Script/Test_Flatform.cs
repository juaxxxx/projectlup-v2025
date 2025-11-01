using Roguelike.Define;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test_Flatform : MonoBehaviour
{
    [SerializeField]
    private ChapterData[] rogueLikeChapterDatas;

    [SerializeField]
    private CharacterData[] rogueLikecharacterDatas;

    [SerializeField]
    private ItemData[] rogueLikespawnableItemDatas;

    [SerializeField]
    private ItemData[] rogueLikeInventoryItmeDatas;

    [SerializeField]
    private BuffData[] rogueLikeBuffDatas;

    public ChapterData[] chapterDatas => (ChapterData[])rogueLikeChapterDatas.Clone();
    public CharacterData[] characterDatas => (CharacterData[])rogueLikecharacterDatas.Clone();

    public ItemData[] spawnableItemDatas => (ItemData[])rogueLikespawnableItemDatas.Clone();
    public BuffData[] buffDatas => (BuffData[])rogueLikeBuffDatas.Clone();

    public ItemData[] inventoryItmeDatas => (ItemData[])rogueLikeInventoryItmeDatas.Clone();

    public ChapterData selectedChapter;
    public CharacterData selectedCharacter;

    public int LastSeletedChapter { get; private set; } = -1;
    public int LastSeletedCharacter { get; private set; } = -1;

    private static bool isOriginal = false;

    public void Awake()
    {
        if(!isOriginal)
        {
            DontDestroyOnLoad(this);
            isOriginal = true;
        }

        else
        {
            Destroy(this);
        }
    }

    public void LoadRogueLikeGameScene(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadRogueLikeLobbyScene(string lobbySceneName)
    {
        SceneManager.LoadScene(lobbySceneName);
    }

    public void UploadSelectionDataToFlatform(ChapterData selectChapter, CharacterData selectCharacter)
    {
        selectedChapter = selectChapter;
        selectedCharacter = selectCharacter;
    }

    public (ChapterData, CharacterData) GetSelectionData()
    {
        return (selectedChapter, selectedCharacter);
    }

    public void UploadGameResult(Dictionary<ItemData, int> gainItem, int resultChapter, int resultCharacter)
    {
        //gainItem
        LastSeletedChapter = resultChapter;
        LastSeletedCharacter = resultCharacter;


    }
}
