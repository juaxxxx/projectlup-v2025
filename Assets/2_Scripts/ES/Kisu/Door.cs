using LUP.ES;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LUP.ES
{
    public class Door : MonoBehaviour, IInteractable
    {
        [Header("설정")]
        [SerializeField] private float OpenDoorTime = 1.5f;

        [Header("참조")]
        public EventBroker eventBroker;
        private InteractionUIController interactionUI;

        public bool isOpening;
        public bool IsOpening() { return isOpening; }

        public bool isOpened = false;
        private float currentTime = 0.0f;
        public bool InterruptsOnMove => true;
        public bool CanInteract() => !isOpening && !isOpened;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            interactionUI = GetComponent<InteractionUIController>();
            HideInteractionPrompt();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Interact()
        {
            HideInteractionPrompt();
            HideInteractionTimerUI();

            Debug.Log("문 열림!!!");
            isOpened = true;

            // 0.1초 뒤 삭제 (UI/상호작용 시스템 업데이트 시간 확보)
            this.gameObject.SetActive(false);
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (!isOpening)
            {
                isOpening = true;
                currentTime = OpenDoorTime;

                ShowInteractionTimerUI();
                HideInteractionPrompt();

                return false;
            }

            currentTime -= deltaTime;

            interactionUI.UpdateInteractionTimerUI(OpenDoorTime, currentTime);
            if (currentTime < 0.0f)
            {
                Interact();
                return true;
            }
            return false;
        }

        public void ResetInteraction()
        {
            isOpening = false;
            currentTime = 0.0f;
        }

        public void ShowInteractionPrompt()
        {
            if(!isOpened)
            {
                interactionUI.ShowInteractionPrompt();
            } 
        }
        public void HideInteractionPrompt()
        {
            interactionUI.HideInteractionPrompt();
        }

        public  void ShowInteractionTimerUI()
        {
            interactionUI.ShowInteractionTimerUI();
        }

        public void HideInteractionTimerUI()
        {
            interactionUI.HideInteractionTimerUI();
        }
    }
}
