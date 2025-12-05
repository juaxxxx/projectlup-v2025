using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class SkillUIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform canvas;

        private Vector2 OurhiddenPos = new Vector2(-1530, 294);
        private Vector2 OurshowPos = new Vector2(-680, 405);

        private Vector2 EnmyhiddenPos = new Vector2(1594, 405);
        private Vector2 EnmyshowPos = new Vector2(707, 405);

        private float slideDuration = 0.5f;

        private void Start()
        {
            
        }

        public void ShowSkillPanel(Character target)
        {
            //@TODO targetcharacterData에 나중에 icon을 넣을 예정 거기서 뽑아서 paneliocon에 넣어주기 스킬이름도 마찬가지


            GameObject Object = Instantiate(panel, canvas);
            RectTransform rt = Object.GetComponent<RectTransform>();
            Sequence seq = DOTween.Sequence();

            if (target.isEnemy)
            {
                rt.anchoredPosition = EnmyhiddenPos;

                Transform banner = Object.transform.Find("SkillBanner");
                Image bannerImage = banner.GetComponent<Image>();

                bannerImage.color = Color.red;

                seq.Append(rt.DOAnchorPos(EnmyshowPos, slideDuration).SetEase(Ease.OutCubic));
                seq.AppendInterval(2f);
                seq.Append(rt.DOAnchorPos(EnmyhiddenPos, slideDuration).SetEase(Ease.OutCubic));
            }
            else
            {
                rt.anchoredPosition = OurhiddenPos;

                seq.Append(rt.DOAnchorPos(OurshowPos, slideDuration).SetEase(Ease.OutCubic));
                seq.AppendInterval(2f);
                seq.Append(rt.DOAnchorPos(OurhiddenPos, slideDuration).SetEase(Ease.OutCubic));
            }


            seq.OnComplete(() =>
            {
                Destroy(Object);
            });
        }
    }
}