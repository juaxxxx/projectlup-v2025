using OpenCvSharp;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace LUP.RL
{
    public class Hpbar : MonoBehaviour
    {
        public Slider slider;
        private Transform target;  // Enemy -> Transform으로 변경
        private Camera cam;
        public RectTransform rect;
        public Vector3 offset = new Vector3(0, 2f, 0);
        [SerializeField] private Image fillImage;

        // 통합 Init 메서드
        public void Init(Transform targetTransform, HealthCenter health)
        {
            target = targetTransform;
            SetHealthSystem(health);
        }

        // 기존 Enemy용 (호환성 유지)
        public void Init(Enemy enemy)
        {
            Init(enemy.transform, enemy.healthCenter);
        }

        // Player용 호출
        public void Init(Archer archer)
        {
            Init(archer.transform, archer.HealthCenter);
        }

        void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void Update()
        {
            if (target == null) return;

            Vector3 worldPos = target.position + offset;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

            if (screenPos.z < 0) return;
            rect.position = screenPos;
        }

        public void SetHealthSystem(HealthCenter health)
        {
            slider.maxValue = health.MaxHp;
            slider.value = health.CurrentHp;
            health.OnHpChanged += UpdateBar;
            UpdateBar(health.CurrentHp, health.MaxHp);
        }

        public void UpdateBar(int current, int max)
        {
            slider.value = current;
            if (current <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void DisableHPBar(HealthCenter health)
        {
            health.OnHpChanged -= this.UpdateBar;
        }
        public void HpBarDisable()
        {
            gameObject.SetActive(false);
        }
        public void HpBarEnable()
        {
            gameObject.SetActive(true);
        }
    }

}