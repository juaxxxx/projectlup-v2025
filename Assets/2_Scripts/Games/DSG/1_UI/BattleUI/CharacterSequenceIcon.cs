using LUP.DSG.Utils.Enums;
using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class CharacterSequenceIcon : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image background;
        [SerializeField]
        private UnityEngine.UI.Image portrait;
        [SerializeField]
        private UnityEngine.UI.Image attributeIcon;
        [SerializeField]
        private TextMeshProUGUI level;

        public void SetIconData(EAttributeType type, int characterLevel, bool isEnemy)
        {
            level.text = "Lv." + characterLevel;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            AttributeIconContainer iconContainer = stage.GetComponent<AttributeIconContainer>();
            AttributeTypeImage attribute = iconContainer.GetTypeByAttributeImage(type);

            attributeIcon.sprite = attribute.typeIcon;
            attributeIcon.color = attribute.typeColor;


            UnityEngine.Color color = isEnemy ? UnityEngine.Color.red : UnityEngine.Color.blue;
            color.a = 0.6f;
            background.color = color;
        }
    }
}