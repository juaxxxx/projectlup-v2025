using UnityEngine;
using UnityEngine.UI;

namespace LUP.ST
{
    public class CharacterHpSlotUI : MonoBehaviour
    {
        [SerializeField] private Image hpFill; 

        private StatComponent boundStat;

        public void Bind(StatComponent stat)
        {
            Unbind();

            boundStat = stat;
            if (boundStat == null)
            {
                SetFill(0f);
                return;
            }

            // 초기값 1회 반영
            SetFill(boundStat.CurrentHealth / Mathf.Max(1f, boundStat.MaxHealth));

            // 이벤트 구독
            boundStat.OnHealthChanged += HandleHealthChanged;
            boundStat.OnDeath += HandleDeath;
        }

        public void Unbind()
        {
            if (boundStat != null)
            {
                boundStat.OnHealthChanged -= HandleHealthChanged;
                boundStat.OnDeath -= HandleDeath;
                boundStat = null;
            }
        }

        private void HandleHealthChanged(float cur, float max)
        {
            SetFill(cur / Mathf.Max(1f, max));
        }

        private void HandleDeath()
        {
            SetFill(0f);
        }

        private void SetFill(float ratio01)
        {
            if (hpFill == null) return;
            hpFill.fillAmount = Mathf.Clamp01(ratio01);
        }

        private void OnDisable()
        {
            Unbind();
        }
    }
}