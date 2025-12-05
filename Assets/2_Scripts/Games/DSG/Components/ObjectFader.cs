using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed;
    float curretOpacity;
    List<Material> materials = new List<Material>();
    public bool doFade = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if(doFade)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
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
}
