using LUP.DSG;
using LUP.DSG.Utils.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
public enum ActionEffect
{
    AttackBasic,
    GetHitBasic,
    GetHitBurn,
}

[System.Serializable]
public struct EffectParticlePair
{
    public ActionEffect name;
    public GameObject particlePrefab;
}

public class EffectPool : MonoBehaviour
{
    [SerializeField] private EffectParticlePair[] effectpairs;
    private Dictionary<ActionEffect, Queue<GameObject>> vfxPool = new Dictionary<ActionEffect, Queue<GameObject>>();

    private void Awake()
    {
        foreach(var pair in effectpairs)
        {
            var q = new Queue<GameObject>();
            vfxPool[pair.name] = q;
        }

        StartCoroutine(TryLoading());
    }

    private IEnumerator TryLoading()
    {
        foreach(var pair in effectpairs)
        {
            GameObject eff;
            eff = Instantiate(pair.particlePrefab);

            if(eff != null)
            {

                eff.SetActive(true);
                ParticleSystem ps = eff.GetComponent<ParticleSystem>();

                ps.Play();
                yield return null;
                ps.Stop();

                eff.SetActive(false);
                vfxPool[pair.name].Enqueue(eff);
            }
        }
    }

    public void PlayVFX(ActionEffect effectname, Vector3 position, Quaternion rotation, float lifeTime = 1.0f)
    {
        GameObject eff;
        if (vfxPool[effectname].Count > 0)
        {
            eff = vfxPool[effectname].Dequeue();
        }
        else
        {
            eff = Instantiate(System.Array.Find(effectpairs, s => s.name == effectname).particlePrefab);
        }

        if (eff == null)
            return;

        eff.transform.SetPositionAndRotation(position, rotation);
        eff.SetActive(true);

        StartCoroutine(ReturnVFX(eff, effectname, lifeTime));
    }

    private IEnumerator ReturnVFX(GameObject go, ActionEffect id, float  lifeTime = 1.0f)
    {
        yield return new WaitForSeconds(lifeTime);
        go.SetActive(false);
        vfxPool[id].Enqueue(go);
    }
}
