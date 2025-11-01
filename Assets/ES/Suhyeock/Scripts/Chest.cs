using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public EventBroker eventBroker;
    public ItemCenter itemCenter;
    private InteractionUIController InteractionUIController;
    private float currentTime = 0.0f;
    [SerializeField]
    private float interactionDuration = 5.0f;
    private bool isInteracted = false;
    private bool isInteracting = false;

    private List<Item> dropItems = new List<Item>();
    public bool CanInteract() => !isInteracting;

    private void Start()
    {
        InteractionUIController = GetComponent<InteractionUIController>();
    }
    public void Interact()
    {
        Debug.Log("Interacted");
        ResetInteraction();
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
        if (isInteracted == false)
        {
            dropItems = itemCenter.GenerateLoot();
        }
        eventBroker.OpenLootDisplay(dropItems);
        eventBroker.HandleIventoryVisibility(true);
        HideInteractionTimerUI();
        ShowInteractionPrompt();
        isInteracted = true;
    }

    public bool TryStartInteraction(float deltaTime)
    {
        if(!isInteracting)
        {
            if (isInteracted)
            {
                Interact();
                return true;
            }
            isInteracting = true;
            currentTime = interactionDuration;
            return false;
        }

        currentTime -= deltaTime;
        InteractionUIController.UpdateInteractionTimerUI(interactionDuration, currentTime);

        if (currentTime < 0.0f)
        {
            Interact();
            
            return true;
        }

        return false;
    }

    public void ResetInteraction()
    {
        isInteracting = false;
        currentTime = 0.0f;
    }

    public void ShowInteractionPrompt()
    {
        InteractionUIController.ShowInteractionPrompt();
    }

    public void HideInteractionPrompt()
    {
        InteractionUIController.HideInteractionPrompt();
    }

    public void ShowInteractionTimerUI()
    {
        InteractionUIController.ShowInteractionTimerUI();
    }

    public void HideInteractionTimerUI()
    {
        InteractionUIController.HideInteractionTimerUI();
    }
}
