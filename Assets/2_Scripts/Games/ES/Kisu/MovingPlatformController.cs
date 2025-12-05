using LUP.ES;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class MovingPlatformController : MonoBehaviour, IInteractable
    {
        [Header("설정")]
        public float interactionDuration = 3f;
        public MovingPlatform platform;

        [Header("상호작용")]
        public bool isInteracting { get; private set; } = false;
        public float currentInteractionTime { get; private set; } = 0f;

        [Header("참조")]
        private InteractionUIController interactionUI;

        public bool InterruptsOnMove => true;
        public bool CanInteract() => !isInteracting;

        void Start()
        {
            interactionUI = GetComponent<InteractionUIController>();
            //HideInteractionPrompt();
        }

        public void Interact()
        {
            // 상호작용 시작, 타이머 UI 표시
            isInteracting = true;
            currentInteractionTime = interactionDuration;
            ShowInteractionTimerUI();
            Debug.Log("엘리베이터 상호작용 시작!");
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (!isInteracting)
            {
                Interact();
                return false;
            }

            currentInteractionTime -= deltaTime;
            interactionUI.UpdateInteractionTimerUI(interactionDuration, currentInteractionTime);

            if (currentInteractionTime <= 0f)
            {
                isInteracting = false;
                HideInteractionTimerUI();

                // 타이머 완료 시 실제 엘리베이터 이동
                if (platform != null)
                    platform.StartMove();

                return true;
            }

            return false;
        }

        public void ResetInteraction()
        {
            isInteracting = false;
            currentInteractionTime = 0f;
        }

        public void ShowInteractionPrompt()
        {
            if (!isInteracting)
                interactionUI.ShowInteractionPrompt();
        }

        public void HideInteractionPrompt()
        {
            interactionUI.HideInteractionPrompt();
        }

        public void ShowInteractionTimerUI()
        {
            interactionUI.ShowInteractionTimerUI();
        }

        public void HideInteractionTimerUI()
        {
            interactionUI.HideInteractionTimerUI();
        }
    }
}