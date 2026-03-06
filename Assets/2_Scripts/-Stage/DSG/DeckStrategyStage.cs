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
        public CharacterPrefabDataTable characterModelDataTable;
        public TeamMVPData mvpData;

        public DeckStrategyRuntimeData DSGRuntimeData { get; private set; }
        public DSGEnemyStageRuntimeData DSGEnemyRuntimeData { get; private set; }

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

            if(DSGRuntimeData == null)
               DSGRuntimeData = (DeckStrategyRuntimeData)RuntimeData;

            if (DSGEnemyRuntimeData != null && (DSGRuntimeData.OwnedCharacterList == null || DSGRuntimeData.OwnedCharacterList.Count <= 0))
            {
                OwnedCharacterTable testCharacterTable
                    = Resources.Load<OwnedCharacterTable>("Data/Games/DSG/ScriptableObjects/OwnedCharacter/OwnedCharacterListTable");
                if (testCharacterTable != null)
                    DSGRuntimeData.OwnedCharacterList = testCharacterTable.ownedCharacterList;
            }

            if(DSGEnemyRuntimeData == null)
               DSGEnemyRuntimeData = (DSGEnemyStageRuntimeData)enemyStageRuntimeData;

            if (DSGEnemyRuntimeData != null && DSGEnemyRuntimeData.SelectedEnemyStage == null)
            {
                EnemyStageData enemyStageData
                    = Resources.Load<EnemyStageData>("Data/Games/DSG/ScriptableObjects/EnemyStageData1");
                if (enemyStageData != null)
                {
                    DSGEnemyRuntimeData.SelectedEnemyStage = enemyStageData;
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

        public CharacterPrefabData FindCharacterPrefabData(int prefabId)
        {
            if (characterModelDataTable == null || characterModelDataTable.characterModelDataList == null)
                return null;

            foreach (var data in characterModelDataTable.characterModelDataList)
                if (data.ID == prefabId) return data;

            return null;
        }

        public GameObject GetCharacterPrefab(int modelId)
        {
            CharacterPrefabData modelData = FindCharacterPrefabData(modelId);
            if (modelData == null) return null;
            if (modelData.prefab == null) return null;

            return modelData.prefab;
        }
        public CharacterData FindCharacterData(int id, int level)
        {
            foreach (DeckCharacterStaticData data in CharacterDataList)
            {
                if (data.CharacterId == id)
                {
                    if (DSGRuntimeData == null || DSGRuntimeData.OwnedCharacterList.Count == 0) return null;

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
            if (DSGEnemyRuntimeData == null) return null;

            return DSGEnemyRuntimeData.SelectedEnemyStage;
        }

        public void SetEnemyStage(EnemyStageData enemyStageData)
        {
            if (DSGEnemyRuntimeData == null) return;
            DSGEnemyRuntimeData.SelectedEnemyStage = enemyStageData;
            SaveDatas();
        }

        public void ChangeScene(int sceneIndex)
        {
            //0: edit
            //1: battle
            //2: result

            LoadStage(StageKind, sceneIndex);
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

        public Team GetSelectedTeam()
        {
            if (DSGRuntimeData == null) return null;
            if (DSGRuntimeData.Teams[DSGRuntimeData.SelectedTeamIndex] == null)
            {
                return DSGRuntimeData.Teams[DSGRuntimeData.SelectedTeamIndex] = new Team();
            }

            return DSGRuntimeData.Teams[DSGRuntimeData.SelectedTeamIndex];
        }

        public void SetSelectedTeam(int index)
        {
            if (DSGRuntimeData == null) return;
            DSGRuntimeData.SelectedTeamIndex = index;
            SaveDatas();
        }

        public Team ChangeSelectedTeam(int index)
        {
            if (DSGRuntimeData == null) return null;

            DSGRuntimeData.SelectedTeamIndex = index;

            if (DSGRuntimeData.Teams[index] == null)
            {
                return DSGRuntimeData.Teams[index] = new Team();
            }

            return DSGRuntimeData.Teams[DSGRuntimeData.SelectedTeamIndex];
        }

        public OwnedCharacterInfo GetOwnedCharacterById(int id)
        {
            if (DSGRuntimeData == null || DSGRuntimeData.OwnedCharacterList == null) return null;

            List<OwnedCharacterInfo> ownedList = DSGRuntimeData.OwnedCharacterList;
            for(int i = 0; i < ownedList.Count; ++i)
            {
                if (ownedList[i].characterID == id)
                    return ownedList[i];
            }

            return null;
        }

    }
}

