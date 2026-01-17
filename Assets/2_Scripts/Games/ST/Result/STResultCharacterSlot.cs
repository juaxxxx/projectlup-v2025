using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LUP.ST
{
    /// <summary>
    /// 결과창의 개별 캐릭터 슬롯 UI
    /// </summary>
    public class STResultCharacterSlot : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private Image characterIcon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI expGainText;

        [Header("경험치 바 (Image Fill 방식)")]
        [SerializeField] private Image expBarFill;

        [Header("레벨업 이펙트")]
        [SerializeField] private GameObject levelUpEffect;

        private int characterId;
        private int previousLevel;
        private int expGained;

        /// <summary>
        /// 슬롯 초기화
        /// </summary>
        public void Setup(STCharacterData characterData, OwnedCharacterInfo ownedInfo, int expReward)
        {
            if (characterData == null || ownedInfo == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            characterId = characterData.characterId;
            previousLevel = ownedInfo.level;
            expGained = expReward;

            // 아이콘 설정
            if (characterIcon != null && characterData.thumbnail != null)
                characterIcon.sprite = characterData.thumbnail;

            // 이름 설정
            if (nameText != null)
                nameText.text = characterData.characterName;

            // 레벨 설정
            if (levelText != null)
                levelText.text = $"Lv. {previousLevel}";

            // 경험치 획득량 표시
            if (expGainText != null)
                expGainText.text = $"+{expReward} Exp";

            // 경험치 바 초기화
            if (expBarFill != null)
            {
                int maxExp = GetRequiredExp(previousLevel);
                expBarFill.fillAmount = (float)ownedInfo.currentExp / maxExp;
            }

            // 레벨업 이펙트 숨기기
            if (levelUpEffect != null)
                levelUpEffect.SetActive(false);
        }

        /// <summary>
        /// 경험치 적용 및 레벨업 처리
        /// </summary>
        public void ApplyExpReward(OwnedCharacterInfo ownedInfo)
        {
            if (ownedInfo == null) return;

            int remainingExp = expGained;
            int currentLevel = ownedInfo.level;
            int currentExp = ownedInfo.currentExp;

            // 경험치 추가 및 레벨업 처리
            while (remainingExp > 0)
            {
                int requiredExp = GetRequiredExp(currentLevel);
                int expToNextLevel = requiredExp - currentExp;

                if (remainingExp >= expToNextLevel)
                {
                    // 레벨업!
                    remainingExp -= expToNextLevel;
                    currentLevel++;
                    currentExp = 0;
                }
                else
                {
                    // 경험치만 추가
                    currentExp += remainingExp;
                    remainingExp = 0;
                }
            }

            // 실제 데이터에 반영
            ownedInfo.level = currentLevel;
            ownedInfo.currentExp = currentExp;

            // UI 업데이트
            if (levelText != null)
                levelText.text = $"Lv. {currentLevel}";

            if (expBarFill != null)
            {
                int maxExp = GetRequiredExp(currentLevel);
                expBarFill.fillAmount = (float)currentExp / maxExp;
            }

            // 레벨업 이펙트
            if (currentLevel > previousLevel && levelUpEffect != null)
            {
                levelUpEffect.SetActive(true);
            }
        }

        /// <summary>
        /// 레벨업에 필요한 경험치 (레벨 * 100)
        /// </summary>
        private int GetRequiredExp(int level)
        {
            return level * 100;
        }
    }
}