using UnityEngine;

namespace LUP.ES
{
    public interface IInteractable
    {
        bool InterruptsOnMove { get; } // 기수 추가한 코드

        bool TryStartInteraction(float deltaTime);
        bool CanInteract();
        void Interact();
        void ResetInteraction();

        void ShowInteractionPrompt();
        void HideInteractionPrompt();

        void ShowInteractionTimerUI();

        void HideInteractionTimerUI();  
    }
}
