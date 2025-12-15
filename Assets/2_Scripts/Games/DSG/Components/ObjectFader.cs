using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed = 1.0f;
    float curretOpacity;
    List<Material> materials = new List<Material>();
    public bool doFade = false;
    public bool isActive = false;

    [SerializeField]
    private float proximityRadius = 2.0f;
    private Collider[] hitColliders;
    private HashSet<ObjectFader> lastTargets = new HashSet<ObjectFader>();

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

            if(objectFader != null && objectFader.gameObject != gameObject)
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

    public void OffFade()
    {
        foreach (ObjectFader target in lastTargets)
        {
            target.doFade = false;
        }

        lastTargets.Clear();
        isActive = false;
    }
}
