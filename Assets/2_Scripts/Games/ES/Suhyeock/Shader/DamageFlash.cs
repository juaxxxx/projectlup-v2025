using System.Collections;
using UnityEngine;

namespace LUP.ES
{
    public class DamageFlash : MonoBehaviour
    {
        public Renderer targetRenderer;
        public Color flashColor = Color.red;
        public float flashDuration = 0.5f;

        public string colorPropertyName = "_OutlineColor";

        private Material mat;
        private Color originalColor;
        private Coroutine flashCoroutine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>();
            mat = targetRenderer.material;

            if (mat.HasProperty(colorPropertyName))
            {
                originalColor = mat.GetColor(colorPropertyName);
            }
        }

       public void TakeDamage()
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            mat.SetColor(colorPropertyName, flashColor);

            float elapsedTime = 0f;

            // 서서히 원래 색으로 돌아옴 (Fade Out만 수행)
            while (elapsedTime < flashDuration)
            {
                elapsedTime += Time.deltaTime;
                // 시간에 따른 비율(0~1) 계산
                float t = elapsedTime / flashDuration;

                // 빨간색에서 원래 색상으로 부드럽게 전환
                mat.SetColor(colorPropertyName, Color.Lerp(flashColor, originalColor, t));

                yield return null;
            }

            // 마지막에 원래 색상으로 확실히 고정
            mat.SetColor(colorPropertyName, originalColor);
        }
    }

}
