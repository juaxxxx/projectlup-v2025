using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static LUP.DSG.Character;
using static UnityEngine.GraphicsBuffer;

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

        private Dictionary<EStatusEffectType, Sprite> statusSprites;
        private Dictionary<EStatusEffectType, Image> activeIcons;

        private Image gaugeImage;

        public void Init(Character character)
        {
            if (character == null || character.characterData == null) return;
            Debug.Log("Init : " + character.characterData.ID);

            activeIcons = new Dictionary<EStatusEffectType, Image>();
            healthSlider.maxValue = character.characterData.maxHp;
            healthSlider.value = character.characterData.maxHp;

            gaugeSlider.maxValue = character.BattleComp.maxSkillGauge;
            gaugeSlider.value = 0;
            gaugeImage = gaugeSlider.fillRect.GetComponent<Image>();
            // 각 UI마다 개별 머티리얼을 새로 생성하여 UI 이펙트를 개별 적용
            gaugeImage.material = new Material(gaugeImage.material);

            character.BattleComp.OnDamaged += HealthUpdate;
            character.BattleComp.OnChangeGauge += GaugeUpdate;

            character.StatusEffectComp.OnEffectAdded = OnEffectAdded;
            character.StatusEffectComp.OnEffectRemoved = OnEffectRemoved;
            character.StatusEffectComp.OnEffectEndTurn = OnEffectEndTurn;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            AttributeIconContainer iconContainer = stage.GetComponent<AttributeIconContainer>();
            AttributeTypeImage typeIcon = iconContainer.GetTypeByAttributeImage(character.characterData.type);

            centerAreaImage.sprite = typeIcon.typeIcon;
            centerAreaImage.color = typeIcon.typeColor;

        }

        private void OnDisable()
        {
            //if (owner == null || owner.characterData == null) return;

            //owner.BattleComp.OnDamaged -= HealthUpdate;

            //owner.StatusEffectComp.OnEffectAdded -= OnEffectAdded;
            //owner.StatusEffectComp.OnEffectRemoved -= OnEffectRemoved;
        }
        private void Awake()
        {
            statusSprites = new Dictionary<EStatusEffectType, Sprite>();
            foreach (var pair in spritePairs)
            {
                if (!statusSprites.ContainsKey(pair.Name))
                    statusSprites.Add(pair.Name, pair.Sprite);
            }
        }

        private void HealthUpdate(float CurrHp)
        {
            healthSlider.value = CurrHp;
        }
        private void GaugeUpdate(float CurrGauge)
        {
            gaugeSlider.value = CurrGauge;
            if(gaugeSlider.value >= 100)
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
            if (activeIcons.TryGetValue(effect.effectType, out Image image))
            {
                image.GetComponentInChildren<TextMeshProUGUI>().text = $"Stack : {effect.amount}";
                return;
            }

            GameObject go = new GameObject("StatusIcon", typeof(Image));
            Image icon = go.GetComponent<Image>();

            icon.gameObject.SetActive(true);
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

            activeIcons.TryAdd(effect.effectType, icon);

            if (statusSprites.TryGetValue(effect.effectType, out Sprite sprite))
            {
                icon.sprite = sprite;
            }
            else { icon.sprite = null; }

            icon.enabled = true;
            activeIcons.TryAdd(effect.effectType, icon);
        }
        private void OnEffectRemoved(StatusEffect effect)
        {
            if (!activeIcons.TryGetValue(effect.effectType, out Image icon))
                return;

            Destroy(icon.gameObject);
            activeIcons.Remove(effect.effectType);
        }
        private void OnEffectEndTurn(StatusEffect effect)
        {
            if (activeIcons.TryGetValue(effect.effectType, out Image image))
            {
                image.GetComponentInChildren<TextMeshProUGUI>().text = $"Stack : {effect.amount}";
                return;
            }
        }
    }
}