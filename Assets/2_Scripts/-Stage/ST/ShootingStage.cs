using LUP.ST;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ShootingStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<ShootingStaticData> DataList;


        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.ST;

        }

        void Start()
        {

        }

        void Update()
        {

        }
        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

            var srd = STDataManage.Instance?.RuntimeData;

            if (srd == null)
            {
                yield break;
            }

            if (srd.Team == null || srd.Team.Length < 5)
            {
                yield break;
            }

            var spawner = FindFirstObjectByType<LUP.ST.STTeamSpawner>();
            if (spawner == null)
            {
                yield break;
            }

            var spawnedCharacters = spawner.Spawn(srd);

            var skillSys = FindFirstObjectByType<LUP.ST.TeamSharedSkillSystem>();
            skillSys?.SetTeamCharacters(spawnedCharacters);

            var ui = FindFirstObjectByType<LUP.ST.UIGameController>();
            if (ui != null)
            {
                // 순서 변경!
                ui.BindHpToSpawnedCharacters(spawnedCharacters);  // 1. 먼저 리스트 저장
                ui.ApplyTeamThumbnails(srd.Team);                  // 2. 썸네일 적용
                ui.RebuildAfterSpawn();                            // 3. 마지막에 리빌드
            }

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

            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is ShootingStaticDataLoader stLoader)
                    {
                        DataList = stLoader.GetDataList();
                    }
                }
            }

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is ShootingRuntimeData stRuntimeData)
                    {
                        RuntimeData = stRuntimeData;
                    }
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

            base.SaveRuntimeDataList(runtimeDataList);
        }

    }
}

