using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace LUP
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private static Dictionary<string, Object> _cache = new();
        private static T LoadResource<T>(string path) where T : Object
        {
            if (_cache.ContainsKey(path)) return _cache[path] as T;
            var obj = Resources.Load<T>(path);
            if (obj != null) _cache[path] = obj;
            return obj;
        }

        public VideoClip LoadVideoClip(Define.VideoResourceType type)
        {
            VideoClip videoClip = null;
            //string path = "VideoClip/";
            switch (type) { 
            case Define.VideoResourceType.Sample:
                    //videoClip = LoadResource<VideoClip>(path+"/SampleVideo");
                    videoClip = LoadResource<VideoClip>("VideoClip/SampleVideo");
                    break;
            }
            return videoClip;
        }

        public AudioClip LoadAudioBGM(Define.SoundBGMResourceType type)
        {
            AudioClip audioClip = null;
            //string path = "VideoClip/";
            switch (type)
            {
                case Define.SoundBGMResourceType.Sample:
                    audioClip = LoadResource<AudioClip>("BGM/SampleBGM");
                    break;
            }
            return audioClip;
        }

        public AudioClip LoadAudioSFX(Define.SoundSFXResourceType type)
        {
            AudioClip audioClip = null;
            //string path = "VideoClip/";
            switch (type)
            {
                case Define.SoundSFXResourceType.Sample:
                    audioClip = LoadResource<AudioClip>("SFX/SampleSFX");
                    break;
            }
            return audioClip;
        }

        public List<BaseStaticDataLoader> LoadStaticData(Define.StageKind type, int stagetype)
        {
            List<BaseStaticDataLoader> dataList = new List<BaseStaticDataLoader>();
            string folderPath = "";

            switch (type)
            {
                case Define.StageKind.ST:
                    folderPath = "Data/ST";
                    break;
                case Define.StageKind.DSG:
                    folderPath = "Data/DSG";
                    break;
                case Define.StageKind.ES:
                    folderPath = "Data/ES";
                    break;
                case Define.StageKind.RL:
                    folderPath = "Data/RL";
                    break;
                case Define.StageKind.Main:
                    folderPath = "Data/PCR";
                    break;
                case Define.StageKind.Tutorial:
                    folderPath = "Data/FW";
                    break;
            }

            if (!string.IsNullOrEmpty(folderPath))
            {
                BaseStaticDataLoader[] loadedData = Resources.LoadAll<BaseStaticDataLoader>(folderPath);
                dataList.AddRange(loadedData);
            }

            return dataList;
        }
    }
}

