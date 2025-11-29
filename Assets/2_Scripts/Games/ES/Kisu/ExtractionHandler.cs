using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LUP.ES
{
    public class ExtractionHandler : MonoBehaviour
    {
        [Header("참조")]
        public InteractionDetector detector;
        public PlayerBlackboard playerBlackboard;

        [Header("설정")]
        public Button interactButton;

        private bool isTimerRunning = false;

        private ExtractionPoint currentExtractionTarget = null;
        private IInteractable nearestInteractable = null;

        void Start()
        {
            if (interactButton != null)
            {
                interactButton.onClick.AddListener(HandleButtonPress);
            }
        }
        void Update()
        {
            if (isTimerRunning && currentExtractionTarget != null)
            {
                HandleRunningExtraction();
            }
        }

        private void HandleButtonPress()
        {
            nearestInteractable = detector.GetNearestInteractable();

            if (isTimerRunning)
            {
                return;
            }
            else if (nearestInteractable is ExtractionPoint detectedPoint)
            {
                currentExtractionTarget = detectedPoint;
                isTimerRunning = true;
                currentExtractionTarget.TryStartInteraction(Time.deltaTime);
            }
        }

        private void HandleRunningExtraction()
        { 
            IInteractable currentlyNearest = detector.GetNearestInteractable();
            
            if (!detector.IsObjectNearby(currentExtractionTarget))
            {
                Debug.Log("탈출 영역 이탈 감지: 상호작용 자동 취소");
                CancelCurrentExtraction();
                return;
            }

            bool completed = currentExtractionTarget.TryStartInteraction(Time.deltaTime);

            if (completed)
            {
                isTimerRunning = false;
                currentExtractionTarget = null;
            }
        }

        private void CancelCurrentExtraction()
        {
            if (currentExtractionTarget != null)
            {
                currentExtractionTarget.ResetInteraction();
                currentExtractionTarget = null;
            }
            isTimerRunning = false;

            if (playerBlackboard != null)
            {
                playerBlackboard.ResetInteractionState();
            }
        }
    }
}