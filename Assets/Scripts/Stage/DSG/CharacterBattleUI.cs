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

        [SerializeField] private List<StatusSpritePair> spritePairs;

        private Dictionary<EStatusEffectType, Sprite> statusSprites;
        private Dictionary<EStatusEffectType, Image> activeIcons;

        private Camera mainCamera;
        private RectTransform rectTransform;
        private Transform target;

        [SerializeField]
        private Vector3 offset = new Vector3(0, 1.5f, 0);


        public void Init(Character character)
        {
            //Debug.Log("Init");

            if (character == null || character.characterData == null) return;

            activeIcons = new Dictionary<EStatusEffectType, Image>();
            healthSlider.maxValue = character.characterData.maxHp;
            healthSlider.value = character.characterData.maxHp;

            gaugeSlider.maxValue = character.BattleComp.maxSkillGauge;
            gaugeSlider.value = 0;

            character.BattleComp.OnDamaged += HealthUpdate;
            character.BattleComp.OnChangeGauge += GaugeUpdate;

            character.StatusEffectComp.OnEffectAdded = OnEffectAdded;
            character.StatusEffectComp.OnEffectRemoved = OnEffectRemoved;
            character.StatusEffectComp.OnEffectEndTurn = OnEffectEndTurn;
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

            mainCamera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
        }
        private void Start()
        {

        }

        private void HealthUpdate(float CurrHp)
        {
            healthSlider.value = CurrHp;
        }
        private void GaugeUpdate(float CurrGauge)
        {
            gaugeSlider.value = CurrGauge;
        }
        private void OnEffectAdded(IStatusEffect effect)
        {
            if (activeIcons.TryGetValue(effect.type, out Image image))
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

            activeIcons.TryAdd(effect.type, icon);

            if (statusSprites.TryGetValue(effect.type, out Sprite sprite))
            {
                icon.sprite = sprite;
            }
            else { icon.sprite = null; }

            icon.enabled = true;
            activeIcons.TryAdd(effect.type, icon);
        }
        private void OnEffectRemoved(IStatusEffect effect)
        {
            if (!activeIcons.TryGetValue(effect.type, out Image icon))
                return;

            Destroy(icon.gameObject);
            activeIcons.Remove(effect.type);
        }
        private void OnEffectEndTurn(IStatusEffect effect)
        {
            if (activeIcons.TryGetValue(effect.type, out Image image))
            {
                image.GetComponentInChildren<TextMeshProUGUI>().text = $"Stack : {effect.amount}";
                return;
            }
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
    }
}