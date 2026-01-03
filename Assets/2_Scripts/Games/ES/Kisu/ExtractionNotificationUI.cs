using System.Collections;
using TMPro;
using UnityEngine;

namespace LUP.ES
{
    public class ExtractionNotificationUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private float displayDuration = 3f;

        private Coroutine displayRoutine;

        public void ShowMessage(string message)
        {
            if (displayRoutine != null)
                StopCoroutine(displayRoutine);

            displayRoutine = StartCoroutine(DisplayRoutine(message));
        }

        IEnumerator DisplayRoutine(string message)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);

            yield return new WaitForSeconds(displayDuration);

            notificationText.gameObject.SetActive(false);
        }
    }
}