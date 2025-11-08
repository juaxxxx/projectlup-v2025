using Manager;
using OpenCvSharp;
using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;
namespace Manager
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
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                versionsdata.assetbundlehash = "1231";
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("versionsdata.assetbundlehash : " + versionsdata.assetbundlehash);
            }
        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();
            
            //구현부


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
            versionsdata = (VersionsData)Manager.DataManager.Instance.GetRuntimeData(Define.StageKind.Main,1);
            //AB = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("Resources/AssetBundles", "staticdatas")));
            //versionsdata.assetbundlehash = AB.GetHashCode().ToString();
        }

        protected override void GetDatas()
        {
            
        }

        protected override void SaveDatas()
        {

        }

        void SetAudioVolume(float value)
        {
            Debug.LogFormat("SoundVolume : {0}", value);

            Manager.SoundManager.Instance.SetBGMVolume(slider.value);
            Manager.SoundManager.Instance.SetSFXVolume(slider.value);
        }

        public void PlaySFX()
        {
            Manager.SoundManager.Instance.PlaySFX(Define.SoundSFXResourceType.Sample);
        }
        public void PlayBGM()
        {
            Manager.SoundManager.Instance.PlayBGM(Define.SoundBGMResourceType.Sample);
        }
        public void StopBGM()
        {
            Manager.SoundManager.Instance.StopBGM();
        }

        protected override void SetupInventory()
        {

        }

    }
}

