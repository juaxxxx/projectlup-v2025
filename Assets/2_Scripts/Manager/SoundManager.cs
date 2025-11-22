using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Audio Source Prefabs")]
        public AudioSource bgmSource;             // BGM 전용
        public AudioSource sfxPrefab;             // SFX용 AudioSource 프리팹

        [Header("Audio Volume")]
        public float currentBGMVolume=1;
        public float currentSFXVolume=1;

        [Header("SFX Settings")]
        public int maxSameSFXCount = 10;           // 같은 소리 최대 동시 재생 수
        private Dictionary<string, List<AudioSource>> activeSFX = new(); //재생중인 효과음 목록

        Vector3 zeroVector = Vector3.zero;
        public void PlayBGM(Define.SoundBGMResourceType bgmtype, bool loop = true)
        {
            AudioClip clip = ResourceManager.Instance.LoadAudioBGM(bgmtype);
            if (clip == null)
            {
                Debug.LogWarning($"[SoundManager] BGM not found: {name}");
                return;
            }

            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.volume = currentBGMVolume;
            bgmSource.Play();
        }
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        public void PlaySFX(Define.SoundSFXResourceType sfxtype, GameObject gameobject = null, bool spatial = true)
        {
            AudioClip clip = ResourceManager.Instance.LoadAudioSFX(sfxtype);
            if (clip == null)
            {
                Debug.LogWarning($"[SoundManager] SFX not found: {name}");
                return;
            }

            if (!activeSFX.ContainsKey(name))
                activeSFX[name] = new List<AudioSource>();

            List<AudioSource> list = activeSFX[name];
            list.RemoveAll(a => a == null || !a.isPlaying);

            if (list.Count >= maxSameSFXCount)
                return; // 너무 많으면 재생 안 함
            AudioSource newSFX;

            if (gameobject == null)
            {
                newSFX = Instantiate(sfxPrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                newSFX = Instantiate(sfxPrefab, gameobject.transform.position, Quaternion.identity);
            }
            newSFX.clip = clip;
            newSFX.volume = currentSFXVolume;
            newSFX.spatialBlend = spatial ? 1f : 0f; // 3D/2D 전환
            newSFX.Play();

            list.Add(newSFX);

            // 자동 파괴
            Destroy(newSFX.gameObject, clip.length + 0.1f);
        }

        public void SetBGMVolume(float volume)
        {
            currentBGMVolume = volume;
            bgmSource.volume = currentBGMVolume;
        }

        public void SetSFXVolume(float volume)
        {
            currentSFXVolume = volume;

            foreach (KeyValuePair<string, List<AudioSource>> pair in activeSFX)
            {
                foreach (AudioSource src in pair.Value)
                {
                    if (src != null)
                        src.volume = currentSFXVolume;
                }
            }
        }
    }
}
