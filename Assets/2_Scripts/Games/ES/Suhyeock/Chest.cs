using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace LUP.ES
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [Header("Open FX")] // 기수 추가한 코드
        public GameObject openFxPrefab; // 기수 추가한 코드
        public Transform fxAnchor;  // 기수 추가한 코드

        private GameObject fxInstance;  // 기수 추가한 코드
        private ParticleSystem[] fxSystems; // 기수 추가한 코드

        private EventBroker eventBroker;
        public ItemCenter itemCenter;
        private InteractionUIController InteractionUIController;
        private float currentTime = 0.0f;
        [SerializeField]
        private float interactionDuration = 5.0f;
        private bool isInteracted = false;
        private bool isInteracting = false;

        public bool InterruptsOnMove => true;  // 기수 추가한 코드

        private List<Item> dropItems = new List<Item>();
        public bool CanInteract() => !isInteracting;

        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            itemCenter = FindAnyObjectByType<ItemCenter>();
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

            StopOpenFX(); // 기수 추가한 코드

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

                PlayOpenFX(); // 기수 추가한 코드

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

            StopOpenFX(); // 기수 추가한 코드

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

        // 기수 추가한 코드
        void PlayOpenFX() 
        {
            if (fxInstance != null) return;

            fxInstance = Instantiate(openFxPrefab, fxAnchor.position, fxAnchor.rotation, fxAnchor);
            fxSystems = fxInstance.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in fxSystems)
            {
                var main = ps.main;
                main.loop = true;
                ps.Play();
            }
        }

        // 기수 추가한 코드
        void StopOpenFX()
        {
            if (fxSystems == null) return;

            foreach (var ps in fxSystems)
            {
                if (ps == null) continue;
                var main = ps.main;
                main.loop = false;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            Destroy(fxInstance, 2.0f);
            fxInstance = null;
            fxSystems = null;
        }
    }
}
