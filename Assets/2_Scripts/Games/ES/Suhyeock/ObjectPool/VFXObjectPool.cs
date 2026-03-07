using OpenCvSharp.Aruco;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class VFXObjectPool : MonoBehaviour
    {
        private Dictionary<GameObject, Queue<GameObject>> poolDict = new Dictionary<GameObject, Queue<GameObject>>();
        
        public void SpawnVFX(GameObject prefab, Vector3 position, float duration = 1.0f)
        {
            if (prefab == null) return;

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

            ParticleSystem[] particles = vfx.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in particles)
            {
                p.Play();
            }

            StartCoroutine(ReturnRoutine(prefab, vfx, duration));
        }
        private IEnumerator ReturnRoutine(GameObject prefabKey, GameObject vfx, float delay)
        {
            yield return new WaitForSeconds(delay);
            vfx.SetActive(false);
            poolDict[prefabKey].Enqueue(vfx);
        }
    }
    
}
