using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class DamageLog : MonoBehaviour
    {
        public float LogSpeed = 1f;
        public float fadeDuration = 0.8f;
        public Vector3 moveOffset = new Vector3(0, 1f, 0);
        private TMP_Text textMesh;
        private Color originalColor;

        private float timer;

        private void Awake()
        {
            textMesh = GetComponentInChildren<TMP_Text>();
            if (textMesh == null)
                return;
        }

        public void Setup(float damage)
        {
            if (textMesh == null) return;

            textMesh.text = damage.ToString("F0");

            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0);
            }
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void Update()
        {
            transform.position += new Vector3(0, 1f, 0) * LogSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            if (timer >= fadeDuration)
                Destroy(gameObject);
        }
    }
}