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

        public Inventory inven;

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
                QuestManager.Instance.SaveActiveQuests();
                CheckVersions();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                QuestManager.Instance.Trigger(123, 1);
            }
        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

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
            LUP.SoundManager.Instance.PlaySFX("SampleSFX");
        }
        public void PlayBGM()
        {
            LUP.SoundManager.Instance.PlayBGM("SampleBGM");
        }
        public void StopBGM()
        {
            LUP.SoundManager.Instance.StopBGM();
        }

        private void CheckVersions()
        {
            versionsdata.Videohash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Video).GetHashCode().ToString();

            versionsdata.Audiohash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Audio).GetHashCode().ToString();

            versionsdata.Imagehash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Image).GetHashCode().ToString();

            versionsdata.VFXhash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.VFX).GetHashCode().ToString();

            versionsdata.GUIhash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.GUI).GetHashCode().ToString();

            versionsdata.Modelhash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Model).GetHashCode().ToString();

            versionsdata.Shaderhash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Shader).GetHashCode().ToString();

            versionsdata.Datahash = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Data).GetHashCode().ToString();

            versionsdata.SaveData();
        }
    }
}

