using DG.Tweening;
using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class ConstructionDecisionView : MonoBehaviour
    {
        [SerializeField] private Button acceptBtn;
        [SerializeField] private Button rejectBtn;

        private readonly CompositeDisposable cd = new();

        [SerializeField] private RectTransform constructionDecisionPanel;

        private Vector2 onScreenConstructionDecisionPanelPos;
        private Vector2 offScreenConstructionDecisionPanelPos;

        private void Start()
        {
            onScreenConstructionDecisionPanelPos = Vector2.zero;
            offScreenConstructionDecisionPanelPos = new Vector2(0f, -100f);
        }

        public void Bind(ConstructionDecisionViewModel vm)
        {
            cd.Clear();

            acceptBtn?.onClick.RemoveAllListeners();
            acceptBtn?.onClick.AddListener(() => vm.OnClickAccept.OnNext(Unit.Default));

            rejectBtn?.onClick.RemoveAllListeners();
            rejectBtn?.onClick.AddListener(() => vm.OnClickReject.OnNext(Unit.Default));
        }

        private void OnDestroy()
        {
            cd.Dispose();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            constructionDecisionPanel.DOAnchorPos(onScreenConstructionDecisionPanelPos, 0.2f).SetEase(Ease.OutCubic);
        }

        public void Hide()
        {
            constructionDecisionPanel.DOAnchorPos(offScreenConstructionDecisionPanelPos, 0.2f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }

}
