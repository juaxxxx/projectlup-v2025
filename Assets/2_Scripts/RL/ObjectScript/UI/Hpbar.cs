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
        private Enemy target;
        private Camera cam;
        public RectTransform rect;
        public Vector3 offset = new Vector3(0, 2f, 0);
        [SerializeField] private Image fillImage;
        public void Init(Enemy enemy)
        {
            target = enemy;
            slider.maxValue = enemy.EnemyStats.MaxHp;
            slider.value = enemy.EnemyStats.MaxHp;

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
            if (target == null)
                Debug.Log("target null");
            {
            }
        Vector3 worldPos = target.transform.position + offset;
       
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

            if (screenPos.z < 0) return; // 카메라 뒤면 무시

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
            if(current <= 0)
            {
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }
    }

}