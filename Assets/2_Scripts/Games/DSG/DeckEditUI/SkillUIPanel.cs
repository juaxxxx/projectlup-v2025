using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace LUP.DSG
{
    public class SkillUIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform canvas;
        [SerializeField] private BattleSystem battle;

        private Vector2 OurhiddenPos = new Vector2(-1530, 405);
        private Vector2 OurshowPos = new Vector2(-720, 405);

        private Vector2 EnmyhiddenPos = new Vector2(1594, 405);
        private Vector2 EnmyshowPos = new Vector2(707, 405);

        private float slideDuration = 0.7f;

        private void Start()
        {
            BattleSystem battle = FindAnyObjectByType<BattleSystem>();
            if (battle != null)
            {
                battle.onStartSkill += ShowSkillPanel;
            }
        }

        public void ShowSkillPanel(Character Attacker)
        {
            GameObject Object = Instantiate(panel, canvas);

            TextMeshProUGUI skillNameText = Object.transform.Find("SkillBanner/SkillName").GetComponent<TextMeshProUGUI>();
            skillNameText.text = Attacker.BattleComp.skillInfo.Skillname;

            Sprite AttackerIcon;
            CharacterIconCache.TryGetByModelId(Attacker.characterModelData.ID, out AttackerIcon);

            Image ObjectIcon = Object.transform.Find("SkillBanner/PlayerIcon").GetComponent<Image>();
            ObjectIcon.sprite = AttackerIcon;

            RectTransform rt = Object.GetComponent<RectTransform>();
            Sequence seq = DOTween.Sequence();

            if (Attacker.isEnemy)
            {
                rt.anchoredPosition = EnmyhiddenPos;

                Transform banner = Object.transform.Find("SkillBanner");
                Image bannerImage = banner.GetComponent<Image>();


                bannerImage.color = Color.red;

                seq.Append(rt.DOAnchorPos(EnmyshowPos, slideDuration).SetEase(Ease.OutCubic));
                seq.AppendInterval(1.5f);
                seq.Append(rt.DOAnchorPos(EnmyhiddenPos, slideDuration).SetEase(Ease.OutCubic));
            }
            else
            {
                rt.anchoredPosition = OurhiddenPos;

                seq.Append(rt.DOAnchorPos(OurshowPos, slideDuration).SetEase(Ease.OutCubic));
                seq.AppendInterval(1.5f);
                seq.Append(rt.DOAnchorPos(OurhiddenPos, slideDuration).SetEase(Ease.OutCubic));
            }

            seq.OnComplete(() =>
            {
                Destroy(Object);
            });
        }
    }
}