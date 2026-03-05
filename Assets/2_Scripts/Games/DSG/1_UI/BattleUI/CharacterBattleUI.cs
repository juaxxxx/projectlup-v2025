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
        private readonly int CycleTimeId = Shader.PropertyToID("_CycleTime");

        private readonly Stack<Image> pooledIcons = new();

        private Image gaugeImage;

        private BattleComponent battleComp;
        private StatusEffectComponent statusEffectComp;

        private void Awake()
        {
            statusSprites.Clear();
            if (spritePairs == null) return;

            for (int i = 0; i < spritePairs.Count; i++)
            {
                StatusSpritePair pair = spritePairs[i];
                if (!statusSprites.ContainsKey(pair.Name))
                    statusSprites.Add(pair.Name, pair.Sprite);
            }
        }
        private void OnDisable()
        {
            Unsubscribe();
            ClearStatusIcons();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(Character character)
        {
            if (character == null || character.characterData == null || character.BattleComp == null || character.StatusEffectComp == null)
                return;

            Unsubscribe();
            ClearStatusIcons();

            battleComp = character.BattleComp;

            if (healthSlider != null)
            {
                healthSlider.maxValue = character.characterData.maxHp;
                healthSlider.value = battleComp.currHp;
            }

            if (gaugeSlider != null)
            {
                gaugeSlider.maxValue = battleComp.maxSkillGauge;
                gaugeSlider.value = battleComp.currGauge;

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

        private void HealthUpdate(float currHp)
        {
            if (healthSlider != null) healthSlider.value = currHp;
        }
        private void GaugeUpdate(float currGauge)
        {
            if (gaugeImage == null || gaugeImage.material == null || gaugeSlider == null) return;

            gaugeSlider.value = currGauge;

            bool isFull = currGauge >= gaugeSlider.maxValue;
            gaugeImage.material.SetFloat(CycleTimeId, isFull ? 1f : 0f);
        }
        private void OnEffectAdded(StatusEffect effect)
        {
            if (panel == null) return;

            if (activeIcons.TryGetValue(effect.effectType, out Image image) && image != null)
            {
                UpdateStackLabel(image, effect.amount);
                return;
            }

            GameObject go = new GameObject("StatusIcon", typeof(Image));
            Image icon = go.GetComponent<Image>();

            icon.transform.SetParent(panel, false);
            icon.rectTransform.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            icon.color = Color.white;

            var textGO = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
            textGO.transform.SetParent(icon.transform, false);

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

            activeIcons.Remove(effect.effectType);
            ReleaseStatusIcon(icon);
        }
        private void OnEffectEndTurn(StatusEffect effect)
        {
            if (activeIcons.TryGetValue(effect.effectType, out Image image))
                UpdateStackLabel(image, effect.amount);
        }

        private void ReleaseStatusIcon(Image icon)
        {
            if (icon == null) return;

            icon.gameObject.SetActive(false);
            icon.sprite = null;
            UpdateStackLabel(icon, 0f, clearOnly: true);
            pooledIcons.Push(icon);
        }

        private void ClearStatusIcons()
        {
            if (activeIcons.Count == 0) return;

            List<Image> icons = new List<Image>(activeIcons.Values);
            activeIcons.Clear();

            for (int i = 0; i < icons.Count; i++)
                ReleaseStatusIcon(icons[i]);
        }

        private void Unsubscribe()
        {
            if (battleComp == null) return;

            battleComp.OnDamaged -= HealthUpdate;
            battleComp.OnChangeGauge -= GaugeUpdate;
            battleComp = null;
        }

        private void UpdateStackLabel(Image icon, float amount, bool clearOnly = false)
        {
            if (icon == null) return;

            TextMeshProUGUI label = icon.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label == null) return;

            label.text = clearOnly ? string.Empty : $"Stack : {amount}";
        }
    }
}