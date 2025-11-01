using UnityEngine;

public interface IInteractable
{
    bool TryStartInteraction(float deltaTime);
    bool CanInteract();
    void Interact();
    void ResetInteraction();

    void ShowInteractionPrompt();
    void HideInteractionPrompt();

    void ShowInteractionTimerUI();

    void HideInteractionTimerUI();
}
