using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace LUP.ES
{
    public class ExtractionPoint : MonoBehaviour, IInteractable
    {
        [Header("설정")]
        [SerializeField] private float extractionTime = 10.0f;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color idleColor = Color.red;

        [Header("참조")]
        public EventBroker eventBroker;
        private InteractionUIController interactionUI;
        private Renderer rend;
        
        // 기수 추가한 코드
        public bool InterruptsOnMove => false;

        public bool isExtracting;
        public bool IsExtracting() {  return isExtracting; }

        public bool isExtracted = false;
        private float currentTime = 0.0f;

        public TextMeshProUGUI timerText;

        public bool CanInteract() => !isExtracting && !isExtracted;
        
        void Start()
        {
            interactionUI = GetComponent<InteractionUIController>();
            rend = GetComponent<Renderer>();
            rend.material.color = idleColor;

            HideTimerTextObject();
        }

        public void Interact()
        {
            rend.material.color = activeColor;

            HideTimerTextObject();

            Debug.Log("탈출 성공!");
            isExtracted = true;

            if (eventBroker != null)
            {
                eventBroker.OnExtractionSuccess();
            }
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (!isExtracting)
            {
                isExtracting = true;
                currentTime = extractionTime;

                ShowTimerTextObject();
                HideInteractionPrompt();
                UpdateInteractionTimerText(currentTime);

                return false;
            }

            currentTime -= deltaTime;
            UpdateInteractionTimerText(currentTime);

            if (currentTime <= 0.0f)
            {
                Interact();
                return true;
            }
            return false;
        }

        public void ResetInteraction()
        {
            isExtracting = false;
            currentTime = 0.0f;

            HideTimerTextObject();
        }

        public void ShowInteractionPrompt()
        {
            if(!isExtracted)
            {
                interactionUI.ShowInteractionPrompt();
            }
        }
        public void HideInteractionPrompt()
        {
            interactionUI.HideInteractionPrompt();
        }
    
        public void ShowInteractionTimerUI() { }

        public void HideInteractionTimerUI() { }
     

        public void UpdateInteractionTimerText(float remainTime)
        {
            if(timerText != null)
            {
                timerText.text = remainTime.ToString("F1");
            } 
        }
        public void ShowTimerTextObject()
        {
            if (timerText != null && timerText.gameObject != null)
            {
                timerText.gameObject.SetActive(true);
            }
        }
        public void HideTimerTextObject()
        {
            if (timerText != null && timerText.gameObject != null)
            {
                timerText.gameObject.SetActive(false);
            }
        }
    }
}