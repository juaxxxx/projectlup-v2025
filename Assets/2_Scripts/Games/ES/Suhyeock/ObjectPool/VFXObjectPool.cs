using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    [System.Serializable]
    public struct VFXPoolSetting
    {
        public GameObject vfxPrefab;
        public int initialCount;
    }

    public class VFXObjectPool : MonoBehaviour
    {
        [Header("Pre-warm Settings")]
        public List<VFXPoolSetting> prewarmSettings = new List<VFXPoolSetting>();

        private Dictionary<GameObject, Queue<GameObject>> poolDict = new Dictionary<GameObject, Queue<GameObject>>();

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            foreach (var setting in prewarmSettings)
            {
                if (setting.vfxPrefab == null || setting.initialCount <= 0) continue;

                if (!poolDict.ContainsKey(setting.vfxPrefab))
                {
                    poolDict[setting.vfxPrefab] = new Queue<GameObject>();
                }

                for (int i = 0; i < setting.initialCount; i++)
                {
                    GameObject vfx = Instantiate(setting.vfxPrefab, transform);
                    vfx.SetActive(false);
                    poolDict[setting.vfxPrefab].Enqueue(vfx);
                }
            }
        }

        public GameObject SpawnVFX(GameObject prefab, Vector3 position, bool bLoop = false, float duration = 1.0f)
        {
            if (prefab == null) return null;

            if (!poolDict.ContainsKey(prefab))
            {
                poolDict[prefab] = new Queue<GameObject>();
            }

            GameObject vfx = null;

            if (poolDict[prefab].Count > 0)
            {
                vfx = poolDict[prefab].Dequeue();
            }
            else
            {
                vfx = Instantiate(prefab, transform);
            }

            vfx.transform.position = position;
            vfx.SetActive(true);

            ParticleSystem mainPS = vfx.GetComponent<ParticleSystem>();
            if (mainPS != null)
            {
                var main = mainPS.main;
                main.loop = bLoop;

                mainPS.Play(true); // true: 자식 파티클까지 모두 재생

                if (!bLoop)
                {
                    StartCoroutine(ReturnRoutine(prefab, vfx, mainPS));
                }

            }
            else
            {
                if (!bLoop)
                {
                    StartCoroutine(ReturnRoutine(prefab, vfx, null));
                }
            }
            return vfx;
        }

        public void DespawnVFX(GameObject prefabKey, GameObject vfx)
        {
            if (vfx == null || prefabKey == null) return;

            ParticleSystem mainPS = vfx.GetComponent<ParticleSystem>();
            if (mainPS != null)
            {
                mainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting); // 파티클 부드럽게 정지
            }

            vfx.SetActive(false);

            if (poolDict.ContainsKey(prefabKey))
            {
                poolDict[prefabKey].Enqueue(vfx);
            }
        }

        private IEnumerator ReturnRoutine(GameObject prefabKey, GameObject vfx, ParticleSystem ps)
        {
            if (ps != null)
            {
                yield return new WaitWhile(() => ps.IsAlive(true));
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }

            vfx.SetActive(false);
            poolDict[prefabKey].Enqueue(vfx);
        }


    }
    
}
