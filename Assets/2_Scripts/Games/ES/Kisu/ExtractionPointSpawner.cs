using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class ExtractionPointSpawner : MonoBehaviour
    {
        [Header("Timer Reference")]
        [SerializeField] private GameTimerUI gameTimerUI;

        [Header("Extraction Points (Design Controlled)")]
        [SerializeField] private List<GameObject> extractionPoints;

        [Header("Extraction Settings")]
        [SerializeField] private float extractionDuration = 60f;

        [SerializeField] private ExtractionNotificationUI notificationUI;

        private GameObject currentExtractionPoint;
        private Coroutine extractionRoutine;

        private bool[] triggered = new bool[4];

        void Update()
        {
            if (gameTimerUI == null) return;

            float remainingTime = gameTimerUI.RemainingTime;

            CheckTrigger(remainingTime, 9 * 60f, 0);
            CheckTrigger(remainingTime, 7.5f * 60f, 1);
            CheckTrigger(remainingTime, 5.5f * 60f, 2);
            CheckTrigger(remainingTime, 3.5f * 60f, 3);
        }

        void CheckTrigger(float remainingTime, float triggerTime, int index)
        {
            if (triggered[index]) return;

            if (remainingTime <= triggerTime)
            {
                triggered[index] = true;
                SpawnExtractionPoint();
            }
        }

        void SpawnExtractionPoint()
        {
            if (extractionRoutine != null)
                StopCoroutine(extractionRoutine);

            extractionRoutine = StartCoroutine(ExtractionRoutine());
        }

        IEnumerator ExtractionRoutine()
        {
            // 기존 탈출 지점 비활성화
            if (currentExtractionPoint != null)
            {
                currentExtractionPoint.SetActive(false);
            }

            currentExtractionPoint = GetRandomExtractionPoint();

            if (currentExtractionPoint == null)
                yield break;

            currentExtractionPoint.SetActive(true);
            notificationUI?.ShowMessage("Extraction Point Activated!!!");

            yield return new WaitForSeconds(extractionDuration);

            currentExtractionPoint.SetActive(false);
            notificationUI?.ShowMessage("Extraction Point Deactivated!!!");

            currentExtractionPoint = null;
        }

        GameObject GetRandomExtractionPoint()
        {
            if (extractionPoints == null || extractionPoints.Count == 0)
            {
                Debug.LogWarning("[Extraction] No extraction points assigned.");
                return null;
            }

            if (extractionPoints.Count == 1)
                return extractionPoints[0];

            GameObject selected;

            do
            {
                selected = extractionPoints[
                    Random.Range(0, extractionPoints.Count)
                ];
            }
            while (selected == currentExtractionPoint);

            return selected;
        }
    }
}