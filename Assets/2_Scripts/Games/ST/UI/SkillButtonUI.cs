using UnityEngine;
using UnityEngine.UI;

namespace LUP.ST
{
    public class SkillButtonUI : MonoBehaviour
    {
        [SerializeField] private TeamSharedSkillSystem skillSystem;

        [Header("Heal")]
        [SerializeField] private Button healButton;
        [SerializeField] private Image healCooldownOverlay;

        [Header("Buff")]
        [SerializeField] private Button buffButton;
        [SerializeField] private Image buffCooldownOverlay;

        [Header("AOE")]
        [SerializeField] private Button aoeButton;
        [SerializeField] private Image aoeCooldownOverlay;

        private void Awake()
        {
            if (skillSystem == null)
                skillSystem = FindFirstObjectByType<TeamSharedSkillSystem>();

            if (healButton) healButton.onClick.AddListener(() => skillSystem?.TryHealAllies());
            if (buffButton) buffButton.onClick.AddListener(() => skillSystem?.TryBuffAllies());
            if (aoeButton) aoeButton.onClick.AddListener(() => skillSystem?.TryAoeAllEnemies());
        }

        private void Update()
        {
            if (skillSystem == null) return;

            UpdateSkill(healButton, healCooldownOverlay, skillSystem.GetHealCd01());
            UpdateSkill(buffButton, buffCooldownOverlay, skillSystem.GetBuffCd01());
            UpdateSkill(aoeButton, aoeCooldownOverlay, skillSystem.GetAoeCd01());
        }

        private void UpdateSkill(Button button, Image cooldownOverlay,float cd01)
        {
            bool onCooldown = cd01 > 0.001f;

            // 버튼 클릭 가능 / 불가
            if (button != null)
                button.interactable = !onCooldown;

            // 쿨다운 오버레이
            if (cooldownOverlay != null)
            {
                if (onCooldown)
                {
                    cooldownOverlay.enabled = true;
                    cooldownOverlay.fillAmount = cd01; // 1 → full, 0 → empty
                }
                else
                {
                    cooldownOverlay.enabled = false;
                }
            }
        }

    }
}
