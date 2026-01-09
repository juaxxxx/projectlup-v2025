using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace LUP.ST
{
    public class ResultSlotUI : MonoBehaviour
    {
        [Header("UI 참조")]
        public Image characterIcon;
        public TextMeshProUGUI levelText;
        public Slider expSlider;
        public TextMeshProUGUI expText;

        // 슬롯 초기화 (이미지, 현재 레벨 등)
        public void SetupSlot(Sprite thumbnail, int level, float currentExp, float maxExp)
        {
            characterIcon.sprite = thumbnail;
            levelText.text = $"LV. {level}";

            expSlider.maxValue = maxExp;
            expSlider.value = currentExp;
            expText.text = $"{(int)currentExp} / {(int)maxExp}";
        }

        // 경험치 차오르는 애니메이션
        public IEnumerator AnimateExp(int startExp, int gainAmount, int maxExp)
        {
            float duration = 1.5f; // 1.5초 동안 차오름
            float elapsed = 0f;
            int targetExp = startExp + gainAmount;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float current = Mathf.Lerp(startExp, targetExp, elapsed / duration);

                expSlider.value = current;
                expText.text = $"{(int)current} / {maxExp}";

                yield return null;
            }

            // 마지막 값 보정
            expSlider.value = targetExp;
            expText.text = $"{targetExp} / {maxExp}";
        }
    }
}