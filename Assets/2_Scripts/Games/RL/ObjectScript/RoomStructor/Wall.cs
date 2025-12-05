using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

using System.Collections;
using static UnityEngine.Rendering.DebugUI;

namespace LUP.RL
{
    public class Wall : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 WorldPosition;
        public float TargetDistortion = 5f;
        public GameObject NoiseObject;

        private MeshRenderer NoiseWallRenderer;
        private Material WallMaterial;

        private Coroutine distortionCoroutine;


        private void Awake()
        {
            WorldPosition = this.transform.position;

            if (NoiseObject)
            {
                NoiseWallRenderer = NoiseObject.GetComponent<MeshRenderer>();
                WallMaterial = NoiseWallRenderer.material;

                NoiseWallRenderer.enabled = false;
                //WallMaterial.SetFloat("_Distortion", 5.0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") == false)
                return;

            if (NoiseWallRenderer) NoiseWallRenderer.enabled = true;

            if (distortionCoroutine != null)
            {
                StopCoroutine(distortionCoroutine);
                distortionCoroutine = null;
            }


            Vector3 hitPosition = other.ClosestPoint(transform.position);

            distortionCoroutine = StartCoroutine(ChangeDistortion(WallMaterial.GetFloat("_Distortion"), TargetDistortion, 2.0f));
            WallMaterial.SetVector("_StartPosition", hitPosition);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") == false)
                return;

            Vector3 StayedPosition = other.ClosestPoint(transform.position);
            WallMaterial.SetVector("_StartPosition", StayedPosition);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            if (distortionCoroutine != null)
            {
                StopCoroutine(distortionCoroutine);
                distortionCoroutine = null;
            }


            distortionCoroutine = StartCoroutine(ChangeDistortion(WallMaterial.GetFloat("_Distortion"), 0f, 0.5f));

            StartCoroutine(DisableRendererAfterDelay(1.0f));
        }

        private IEnumerator ChangeDistortion(float start, float end, float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float value = Mathf.Lerp(start, end, timer / duration);
                WallMaterial.SetFloat("_Distortion", value);
                yield return null;
            }

            WallMaterial.SetFloat("_Distortion", end);
        }

        private IEnumerator DisableRendererAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (NoiseWallRenderer)
                NoiseWallRenderer.enabled = false;
        }
    }
}

