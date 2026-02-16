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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
