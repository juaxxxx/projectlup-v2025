using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace LUP
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [SerializeField, ReadOnly]
        private List<AssetBundle> assetbundles = new List<AssetBundle>();

        //만약 게임 단위로 애셋번들을 나눌 수 있다면 딕셔너리로
        //private Dictionary<Define.StageKind,AssetBundle> assetbundles;

        [SerializeField, ReadOnly]
        private AssetBundle AB_Video;
        [SerializeField, ReadOnly]
        private AssetBundle AB_Audio;
        [SerializeField, ReadOnly]
        private AssetBundle AB_Image;
        [SerializeField, ReadOnly]
        private AssetBundle AB_VFX;
        [SerializeField, ReadOnly]
        private AssetBundle AB_GUI;
        [SerializeField, ReadOnly]
        private AssetBundle AB_Model;
        [SerializeField, ReadOnly]
        private AssetBundle AB_Shader;
        [SerializeField, ReadOnly]
        private AssetBundle AB_Data;

        private static Dictionary<string, Object> _cache = new();
        private static T LoadResource<T>(string path) where T : Object
        {
            if (_cache.ContainsKey(path)) return _cache[path] as T;
            var obj = Resources.Load<T>(path);
            if (obj != null) _cache[path] = obj;
            return obj;
        }

        private void Awake()
        {
            base.Awake();
            if (Instance.assetbundles.Count==0)
            {
                LoadAssetBundles();
            }
            
        }

        public T LoadVideoClip<T>(string name) where T : Object
        {
            T videoClip = AB_Video.LoadAsset<T>(name);

            return videoClip;
        }

        public T LoadAudioBGM<T>(string name) where T : Object
        {
            T audioClip = AB_Audio.LoadAsset<T>(name);

            return audioClip;
        }

        public T LoadAudioSFX<T>(string name) where T : Object
        {
            T audio = AB_Audio.LoadAsset<T>(name);

            return audio;
        }

        public List<BaseStaticDataLoader> LoadStaticData(Define.StageKind type, int stagetype)
        {
            List<BaseStaticDataLoader> dataList = new List<BaseStaticDataLoader>();
            string folderPath = "";

            switch (type)
            {
                case Define.StageKind.ST:
                    folderPath = "Data/Games/ST";
                    break;
                case Define.StageKind.DSG:
                    folderPath = "Data/Games/DSG";
                    break;
                case Define.StageKind.ES:
                    folderPath = "Data/Games/ES";
                    break;
                case Define.StageKind.RL:
                    folderPath = "Data/Games/RL";
                    break;
                case Define.StageKind.PCR:
                    folderPath = "Data/Games/PCR";
                    break;
                case Define.StageKind.Tutorial:
                    folderPath = "Data/Games/FW";
                    break;
            }

            if (!string.IsNullOrEmpty(folderPath))
            {
                BaseStaticDataLoader[] loadedData = Resources.LoadAll<BaseStaticDataLoader>(folderPath);
                dataList.AddRange(loadedData);
            }

            return dataList;
        }

        public void LoadAssetBundles()
        {
            {
                AB_Video = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "videos")));
                assetbundles.Add(AB_Video);
            }
            {
                AB_Audio = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "audio")));
                assetbundles.Add(AB_Audio);
            }
            {
                AB_Image = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "image")));
                assetbundles.Add(AB_Image);
            }
            {
                AB_VFX = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "vfx")));
                assetbundles.Add(AB_VFX);
            }
            {
                AB_GUI = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "gui")));
                assetbundles.Add(AB_GUI);
            }
            {
                AB_Model = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "models")));
                assetbundles.Add(AB_Model);
            }
            {
                AB_Shader = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, Path.Combine("LUP/assetbundles", "shaders")));
                assetbundles.Add(AB_Shader);
            }
            {
                AB_Data = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath,Path.Combine("LUP/assetbundles", "data")));
                assetbundles.Add(AB_Data);
            }
        }

        public int GetAssetBundleSize()
        {
            return assetbundles.Count;
        }

        public AssetBundle GetAssetBundle(Define.AssetBundleKind assetBundleKind)
        {
            switch (assetBundleKind)
            {
                case Define.AssetBundleKind.Video:
                    return AB_Video;
                case Define.AssetBundleKind.Audio:
                    return AB_Audio;
                case Define.AssetBundleKind.Image:
                    return AB_Image;
                case Define.AssetBundleKind.VFX:
                    return AB_VFX;
                case Define.AssetBundleKind.GUI:
                    return AB_GUI;
                case Define.AssetBundleKind.Model:
                    return AB_Model;
                case Define.AssetBundleKind.Shader:
                    return AB_Shader;
                case Define.AssetBundleKind.Data:
                    return AB_Data;
            }
            return null;
        }
    }
}

