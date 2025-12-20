using LUP.DSG;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    float curretOpacity;
    List<Material> materials = new List<Material>();
    public bool doFade = false;

    void Start()
    {
        SkinnedMeshRenderer[] skinnedMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        if(skinnedMeshList.Length > 0)
        {
            foreach(SkinnedMeshRenderer mesh in skinnedMeshList)
            {
                materials.Add(mesh.material);
            }
        }
        MeshRenderer[] meshList = GetComponentsInChildren<MeshRenderer>();
        if (meshList.Length > 0)
        {
            foreach (MeshRenderer mesh in meshList)
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
