using LUP.DSG;
using LUP.DSG.Utils.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
public enum ActionEffect
{
    None,

    Attack_Melee_OneHanded,
    GetHit_Melee_OneHanded,
    Attack_Melee_TwoHanded,
    GetHit_Melee_TwoHanded,
    Attack_Magic,
    GetHit_Magic,
    Attack_Gun_Rifle,
    GetHit_Gun_Rifle,
    Attack_Throw,
    GetHit_Throw,
    Attack_SKill_QuickSlash,
    GetHit_SKill_QuickSlash,
    Attack_SKill_Earthbreaker,
    GetHit_SKill_Earthbreaker,
    Attack_SKill_AttackBuff,
    GetHit_SKill_AttackBuff,
    Attack_SKill_SearShot,
    GetHit_SKill_SearShot,
    Attack_SKill_ToxicThrow,
    GetHit_SKill_ToxicThrow,

    GetHit_Burn,
    GetHit_Poison,

    Aura_Burn,
    Aura_Poison,
    Aura_AttackBuff,
    Aura_AttackDebuff,

    ThrowBullet,
    MagicBullet,
    GunRifleBullet
}

[System.Serializable]
public struct EffectParticlePair
{
    public ActionEffect name;
    public GameObject particlePrefab;
    public string SFXName;
}

public class EffectPool : MonoBehaviour
{
    [SerializeField] private EffectParticlePair[] effectpairs;
    private Dictionary<ActionEffect, Queue<GameObject>> vfxPool = new Dictionary<ActionEffect, Queue<GameObject>>();
    private Dictionary<ActionEffect, string> effectSFX = new Dictionary<ActionEffect, string>();
    private void Awake()
    {
        foreach (var pair in effectpairs)
        {
            var q = new Queue<GameObject>();
            vfxPool[pair.name] = q;
        }

        StartCoroutine(TryLoading());
    }

    private IEnumerator TryLoading()
    {
        foreach (var pair in effectpairs)
        {
            GameObject eff;
            eff = Instantiate(pair.particlePrefab);

            if (eff != null)
            {
                eff.SetActive(true);
                ParticleSystem ps = eff.GetComponent<ParticleSystem>();

                ps.Play();
                yield return null;
                ps.Stop();

                eff.SetActive(false);
                vfxPool[pair.name].Enqueue(eff);
                effectSFX.Add(pair.name, pair.SFXName);
            }
        }
    }

    public EffectParticlePair PlayVFXAttached(ActionEffect effectname,Transform follow,Vector3 Offset, Quaternion rotation, bool loop,float lifeTime = 1.0f)
    {
        var eff = GetOrCreate(effectname);
        if (eff == null) return default;

        eff.transform.SetParent(follow, false);
        eff.transform.localPosition = Vector3.zero + Offset;
        eff.transform.localRotation = rotation;
        eff.SetActive(true);

        var ps = eff.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.loop = loop;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear(true);
            ps.Play(true);
        }
        if (!loop)
        {
            StartCoroutine(ReturnVFX(eff, effectname, lifeTime));
        }

        EffectParticlePair pair;
        pair.name = effectname;
        pair.particlePrefab = eff;
        pair.SFXName = effectSFX[effectname];

        return pair; // loop면 나중에 StopVFX로 끄기
    }
    public EffectParticlePair PlayVFX(ActionEffect effectname, Vector3 position, Quaternion rotation, bool loop, float lifeTime = 1.0f)
    {
        GameObject eff = GetOrCreate(effectname);
        if (eff == null)
            return default;

        eff.transform.SetPositionAndRotation(position, rotation);
        eff.SetActive(true);

        ParticleSystem ps = eff.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;
            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.None;

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear(true);
            ps.Play(true);
        }

        if (!loop)
        {
            StartCoroutine(ReturnVFX(eff, effectname, lifeTime));
        }

        EffectParticlePair pair;
        pair.name = effectname;
        pair.particlePrefab = eff;
        pair.SFXName = effectSFX[effectname];

        return pair;
    }
    public void StopLoopVFX(GameObject eff, ActionEffect effectname)
    {
        if (eff == null) return;

        var ps = eff.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear(true);
        }

        eff.SetActive(false);
        eff.transform.SetParent(transform, false);
        vfxPool[effectname].Enqueue(eff);
    }

    private IEnumerator ReturnVFX(GameObject go, ActionEffect id, float lifeTime = 1.0f)
    {
        yield return new WaitForSeconds(lifeTime);
        go.SetActive(false);
        vfxPool[id].Enqueue(go);
    }

    public ActionEffect GetAttackEffectByGetHITEffect(ActionEffect attackeffect)
    {
        switch (attackeffect)
        {
            case ActionEffect.Attack_Melee_OneHanded:
                return ActionEffect.GetHit_Melee_OneHanded;
            case ActionEffect.Attack_Melee_TwoHanded:
                return ActionEffect.GetHit_Melee_TwoHanded;
            case ActionEffect.Attack_Gun_Rifle:
                return ActionEffect.GetHit_Gun_Rifle;
            case ActionEffect.Attack_Magic:
                return ActionEffect.GetHit_Magic;
            case ActionEffect.Attack_Throw:
                return ActionEffect.GetHit_Throw;

            default:
                return ActionEffect.None;
        }
    }

    public string GetActionBySound(ActionEffect hiteffect)
    {
        return effectSFX[hiteffect];
    }

    public GameObject GetParticlePrefab(ActionEffect effect)
    {
        if (vfxPool.TryGetValue(effect, out var prefab))
        {
            GameObject eff = Instantiate(System.Array.Find(effectpairs, s => s.name == effect).particlePrefab);
            return eff;
        }
        return null;
    }

    private GameObject GetOrCreate(ActionEffect effectname)
    {
        if (effectname == ActionEffect.None)
            return null;

        GameObject eff;
        if (vfxPool[effectname].Count > 0)
        {
            eff = vfxPool[effectname].Dequeue();
        }
        else
        {
            eff = Instantiate(System.Array.Find(effectpairs, s => s.name == effectname).particlePrefab);
        }
        return eff;
    }
}
