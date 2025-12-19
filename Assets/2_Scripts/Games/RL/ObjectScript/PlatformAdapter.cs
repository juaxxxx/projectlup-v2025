using LUP;
using LUP.ES;
using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LUP.RL
{
    public class PlatformAdapter
    {
        private Test_Flatform testPlatform;

        private RoguelikeStage platform;

        public ChapterData[] chapterDatas { get; private set; }
        public RLCharacterData[] characterDatas { get; private set; }

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

            LastSeletedChapter = runtimesaveData.lastSelectedCharacter;
            LastSeletedCharacter = runtimesaveData.lastPlayedChapter;

            gainableBuffDatas = testPlatform.buffDatas;

            if ((chapterDatas == null || chapterDatas.Length == 0) ||
                (characterDatas == null || characterDatas.Length == 0))
            {
                return false;
            }

            return true;
        }

        public void UploadSelectionData(ChapterData selectedChapter, RLCharacterData selectedCharacter)
        {
            runtimesaveData.selectedChapter = selectedChapter;
            runtimesaveData.selectedCharacter = selectedCharacter;
        }

        public bool LoadSelectionData()
        {
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

        public int GetItemAmountInInventory(RLItemID item)
        {
            return roguelikeStage.inventory.GetItemCount((int)item);
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
            }

            return RLInventory;
        }

        public EquipData[] GetInventoryEquips()
        {
            List<InventorySlot> inventorySlots = roguelikeStage.inventory.GetAllItems();

            for(int i = 0; i < inventorySlots.Count;i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.Item.Type != Define.ItemType.Weapon || slot.Item.Type != Define.ItemType.Armor)
                    continue;


            }

            return new EquipData[inventorySlots.Count];
        }

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

