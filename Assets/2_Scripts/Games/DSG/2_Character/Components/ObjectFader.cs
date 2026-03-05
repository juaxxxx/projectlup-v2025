using LUP.DSG;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.DSG
{
    public class ObjectFader : MonoBehaviour
    {
        public float fadeSpeed = 2.0f;
        public float targetOpacity = 0.2f;

        private Renderer[] renderers;
        private MaterialPropertyBlock materialPropertyBlock;

        private static readonly int OpacityId = Shader.PropertyToID("_Opacity");

        public bool doFade = false;
        private float currentOpacity = 1.0f;

        void Start()
        {
            renderers = GetComponentsInChildren<Renderer>(true);
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        void Update()
        {
            float target = doFade ? targetOpacity : 1f;

            if (Mathf.Abs(currentOpacity - target) < 0.001f) return;

            currentOpacity = Mathf.Lerp(currentOpacity, target, fadeSpeed);
            ApplyOpacity(currentOpacity);
        }

        private void ApplyOpacity(float value)
        {
            if (renderers == null || renderers.Length == 0) return;

            materialPropertyBlock.SetFloat(OpacityId, value);

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null) continue;
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}