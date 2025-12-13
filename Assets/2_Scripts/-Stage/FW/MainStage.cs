using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP
{
    public class MainStage : BaseStage
    {
        public AudioSource SFX;
        public AudioSource BGM;
        public float soundVolume= 0;
        public Slider slider;
        [SerializeField]
        private VersionsData versionsdata;
        [SerializeField]
        private AssetBundle AB;
        [SerializeField]
        private CurrentQuestListData currentQuestListData;

        // Inventory inven;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.Main;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            slider.onValueChanged.AddListener(SetAudioVolume);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                versionsdata.SaveData();
                currentQuestListData.SaveData();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                versionsdata.assetbundlehash = "1231";
                QuestManager.Instance.SaveActiveQuests();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                QuestManager.Instance.Trigger(123, 1);
            }
        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

            //// Inventory 생성 및 파일명 설정
            //string inventoryFilename = "inventory.json";

            //if (JsonDataHelper.FileExists(inventoryFilename))
            //{
            //    // 기존 인벤토리 로드
            //    inven = JsonDataHelper.LoadData<Inventory>(inventoryFilename);
            //    if (inven != null)
            //    {
            //        inven.filename = inventoryFilename;
            //        inven.InitializeFromJson();  // Dictionary 복원
            //        Debug.Log("[MainStage] 인벤토리 로드 완료");
            //    }
            //    else
            //    {
            //        Debug.LogWarning("[MainStage] 인벤토리 로드 실패, 새로 생성");
            //        inven = new Inventory();
            //        inven.filename = inventoryFilename;
            //    }
            //}
            //else
            //{
            //    // 새 인벤토리 생성
            //    inven = new Inventory();
            //    inven.filename = inventoryFilename;
            //    Debug.Log("[MainStage] 새 인벤토리 생성");
            //}

            yield return null;
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
            //AB = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("Resources/AssetBundles", "staticdatas")));
            //versionsdata.assetbundlehash = AB.GetHashCode().ToString();
        }

        protected override void GetDatas()
        {
            List<BaseRuntimeData> runtimeDatas = base.GetRuntimeData(this, 1);

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is VersionsData versionData)
                    {
                        versionsdata = versionData;
                    }
                }
            }

            runtimeDatas = base.GetRuntimeData(this, 2);
            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is CurrentQuestListData versionData)
                    {
                        currentQuestListData = versionData;
                    }
                }
            }
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (versionsdata != null)
            {
                runtimeDataList.Add(versionsdata);
            }

            base.SaveRuntimeDataList(runtimeDataList);
        }


        void SetAudioVolume(float value)
        {
            Debug.LogFormat("SoundVolume : {0}", value);

            LUP.SoundManager.Instance.SetBGMVolume(slider.value);
            LUP.SoundManager.Instance.SetSFXVolume(slider.value);
        }

        public void PlaySFX()
        {
            LUP.SoundManager.Instance.PlaySFX(Define.SoundSFXResourceType.Sample);
        }
        public void PlayBGM()
        {
            LUP.SoundManager.Instance.PlayBGM(Define.SoundBGMResourceType.Sample);
        }
        public void StopBGM()
        {
            LUP.SoundManager.Instance.StopBGM();
        }
    }
}

