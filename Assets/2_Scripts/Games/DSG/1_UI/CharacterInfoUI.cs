using LUP.DSG.Utils.Enums;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterInfoUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        private Image attributeIcon;

        private AttributeIconContainer iconContainer;
        public void SetCharacterInfo(EAttributeType attribute, int level)
        {
            if (levelText != null) levelText.text = $"LV.{level}";

            if(iconContainer == null)
            {
                DeckStrategyStage stage = LUP.StageManager.Instance != null
                                          ? (LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage)
                                          : null;

                iconContainer = stage != null ? stage.GetComponent<AttributeIconContainer>() : null;
            }
            if (iconContainer == null || attributeIcon == null) return;

            AttributeTypeImage icon = iconContainer.GetTypeByAttributeImage(attribute);
            if (icon.typeIcon == null) return;

            attributeIcon.sprite = icon.typeIcon;
            attributeIcon.color = icon.typeColor;
        }
    }
}
