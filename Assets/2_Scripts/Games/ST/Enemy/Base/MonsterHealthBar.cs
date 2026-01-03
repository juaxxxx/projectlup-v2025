using UnityEngine;
using UnityEngine.UI;

namespace LUP.ST
{
    public class MonsterHealthBar : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image fillImage;

        [Header("데미지 팝업")]
        [SerializeField] private GameObject damagePopupPrefab;

        [Header("설정")]
        [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

        private StatComponent stats;
        private float lastDamageTime;
        private bool hasBeenHit = false;

        private float lastKnownHealth;

        void Awake()
        {
            stats = GetComponent<StatComponent>();

            if (stats != null)
            {
                stats.OnHealthChanged += OnHealthChanged;
                lastKnownHealth = stats.MaxHealth; // 시작 시 풀피로 초기화
            }

            if (canvas != null)
                canvas.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            if (stats != null)
                stats.OnHealthChanged -= OnHealthChanged;
        }

        void LateUpdate()
        {
            if (canvas == null || !canvas.gameObject.activeSelf) return;

            Transform cam = Camera.main.transform;
        }

        private void OnHealthChanged(float current, float max)
        {
            Debug.Log(" OnHealthChanged 이벤트 발생!");
            
            float damage = lastKnownHealth - current;
            lastKnownHealth = current; // 다음 데미지 계산을 위해 업데이트

            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
            }
            if (fillImage != null)
            {
                fillImage.fillAmount = current / max;
            }
            if (damage > 0)
            {
                SpawnDamagePopup(damage);
            }
        }

        private void SpawnDamagePopup(float damage)
        {
            if (damagePopupPrefab == null) return;

            Vector3 spawnPos = transform.position + offset + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0, 0.3f), 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                popupScript.Setup(damage);
            }
        }

        public void ResetHealthBar()
        {
            hasBeenHit = false;
            if (stats != null) lastKnownHealth = stats.MaxHealth;
            if (canvas != null) canvas.gameObject.SetActive(false);
        }
    }
}