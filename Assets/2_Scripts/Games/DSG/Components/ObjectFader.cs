using LUP.DSG;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    float curretOpacity;
    List<Material> materials = new List<Material>();
    public bool doFade = false;
    public bool isActive = false;

    [SerializeField]
    private float proximityRadius = 2.0f;
    private Collider[] hitColliders;
    private HashSet<ObjectFader> lastTargets = new HashSet<ObjectFader>();
    public List<ObjectFader> ignoreTargets = new List<ObjectFader>();

    void Start()
    {
        SkinnedMeshRenderer[] meshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        if(meshList.Length > 0)
        {
            foreach(SkinnedMeshRenderer mesh in meshList)
            {
                materials.Add(mesh.material);
            }
        }
        curretOpacity = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            FindFadeTargets();
        }

        if(doFade)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    void FindFadeTargets()
    {
        hitColliders = Physics.OverlapSphere(transform.position, proximityRadius);

        HashSet<ObjectFader> targets = new HashSet<ObjectFader>();

        foreach (Collider collider in hitColliders)
        {
            ObjectFader objectFader = collider.GetComponentInChildren<ObjectFader>();

            if(objectFader != null && !ignoreTargets.Contains(objectFader))
            {
                objectFader.doFade = true;
                targets.Add(objectFader);
            }
        }

        CheckForFadeIn(targets);
    }

    void CheckForFadeIn(HashSet<ObjectFader> fadeTargets)
    {
        lastTargets.ExceptWith(fadeTargets);

        foreach (ObjectFader target in lastTargets)
        {
            target.doFade = false;
        }

        lastTargets = fadeTargets;
    }

    void FadeIn()
    {
        curretOpacity = Mathf.Lerp(curretOpacity, 1.0f, fadeSpeed);
        foreach (Material material in materials)
        {
            material.SetFloat("_Opacity", curretOpacity);
        }
    }

    void FadeOut()
    {
        curretOpacity = Mathf.Lerp(curretOpacity, 0.2f, fadeSpeed);
        foreach (Material material in materials)
        {
            material.SetFloat("_Opacity", curretOpacity);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0f, 1f, 0f, 0.4f);

    //    Gizmos.DrawSphere(transform.position, proximityRadius);
    //}

    public void FaderOn(List<LineupSlot> targets)
    {
        List<ObjectFader> ignores = new List<ObjectFader>();
        foreach(LineupSlot target in targets)
        {
            ignores.Add(target.character.GetComponent<ObjectFader>());
        }
        ignoreTargets = ignores;
        ignoreTargets.Add(this);
        isActive = true;
    }

    public void FaderOff()
    {
        foreach (ObjectFader target in lastTargets)
        {
            target.doFade = false;
        }

        lastTargets.Clear();
        isActive = false;
    }
}
