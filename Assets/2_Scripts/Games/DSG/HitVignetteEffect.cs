using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LUP.DSG
{
    public class HitVignetteEffect : MonoBehaviour
    {
        [Header("비네트 이미지")]
        [SerializeField] private Image vignetteImage;

        [Header("효과 설정")]
        [SerializeField] private float flashDuration = 0.4f;
        [SerializeField] private float maxAlpha = 0.4f; 

        private Coroutine currentCoroutine;
        public void PlayDamageEffect()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(FadeVignette());
        }

        private IEnumerator FadeVignette()
        {
            Color color = vignetteImage.color;

            float timer = 0f;
            while (timer < flashDuration / 2f)
            {
                timer += Time.deltaTime;
                float t = timer / (flashDuration / 2f);
                color.a = Mathf.Lerp(0f, maxAlpha, t);
                vignetteImage.color = color;
                yield return null;
            }

            timer = 0f;
            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flashDuration;
                color.a = Mathf.Lerp(maxAlpha, 0f, t);
                vignetteImage.color = color;
                yield return null;
            }

            color.a = 0f;
            vignetteImage.color = color;
            currentCoroutine = null;
        }

    }
}


