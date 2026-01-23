using LUP.DSG.Utils.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public static class StageInitializeInvoker
    {
        // base.OnStageEnter() 이후 초기화 지점
        public static event Action<DeckStrategyStage> OnDSGStageInitialize;
        public static event Action<DeckStrategyStage> OnDSGStagePostInitialize;

        public static void Invoke(DeckStrategyStage stage)
        {
            OnDSGStageInitialize?.Invoke(stage);
            OnDSGStagePostInitialize?.Invoke(stage);
        }
    }

    public class DeckStrategyStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public BaseRuntimeData enemyStageRuntimeData;

        public List<DeckStaticData> DeckDataList;
        public List<DeckCharacterStaticData> CharacterDataList;
        public CharacterModelDataTable characterModelDataTable;
        public TeamMVPData mvpData;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.DSG;

        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)RuntimeData;
            if (runtimeData.OwnedCharacterList == null || runtimeData.OwnedCharacterList.Count <= 0)
            {
                OwnedCharacterTable testCharacterTable
                    = Resources.Load<OwnedCharacterTable>("Data/Games/DSG/ScriptableObjects/OwnedCharacter/OwnedCharacterListTable");
                if (testCharacterTable != null)
                {
                    runtimeData.OwnedCharacterList = testCharacterTable.ownedCharacterList;
                }
            }

            DSGEnemyStageRuntimeData enemyRuntimeData = (DSGEnemyStageRuntimeData)enemyStageRuntimeData;
            if (enemyRuntimeData.SelectedEnemyStage == null)
            {
                EnemyStageData enemyStageData
                    = Resources.Load<EnemyStageData>("Data/Games/DSG/ScriptableObjects/EnemyStageData1");
                if (enemyStageData != null)
                {
                    enemyRuntimeData.SelectedEnemyStage = enemyStageData;
                    SaveDatas();
                }
            }

            // PCR 인벤토리 접근 가능 여부 확인
            if (InventoryManager.Instance.HasInventory("PCR"))
                Debug.Log("[DSGStage] PCR 인벤토리 접근 가능");
            else
                Debug.LogWarning("[DSGStage] PCR 인벤토리가 아직 로드되지 않았습니다!");

            StageInitializeInvoker.Invoke(this);

            yield return null;
        }

        // PCR 팀의 공유 인벤토리 가져오기
        public Inventory GetSharedInventory()
        {
            return InventoryManager.Instance.GetInventory("PCR");
        }

        public override IEnumerator OnStageStay()
        {
            yield return base.OnStageStay();
            //일단 납두기
            yield return null;
        }
        public override IEnumerator OnStageExit()
        {
            yield return base.OnStageExit();
            //구현부


            yield return null;
        }
        protected override void LoadResources()
        {
            //resource = ResourceManager.Instance.Load...
        }

        protected override void GetDatas()
        {
            List<BaseStaticDataLoader> loaders = base.GetStaticData(this, 1);
            List<BaseRuntimeData> runtimeDatas = base.GetRuntimeData(this, 1);
            List<BaseRuntimeData> enemyRuntimeDatas = base.GetRuntimeData(this, 2);

            // 일단 타입별로 가져오는 예시
            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is DeckStaticDataLoader deckLoader)
                    {
                        DeckDataList = deckLoader.GetDataList();
                    }
                    else if (loader is DeckCharacterStaticDataLoader charLoader)
                    {
                        CharacterDataList = charLoader.GetDataList();
                    }
                }
            }

            // 일단 타입별로 가져오는 예시
            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is DeckStrategyRuntimeData deckRuntimeData)
                    {
                        RuntimeData = deckRuntimeData;
                    }                    
                }
            }

            if (enemyRuntimeDatas != null && enemyRuntimeDatas.Count > 0)
            {
                foreach (var runtimeData in enemyRuntimeDatas)
                {
                    if (runtimeData is DSGEnemyStageRuntimeData enemyRuntimeData)
                    {
                        enemyStageRuntimeData = enemyRuntimeData;
                    }
                }
            }

            if (RuntimeData != null)
            {
                DeckStrategyRuntimeData deckStrategyRuntimeData = (DeckStrategyRuntimeData)RuntimeData;
                if (deckStrategyRuntimeData != null && deckStrategyRuntimeData.Teams.Count == 0)
                {
                    deckStrategyRuntimeData.Teams.AddRange(new LUP.DSG.Team[6]);
                }
            }
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (RuntimeData != null)
            {
                runtimeDataList.Add(RuntimeData);
            }

            if (enemyStageRuntimeData != null)
            {
                runtimeDataList.Add(enemyStageRuntimeData);
            }

            base.SaveRuntimeDataList(runtimeDataList);
        }

        public CharacterModelData FindCharacterModel(int modelId)
        {
            if (characterModelDataTable == null || characterModelDataTable.characterModelDataList == null)
            {
                Debug.LogError("[DeckStrategyStage] characterModelDataTable 이 비어있습니다.");
                return null;
            }

            foreach (var data in characterModelDataTable.characterModelDataList)
            {
                if (data.ID == modelId)
                    return data;
            }

            return null;
        }

        public GameObject GetCharacterPrefab(int modelId)
        {
            var modelData = FindCharacterModel(modelId);
            if (modelData == null)
            {
                Debug.LogError($"[DeckStrategyStage] modelId {modelId} 에 해당하는 CharacterModelData 를 찾지 못했습니다.");
                return null;
            }

            if (modelData.prefab == null)
            {
                Debug.LogError($"[DeckStrategyStage] modelId {modelId} 의 prefab 이 비어 있습니다. (CharacterModelDataTable 체크)");
                return null;
            }

            //Debug.Log($"[DeckStrategyStage] GetCharacterPrefab({modelId}) → {modelData.prefab.name}");
            return modelData.prefab;
        }
        public CharacterData FindCharacterData(int id, int level)
        {
            foreach (DeckCharacterStaticData data in CharacterDataList)
            {
                if (data.CharacterId == id)
                {
                    DeckStrategyRuntimeData deckStrategyRuntimeData = (DeckStrategyRuntimeData)RuntimeData;
                    if (deckStrategyRuntimeData == null || deckStrategyRuntimeData.OwnedCharacterList.Count == 0) return null;

                    int statusId = id * 100 + level;

                    foreach (DeckStaticData statusData in DeckDataList)
                    {
                        if (statusData.tableId == statusId)
                        {
                            CharacterData characterData = new CharacterData();
                            characterData.ID = id;
                            characterData.characterName = data.CharacterName;
                            characterData.type = (EAttributeType)data.AttributeType;
                            characterData.rangeType = (ERangeType)data.RangeType;
                            characterData.maxHp = statusData.hp;
                            characterData.attack = statusData.attack;
                            characterData.defense = statusData.defense;
                            characterData.speed = statusData.speed;

                            return characterData;
                        }
                    }
                }
            }

            return null;
        }

        public EnemyStageData GetEnemyStage()
        {
            DSGEnemyStageRuntimeData enemyRuntimeData = (DSGEnemyStageRuntimeData)enemyStageRuntimeData;
            if (enemyRuntimeData == null) return null;

            return enemyRuntimeData.SelectedEnemyStage;
        }

        public void SetEnemyStage(EnemyStageData enemyStageData)
        {
            DSGEnemyStageRuntimeData enemyRuntimeData = (DSGEnemyStageRuntimeData)enemyStageRuntimeData;
            if (enemyRuntimeData == null) return;
            enemyRuntimeData.SelectedEnemyStage = enemyStageData;
            SaveDatas();
        }

        public void ChangeScene(int sceneIndex)
        {
            //0: edit
            //1: battle
            //2: result

            LoadStage(StageKind, sceneIndex);

            //BGMListNum num = (BGMListNum)sceneIndex; //@TODO SoundManager.BGM
            //string bgm;

            //switch (num)
            //{
            //    case BGMListNum.EditSceneBGM:
            //        {
            //            bgm = "RPG Combat 1 - Duel of the Fates (Loopable)";
            //            break;
            //        }
            //    case BGMListNum.BattleSceneBGM:
            //        {
            //            bgm = "RPG Combat 1 - Watch out! (Loopable)";
            //            break;
            //        }
            //    case BGMListNum.ResultSceneBGM:
            //        {
            //            bgm = "";
            //            break;
            //        }
            //    default:
            //        return;
            //}

            //SoundManager.Instance.PlayBGM(bgm, true);
        }

        public void BattleEnd()
        {
            BattleSystem battleSystem = GetComponent<BattleSystem>();

            if (battleSystem != null)
            {
                ChangeScene(2);
            }
        }

        public BattleSystem GetBattleSystem() 
        {
            return GetComponent<BattleSystem>();
        }
    }
}

