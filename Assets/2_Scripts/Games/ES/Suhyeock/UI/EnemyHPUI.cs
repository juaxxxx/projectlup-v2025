using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class EnemyHPUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject OverheadUIPrefab;

        [SerializeField]
        private float YOffset = 140.0f;

        private GameObject canvas;
        private Slider hpSlider;

        private RectTransform uiRect;
        private Camera mainCamera;

        private EnemyBlackboard enemyBlackboard;

        [HideInInspector]
        public GameObject UIInstance;

        private void Awake()
        {
            enemyBlackboard = GetComponent<EnemyBlackboard>();
            canvas = GameObject.Find("HUDCanvas");
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            mainCamera = Camera.main;
            Init();
        }

        private void LateUpdate()
        {
            Vector3 ScreenPostion = mainCamera.WorldToScreenPoint(transform.position);
            ScreenPostion.y += YOffset;
            uiRect.position = ScreenPostion;
        }

        private void Init()
        {
            UIInstance = Instantiate(OverheadUIPrefab, canvas.transform);
            uiRect = UIInstance.GetComponent<RectTransform>();

            Slider[] slider = UIInstance.GetComponentsInChildren<Slider>();
            for (int i = 0; i < slider.Length; i++)
            {
                if (slider[i].gameObject.name == "HPSlider")
                {
                    hpSlider = slider[i];
                }

            }

            if (hpSlider != null)
            {
                hpSlider.maxValue = 1f;
                hpSlider.minValue = 0f;
                UpdateHPUI();
            }
            UIInstance.SetActive(false);
        }

        public void UpdateHPUI()
        {
            float hpRatio = enemyBlackboard.healthComponent.HP / enemyBlackboard.healthComponent.MaxHP;
            hpSlider.DOValue(hpRatio, 0.2f).SetEase(Ease.OutCubic);
            //hpSlider.value = hpRatio;
        }


    }
}


