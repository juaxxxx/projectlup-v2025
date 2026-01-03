using UnityEngine;
using TMPro;

namespace LUP.ST
{
    public class DamagePopup : MonoBehaviour
    {
        [Header("설정")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float fadeSpeed = 1f;
        [SerializeField] private float lifetime = 1f;

        private TextMeshProUGUI text;
        private Color textColor;
        private float spawnTime;
        private Transform mainCamera;

        void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            mainCamera = Camera.main.transform;
        }

        public void Setup(float damage)
        {
            if (text != null)
            {
                text.text = Mathf.RoundToInt(damage).ToString();
                textColor = text.color;
            }
            spawnTime = Time.time;
        }

        void Update()
        {
            // 위로 이동
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            // 카메라 바라보기
            transform.LookAt(transform.position + mainCamera.forward);

            // 페이드 아웃
            float elapsed = Time.time - spawnTime;
            float alpha = 1f - (elapsed / lifetime);

            if (text != null)
            {
                textColor.a = alpha;
                text.color = textColor;
            }

            // 수명 끝나면 삭제
            if (elapsed >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}