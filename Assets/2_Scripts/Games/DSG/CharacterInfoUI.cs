using LUP.DSG.Utils.Enums;
using System.ComponentModel;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterInfoUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        private Image attributeIcon;

        private Transform target;
        [SerializeField]
        private Vector3 offset = new Vector3(0, 2.0f, 0);

        private Camera mainCamera;
        private RectTransform rectTransform;

        void Awake()
        {
            mainCamera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 worldPos = target.position + offset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

            rectTransform.position = screenPos;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            gameObject.SetActive(true);
            LineupSlot slot = target.GetComponent<LineupSlot>();
        }

        public void ReleaseTarget()
        {
            gameObject.SetActive(false);
            target = null;
        }

        public void SetCharacterInfo(EAttributeType attribute, int level)
        {
            StringBuilder sb = new StringBuilder("LV." + level.ToString());
            levelText.text = sb.ToString();

            FormationSystem system = FindFirstObjectByType<FormationSystem>();
            AttributeTypeImage typeIcon = system.GetTypeByAttributeImage(attribute);

            if (typeIcon.TypeIcon == null)
                return;

            attributeIcon.sprite = typeIcon.TypeIcon;
            attributeIcon.color = typeIcon.TypeColor;
        }
    }
}
