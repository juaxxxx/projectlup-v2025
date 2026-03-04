using LUP.DSG.Utils.Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    [System.Serializable]
    public struct StatusSpritePair
    {
        public EStatusEffectType Name;
        public Sprite Sprite;
    }

    public class CharacterBattleUI : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider gaugeSlider;
        [SerializeField] private Image centerAreaImage;

        [SerializeField] private List<StatusSpritePair> spritePairs;

        private readonly Dictionary<EStatusEffectType, Sprite> statusSprites = new();
        private readonly Dictionary<EStatusEffectType, Image> activeIcons = new();

        private Image gaugeImage;

        private BattleComponent battleComp;

        private void Awake()
        {
            statusSprites.Clear();
            if (spritePairs == null) return;

            for (int i = 0; i < spritePairs.Count; i++)
            {
                var pair = spritePairs[i];
                if (!statusSprites.ContainsKey(pair.Name))
                    statusSprites.Add(pair.Name, pair.Sprite);
            }
        }
        private void OnDisable()
        {
            Unsubscribe();
            ClearStatusIcons();
        }

        public void Init(Character character)
        {
            if (character == null || character.characterData == null || character.BattleComp == null || character.StatusEffectComp == null)
                return;

            battleComp = character.BattleComp;

            Debug.Log("Init : " + character.characterData.ID);

            if (healthSlider != null)
            {
                healthSlider.maxValue = character.characterData.maxHp;
                healthSlider.value = character.characterData.maxHp;
            }

            if (gaugeSlider != null)
            {
                gaugeSlider.maxValue = battleComp.maxSkillGauge;
                gaugeSlider.value = 0;

                gaugeImage = gaugeSlider.fillRect != null ? gaugeSlider.fillRect.GetComponent<Image>() : null;
                if (gaugeImage != null && gaugeImage.material != null)
                {
                    // 각 UI마다 개별 머티리얼을 새로 생성하여 UI 이펙트를 개별 적용
                    gaugeImage.material = new Material(gaugeImage.material);
                }
            }

            battleComp.OnDamaged += HealthUpdate;
            battleComp.OnChangeGauge += GaugeUpdate;
            character.StatusEffectComp.OnEffectAdded = OnEffectAdded;
            character.StatusEffectComp.OnEffectRemoved = OnEffectRemoved;
            character.StatusEffectComp.OnEffectEndTurn = OnEffectEndTurn;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            AttributeIconContainer iconContainer = stage != null ? stage.GetComponent<AttributeIconContainer>() : null;
            if (iconContainer != null && centerAreaImage != null)
            {
                AttributeTypeImage typeIcon = iconContainer.GetTypeByAttributeImage(character.characterData.type);
                centerAreaImage.sprite = typeIcon.typeIcon;
                centerAreaImage.color = typeIcon.typeColor;
            }
        }

        private void HealthUpdate(float CurrHp)
        {
            healthSlider.value = CurrHp;
        }
        private void GaugeUpdate(float CurrGauge)
        {
            gaugeSlider.value = CurrGauge;
            if (gaugeSlider.value >= 100)
            {
                gaugeImage.material.SetFloat("_CycleTime", 1.0f);
            }
            else
            {
                gaugeImage.material.SetFloat("_CycleTime", 0f);
            }
        }
        private void OnEffectAdded(StatusEffect effect)
        {
            if (panel == null) return;

            if (activeIcons.TryGetValue(effect.effectType, out Image image) && image != null)
            {
                TextMeshProUGUI stackText = image.GetComponentInChildren<TextMeshProUGUI>();
                if (stackText != null) stackText.text = $"Stack : {effect.amount}";

                return;
            }

            GameObject go = new GameObject("StatusIcon", typeof(Image));
            Image icon = go.GetComponent<Image>();

            icon.transform.SetParent(panel, false);
            icon.rectTransform.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            icon.color = Color.white;

            var textGO = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
            textGO.transform.SetParent(icon.transform, false);

            var rt = (RectTransform)textGO.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(200f, 64f);
            rt.anchoredPosition = new Vector2(0f, 32f);

            var label = textGO.GetComponent<TextMeshProUGUI>();
            label.text = $"Stack : {effect.amount}";
            label.fontSize = 36;
            label.alignment = TextAlignmentOptions.Midline;
            label.overflowMode = TextOverflowModes.Overflow;
            label.raycastTarget = false;
            label.color = Color.red;

            if (statusSprites.TryGetValue(effect.effectType, out Sprite sprite))
                icon.sprite = sprite;

            activeIcons[effect.effectType] = icon;
        }
        private void OnEffectRemoved(StatusEffect effect)
        {
            if (!activeIcons.TryGetValue(effect.effectType, out Image icon) || icon == null)
                return;

            Destroy(icon.gameObject);
            activeIcons.Remove(effect.effectType);
        }
        private void OnEffectEndTurn(StatusEffect effect)
        {
            if (activeIcons.TryGetValue(effect.effectType, out Image image))
            {
                TextMeshProUGUI label = image.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null) label.text = $"Stack : {effect.amount}";
            }
        }

        private void ClearStatusIcons()
        {
            foreach (var pair in activeIcons)
            {
                if (pair.Value != null)
                    Destroy(pair.Value.gameObject);
            }
            activeIcons.Clear();
        }

        private void Unsubscribe()
        {
            if (battleComp == null) return;

            battleComp.OnDamaged -= HealthUpdate;
            battleComp.OnChangeGauge -= GaugeUpdate;
            battleComp = null;
        }
    }
}