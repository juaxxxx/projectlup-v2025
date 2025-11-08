using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Manager
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

        public BaseStaticDataLoader LoadStaticData(Define.StageKind type, int stagetype)
        {
            BaseStaticDataLoader data = null;
            switch (type)
            {
                case Define.StageKind.ST:
                    data = Resources.Load<BaseStaticDataLoader>("Data/ShootingStaticData");
                    break;
                case Define.StageKind.DSG:
                    data = Resources.Load<BaseStaticDataLoader>("Data/DeckStrategyStaticData");
                    break;
                case Define.StageKind.ES:
                    data = Resources.Load<BaseStaticDataLoader>("Data/ExtractionShooterStaticData");
                    break;
                case Define.StageKind.RL:
                    data = Resources.Load<BaseStaticDataLoader>("Data/RoguelikeStaticData");
                    break;
                case Define.StageKind.Main:
                    data = Resources.Load<BaseStaticDataLoader>("Data/ProductionStaticData");
                    break;
                case Define.StageKind.Tutorial:
                    data = Resources.Load<BaseStaticDataLoader>("Data/TutorialStaticData");
                    break;
            }
            return data;
        }
    }
}

