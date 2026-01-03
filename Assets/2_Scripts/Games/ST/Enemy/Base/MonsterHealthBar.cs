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
        [SerializeField] private float hideDelay = 3f;

        private StatComponent stats;
        private Transform mainCamera;
        private float lastDamageTime;
        private bool hasBeenHit = false;

        void Awake()
        {
            stats = GetComponent<StatComponent>();
            mainCamera = Camera.main.transform;

            if (stats != null)
            {
                stats.OnHealthChanged += OnHealthChanged;
            }

            // 처음엔 숨김
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        void OnDestroy()
        {
            if (stats != null)
            {
                stats.OnHealthChanged -= OnHealthChanged;
            }
        }

        void LateUpdate()
        {
            if (canvas == null || !canvas.gameObject.activeSelf) return;

            // 카메라 바라보기 (빌보드)
            canvas.transform.position = transform.position + offset;
            canvas.transform.LookAt(canvas.transform.position + mainCamera.forward);

            // 일정 시간 후 숨김
            if (hasBeenHit && Time.time - lastDamageTime > hideDelay)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        private void OnHealthChanged(float current, float max)
        {
            float previousHealth = fillImage.fillAmount * max;
            float damage = previousHealth - current;

            // 체력바 업데이트
            if (fillImage != null)
            {
                fillImage.fillAmount = current / max;
            }

            // 처음 맞으면 체력바 표시
            if (!hasBeenHit && current < max)
            {
                hasBeenHit = true;
                canvas.gameObject.SetActive(true);
            }

            lastDamageTime = Time.time;

            // 데미지 팝업 생성
            if (damage > 0)
            {
                SpawnDamagePopup(damage);
            }
        }

        private void SpawnDamagePopup(float damage)
        {
            if (damagePopupPrefab == null) return;

            Vector3 spawnPos = transform.position + offset + new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                popupScript.Setup(damage);
            }
        }

        // 풀링용 리셋
        public void ResetHealthBar()
        {
            hasBeenHit = false;
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }
            if (fillImage != null)
            {
                fillImage.fillAmount = 1f;
            }
        }
    }
}