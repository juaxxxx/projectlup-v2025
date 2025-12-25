using LUP;
using LUP.ES;
using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

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

        public List<CharacterEquipsID> characterEquipInfos = null;

        private Dictionary<int, EquipData> RLEquipDictionary;

        public void LinkToPlatform()
        {
            testPlatform = GameObject.FindFirstObjectByType<Test_Flatform>();
            platform = GameObject.FindFirstObjectByType<RoguelikeStage>();
            roguelikeStage = GameObject.FindFirstObjectByType<RoguelikeStage>();
            RLEquipDictionary = new Dictionary<int, EquipData>();

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

            MakeEquipTable();

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
            List<EquipData> Equips = new List<EquipData>();

            for(int i = 0; i < inventorySlots.Count;i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.Item.Type != Define.ItemType.Weapon && slot.Item.Type != Define.ItemType.Armor)
                    continue;

                IItemable item = inventorySlots[i].Item;

                EquipData dynamicInventoryEquipData = ConvertItemToEquip(item);
                Equips.Add(dynamicInventoryEquipData);
            }

            if(Equips.Count == 0)
                return null;

            return Equips.ToArray();
        }

        //public EquipData[] GetCharacterEquips(RLCharacterData targetCharacterData)
        //{
        //    EquipsID EquipItems = targetCharacterData.EquipItems;
        //    List<EquipData> Equips = new List<EquipData>();
        //}

        public void AddItemToInventory(string itemName, int gainNum)
        {
            IItemable item = ItemManager.Instance.GetItem(itemName);
            //int id = item.ItemID;

            roguelikeStage.inventory.AddItem(item, gainNum);
        }

        public void RemoveItemFromInventory(string itemName, int removeNum)
        {
            IItemable item = ItemManager.Instance.GetItem(itemName);
            int id = item.ItemID;

            roguelikeStage.inventory.RemoveItem(id, removeNum);
        }

        public async Task waitUntilPlatformDataReady()
        {
            while (platform == null || platform.RuntimeData == null)
            {
                await Task.Delay(100);
            }

            runtimesaveData = (RoguelikeRuntimeData)platform.RuntimeData;
        }

        EquipStat[] ExtrackEquipEffect(string effectText)
        {
            if (string.IsNullOrEmpty(effectText))
                return Array.Empty<EquipStat>();

            string[] statTokens = effectText.Split('/');

            List<EquipStat> stats = new List<EquipStat>();

            foreach (string token in statTokens)
            {
                // "ATK:5"
                string[] pair = token.Split(':');
                if (pair.Length != 2)
                    continue;

                string statName = pair[0];

                if (!int.TryParse(pair[1], out int value))
                    continue;

                stats.Add(new EquipStat
                {
                    statName = statName,
                    value = value
                });
            }

            return stats.ToArray();
        }

        //public void LoadCharacterEquips()
        //{
        //    characterEquipInfos = new List<CharacterEquipsID>();

        //    characterEquipInfos.Add(runtimesaveData.F001Equips);
        //    characterEquipInfos.Add(runtimesaveData.F002Equips);
        //    characterEquipInfos.Add(runtimesaveData.F003Equips);
        //    characterEquipInfos.Add(runtimesaveData.M001Equips);
        //    characterEquipInfos.Add(runtimesaveData.M002Equips);
        //}

        //public void UpLoadCharacterEquips()
        //{
        //    runtimesaveData.F001Equips = characterEquipInfos[(int)CharacterType.F001];
        //    runtimesaveData.F002Equips = characterEquipInfos[(int)CharacterType.F002];
        //    runtimesaveData.F003Equips = characterEquipInfos[(int)CharacterType.F003];
        //    runtimesaveData.M001Equips = characterEquipInfos[(int)CharacterType.M001];
        //    runtimesaveData.M002Equips = characterEquipInfos[(int)CharacterType.M002];
        //}

        void MakeEquipTable()
        {
            foreach (RLItemID itemID in Enum.GetValues(typeof(RLItemID)))
            {
                if (itemID == RLItemID.Max)
                    continue;

                int numID = (int)itemID;

                IItemable item = ItemManager.Instance.GetItem(numID);

                if (item.Type != Define.ItemType.Weapon && item.Type != Define.ItemType.Armor)
                    continue;

                EquipData dynamicInventoryEquipData = ConvertItemToEquip(item);

                if(RLEquipDictionary.ContainsKey(numID) == false)
                    RLEquipDictionary[numID] = dynamicInventoryEquipData;
            }
           
        }

        public EquipData GetEquipDataByID(int id)
        {
            if(RLEquipDictionary.ContainsKey(id))
                return RLEquipDictionary[id];

            return null;
        }

        EquipData ConvertItemToEquip(IItemable item)
        {
            string equipName = item.ItemName;
            Sprite equipIcon = item.Icon;
            int equipTier = item.GetInt("Tier");
            RLEquipPos equipPos = (RLEquipPos)item.GetInt("EquipPos");
            RWeaponType weaponType = (RWeaponType)item.GetInt("WeaponType");
            string equipDescription = item.Description;
            string equipEffects = item.GetString("Effects");

            EquipData dynamicInventoryEquipData = ScriptableObject.CreateInstance<EquipData>();

            dynamicInventoryEquipData.SetItemName(equipName);
            dynamicInventoryEquipData.SetDisplayableImage(equipIcon);
            dynamicInventoryEquipData.SetExtraInfo(equipTier);
            dynamicInventoryEquipData.equipPos = equipPos;
            dynamicInventoryEquipData.weaponType = weaponType;
            dynamicInventoryEquipData.equipStats = ExtrackEquipEffect(equipEffects);
            dynamicInventoryEquipData.equipDescription = equipDescription;

            return dynamicInventoryEquipData;
        }

    }
}

