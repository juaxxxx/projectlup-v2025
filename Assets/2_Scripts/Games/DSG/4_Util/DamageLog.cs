using LUP.DSG.Utils.Enums;
using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class DamageLog : MonoBehaviour
    {
        public float LogSpeed = 1f;
        public float fadeDuration = 0.8f;

        private TMP_Text damageText;   // 숫자
        private TMP_Text weakText;     // "WEAK"

        private float timer;

        private void Awake()
        {
            // 이름 기반으로 명확하게 찾기
            damageText = transform.Find("Damage")?.GetComponent<TMP_Text>();
            weakText = transform.Find("Weak")?.GetComponent<TMP_Text>();

            if (weakText != null)
                weakText.gameObject.SetActive(false);
        }
        public void Setup(float damage, bool isWeak)
        {
            if (damageText == null || weakText == null)
            {
                Debug.LogWarning($"[DamageLog] Find 실패: damageText={(damageText != null)} weakText={(weakText != null)} path={gameObject.name}");
                return;
            }

            Debug.Log($"[DamageLog] isWeak={isWeak} (weak GO before={weakText.gameObject.activeSelf})");

            damageText.text = damage.ToString("F0");
            weakText.gameObject.SetActive(isWeak);

            if (weakText != null)
                weakText.gameObject.SetActive(isWeak);

            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180f, 0);
            }

            transform.localScale = Vector3.one * 0.7f;
        }

        private void Update()
        {
            transform.position += Vector3.up * 1.1f * LogSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            ApplyAlpha(damageText, alpha);
            ApplyAlpha(weakText, alpha);

            if (timer >= fadeDuration)
                Destroy(gameObject);
        }

        private static void ApplyAlpha(TMP_Text text, float alpha)
        {
            if (text == null) return;
            var c = text.color;
            c.a = alpha;
            text.color = c;
        }

    }
}
