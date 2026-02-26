using LUP.DSG.Utils.Enums;
using System.ComponentModel;
using System.Text;
using TMPro;
using Unity.VisualScripting;
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
        public void SetCharacterInfo(EAttributeType attribute, int level)
        {
            StringBuilder sb = new StringBuilder("LV." + level.ToString());
            levelText.text = sb.ToString();

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            AttributeIconContainer iconContainer = stage.GetComponent<AttributeIconContainer>();
            AttributeTypeImage icon = iconContainer.GetTypeByAttributeImage(attribute);

            if (icon.typeIcon == null) return;

            attributeIcon.sprite = icon.typeIcon;
            attributeIcon.color = icon.typeColor;
        }
    }
}
