using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LUP.ES
{
    public class ExtractionNotificationUI : MonoBehaviour
    {
        [SerializeField] private RectTransform notificationPanel;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private float displayDuration = 3f;

        private DG.Tweening.Sequence displaySequence;

        public void ShowMessage(string message)
        {
            if (displaySequence != null && displaySequence.IsActive())
            {
                displaySequence.Kill();
            }

            notificationText.text = message;

            Canvas.ForceUpdateCanvases();

            notificationPanel.gameObject.SetActive(true);
            notificationText.gameObject.SetActive(true);

            notificationPanel.localScale = new Vector3(0, 1, 1);
            notificationText.alpha = 0f;

            displaySequence = DOTween.Sequence();
            displaySequence.SetUpdate(true);

            displaySequence.Append(notificationPanel.DOScaleX(1f, 0.5f).SetEase(Ease.OutExpo));
            displaySequence.Append(notificationText.DOFade(1f, 0.3f));
            displaySequence.AppendInterval(displayDuration);
            displaySequence.Append(notificationText.DOFade(0f, 0.3f));
            displaySequence.Append(notificationPanel.DOScaleX(0f, 0.5f).SetEase(Ease.InExpo));
            displaySequence.OnComplete(() =>
            {
                notificationPanel.gameObject.SetActive(false);
                notificationText.gameObject.SetActive(false);
            });
        }



        //private Coroutine displayRoutine;

        //public void ShowMessage(string message)
        //{
        //    if (displayRoutine != null)
        //        StopCoroutine(displayRoutine);

        //    displayRoutine = StartCoroutine(DisplayRoutine(message));
        //}

        IEnumerator DisplayRoutine(string message)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);

            yield return new WaitForSeconds(displayDuration);

            notificationText.gameObject.SetActive(false);
        }
    }
}