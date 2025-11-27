using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class PlayerOverheadUI : MonoBehaviour
    {
        public EventBroker eventBroker;
        [SerializeField]
        private GameObject OverheadUIPrefab;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private float YOffset = 50.0f;

        private Slider hpSlider;
        private Slider ammoSlider;

        private RectTransform uiRect;
        private Camera mainCamera;
        private PlayerBlackboard blackboard;

        private void Awake()
        {
            blackboard = GetComponent<PlayerBlackboard>();
            hpSlider = GetComponent<Slider>();
            ammoSlider = GetComponent<Slider>();
            eventBroker.OnReloadTimeUpdate += UpdateReloadUI;
        }
        void Start()
        {
            mainCamera = Camera.main;

            Init();
        }

        private void LateUpdate()
        {
            Vector3 ScreenPostion = mainCamera.WorldToScreenPoint (transform.position);
            ScreenPostion.y += YOffset;
            uiRect.position = ScreenPostion;
        }
        private void Init()
        {
            GameObject UIInstance = Instantiate(OverheadUIPrefab, canvas.transform);
            uiRect = UIInstance.GetComponent<RectTransform>();

            Slider[] slider = UIInstance.GetComponentsInChildren<Slider>();
            for (int i = 0; i < slider.Length; i++)
            {
                if (slider[i].gameObject.name == "HPSlider")
                {
                    hpSlider = slider[i];
                }
                else if (slider[i].gameObject.name == "AmmoSlider")
                {
                    ammoSlider = slider[i];
                }

            }

            if (hpSlider != null)
            {
                hpSlider.maxValue = 1f;
                hpSlider.minValue = 0f;
                UpdateHPUI();
            }

            if (ammoSlider != null)
            {
                ammoSlider.maxValue = 1f;
                ammoSlider.minValue = 0f;
                //UpdateAmmoUI();
            }
        }

        public void UpdateHPUI()
        {
            float hpRatio = blackboard.healthComponent.HP / blackboard.healthComponent.MaxHP;
            hpSlider.value = hpRatio;
        }

        public void UpdateAmmoUI()
        {
            if (blackboard.weapon.weaponItem.data.weaponType == WeaponType.Ranged)
            {
                Gun gun = blackboard.weapon as Gun;
                if (gun != null)
                {
                    RangedWeaponItemData data = gun.weaponItem.data as RangedWeaponItemData;
                    if (data != null)
                    {
                        float ammoRatio = gun.magAmmo / (float)data.magCapacity;
                        ammoSlider.value = ammoRatio;
                    }
                    return;
                }
            }
            else
                ammoSlider.value = 1f;
        }

        public void UpdateReloadUI(float time, float reloadTime)
        {
            float reloadRatio = time / reloadTime;
            ammoSlider.value = reloadRatio;
        }
    }
}
