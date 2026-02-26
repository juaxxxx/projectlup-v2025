using LUP.DSG;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.DSG
{
    public class ObjectFader : MonoBehaviour
    {
        public float fadeSpeed = 0.5f;
        float curretOpacity;
        List<Material> materials = new List<Material>();
        public bool doFade = false;

        public float targetOpacity = 0.2f;

        void Start()
        {
            SkinnedMeshRenderer[] skinnedMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshList.Length > 0)
            {
                foreach (SkinnedMeshRenderer mesh in skinnedMeshList)
                {
                    foreach (Material material in mesh.materials)
                    {
                        materials.Add(material);
                    }
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
            if (doFade)
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
            curretOpacity = Mathf.Lerp(curretOpacity, targetOpacity, fadeSpeed);
            foreach (Material material in materials)
            {
                material.SetFloat("_Opacity", curretOpacity);
            }
        }
    }
}