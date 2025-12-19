using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class InteractionDetector : MonoBehaviour
    {
        [HideInInspector]
        public EventBroker eventBroker;
        public PlayerBlackboard blackboard;
        private SphereCollider detectionCollider;
        private List<IInteractable> nearbyInteractables = new List<IInteractable>();

        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            detectionCollider = gameObject.AddComponent<SphereCollider>();
            detectionCollider.radius = blackboard.InteractionRadius;
            detectionCollider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IInteractable interactable))
            {
                interactable.ShowInteractionPrompt();
                nearbyInteractables.Add(interactable);
                Debug.Log("Count: " + nearbyInteractables.Count);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                eventBroker.CloseLootDisplay();
                //lootDisplayCenter.CloseLootPanel();
                interactable.HideInteractionPrompt();
                interactable.HideInteractionTimerUI();
                nearbyInteractables.Remove(interactable);
            }
        }
   
        public IInteractable GetNearestInteractable()
        {
            if (nearbyInteractables.Count == 0)
                return null;

            IInteractable nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < nearbyInteractables.Count; i++)
            {
                GameObject obj = (nearbyInteractables[i] as MonoBehaviour)?.gameObject;
                if (obj == null) continue;

                if (nearbyInteractables[i].CanInteract())
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = nearbyInteractables[i];
                    }
                }
            }
            return nearest;
        }

        // 기수 추가한 코드
        public bool IsObjectNearby(IInteractable target)
        {
            return nearbyInteractables.Contains(target);
        }
    }
}