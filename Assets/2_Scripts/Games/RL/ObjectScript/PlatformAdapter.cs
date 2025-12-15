using LUP;
using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LUP.RL
{
    public class PlatformAdapter
    {
        private Test_Flatform testPlatform;

        private RoguelikeStage platform;

        public ChapterData[] chapterDatas { get; private set; }
        public RLCharacterData[] characterDatas { get; private set; }

        //public ItemData[] spawnableItemDatas { get; private set; }

        //public ItemData[] inventoryItmeDatas { get; private set; }

        public BuffData[] gainableBuffDatas { get; private set; }

        public ChapterData selectedChapter { get; set; }
        public RLCharacterData selectedCharacter { get; set; }

        public int LastSeletedChapter { get; set; }
        public int LastSeletedCharacter { get; set; }

        private RoguelikeRuntimeData runtimesaveData;
        private List<RoguelikeStaticData> staticData;

        private RoguelikeStage roguelikeStage;

        public void LinkToPlatform()
        {
            testPlatform = GameObject.FindFirstObjectByType<Test_Flatform>();
            platform = GameObject.FindFirstObjectByType<RoguelikeStage>();
            roguelikeStage = GameObject.FindFirstObjectByType<RoguelikeStage>();

            if (platform)
            {
                //await waitUntilPlatformDataReady();
                runtimesaveData = (RoguelikeRuntimeData)platform.RuntimeData;
                staticData = platform.DataList;
            }


            if (testPlatform == null)
            {
                UnityEngine.Debug.LogError("Fail to Licnk Platform");
                return;
            }

            else
            {
                if (!Synchronizing())
                {
                    UnityEngine.Debug.LogError("Fail to Sync Platform data");
                }
            }

        }

        bool Synchronizing()
        {
            chapterDatas = testPlatform.chapterDatas;
            characterDatas = testPlatform.characterDatas;

            //LastSeletedChapter = testPlatform.LastSeletedChapter;
            //LastSeletedCharacter = testPlatform.LastSeletedCharacter;

            LastSeletedChapter = runtimesaveData.lastSelectedCharacter;
            LastSeletedCharacter = runtimesaveData.lastPlayedChapter;

            gainableBuffDatas = testPlatform.buffDatas;

            //inventoryItmeDatas = testPlatform.inventoryItmeDatas;

            //spawnableItemDatas = testPlatform.spawnableItemDatas;

            if ((chapterDatas == null || chapterDatas.Length == 0) ||
                (characterDatas == null || characterDatas.Length == 0))
            {
                return false;
            }

            return true;
        }

        public void UploadSelectionData(ChapterData selectedChapter, RLCharacterData selectedCharacter)
        {
            //testPlatform.UploadSelectionDataToFlatform(selectedChapter, selectedCharacter);
            runtimesaveData.selectedChapter = selectedChapter;
            runtimesaveData.selectedCharacter = selectedCharacter;
        }

        public bool LoadSelectionData()
        {
            //var (SelectedChapter, SelectedCharacter) = testPlatform.GetSelectionData();

            ChapterData SelectedChapter = runtimesaveData.selectedChapter;
            RLCharacterData SelectedCharacter = runtimesaveData.selectedCharacter;

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
            platform.LoadStage(LUP.Define.StageKind.RL, 0);
            //testPlatform.LoadRogueLikeLobbyScene(RoguelikeScene.LobbyScene);
        }

        public void LoadGameScene()
        {
            platform.LoadStage(LUP.Define.StageKind.RL, 1);
            //testPlatform.LoadRogueLikeGameScene(RoguelikeScene.GameScene);

        }

        public void ApplyGameResult(Dictionary<ItemData, int> gainItem, ChapterData resultCapter, RLCharacterData resultCharacter, bool stageCleared = true)
        {
            int chapterIndex = -1;
            int characterIndex = -1;

            for (int i = 0; i < chapterDatas.Length; i++)
            {
                if (chapterDatas[i] == resultCapter)
                {
                    chapterIndex = i;
                    break;
                }

            }

            for (int i = 0; i < characterDatas.Length; i++)
            {
                if (characterDatas[i] == resultCharacter)
                {
                    characterIndex = i;
                    break;
                }

            }

            if (chapterIndex == -1 || characterIndex == -1)
            {
                UnityEngine.Debug.LogError("Fail To Apply GameResult");
                return;
            }

            if (stageCleared)
            {
                if (chapterIndex < chapterDatas.Length - 1)
                    chapterIndex++;
            }


            UploadGameResult(gainItem, chapterIndex, characterIndex);
            //testPlatform.UploadGameResult(gainItem, chapterIndex, characterIndex);

            LoadLobbyScene();
            //testPlatform.LoadRogueLikeLobbyScene(RoguelikeScene.LobbyScene);
        }

        void UploadGameResult(Dictionary<ItemData, int> gainItem, int chapterIndex, int characterIndex)
        {
            runtimesaveData.lastSelectedCharacter = chapterIndex;
            runtimesaveData.lastPlayedChapter = characterIndex;

            foreach (KeyValuePair<ItemData, int> item in gainItem)
            {
                ItemData itemData = item.Key;
                int gainNum = item.Value;

                IItemable RLStageItem = ItemManager.Instance.GetItem(itemData.GetDisplayableName());
                roguelikeStage.inventory.AddItem(RLStageItem, gainNum);
            }

        }

        public ItemData[] GetInventoryItems()
        {
            List<InventorySlot> inventorySlots = roguelikeStage.inventory.GetAllItems();
            ItemData[] RLInventory = new ItemData[inventorySlots.Count];

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                IItemable item = inventorySlots[i].Item;

                int itemAmount = inventorySlots[i].Quantity;
                string itemName = item.ItemName;
                Sprite itemIcon = item.Icon;

                ItemData dynamicInventoryItemData = ScriptableObject.CreateInstance<ItemData>();

                dynamicInventoryItemData.SetDisplayableImage(itemIcon);
                dynamicInventoryItemData.SetExtraInfo(itemAmount);
                dynamicInventoryItemData.itemType = item.Type;

                RLInventory[i] = dynamicInventoryItemData;

                //for (int count = 0; count < spawnableItemDatas.Length; count++)
                //{
                //    if (spawnableItemDatas[count].GetDisplayableName() == itemName)
                //    {
                //        dynamicInventoryItemData.SetDisplayableImage(spawnableItemDatas[count].GetDisplayableImage());
                //        dynamicInventoryItemData.SetExtraInfo(itemAmount);
                //        dynamicInventoryItemData.itemType = item.Type;
                //        break;
                //    }
                //}

            }

            return RLInventory;
        }

        //MonoBehavior를 부착할 수 없어서, 코루틴 사용 불가(이건 오브젝트 없이 New로써 사용할 거라서 절대 절대 Mono 부착 금지)
        //void waitUntilPlatformDataReady()
        //{
        //    while (platform.RuntimeData == null)
        //        yield return new WaitForSeconds(0.1f);

        //    runtimesaveData = (RoguelikeRuntimeData)platform.RuntimeData;
        //}

        public async Task waitUntilPlatformDataReady()
        {
            while (platform == null || platform.RuntimeData == null)
            {
                await Task.Delay(100);
            }

            runtimesaveData = (RoguelikeRuntimeData)platform.RuntimeData;
        }
    }
}

