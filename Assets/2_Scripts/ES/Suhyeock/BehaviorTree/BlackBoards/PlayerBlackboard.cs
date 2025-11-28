using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public enum MoveState
    {
        IDLE,
        MOVING,
    }


    public class PlayerBlackboard : BaseBlackboard
    {
        public float InteractionRadius = 2.0f;
        public FixedJoystick leftJoystick;
        public FixedJoystick rightJoystick;
        public LayerMask InteractableLayer;
        public EventBroker eventBroker;
        public Weapon weapon;
        public InteractionDetector InteractionDetector;

        [HideInInspector]
        public MoveState moveState = MoveState.IDLE;
        [HideInInspector]
        public IInteractable interactingObject = null;
        [HideInInspector]
        public bool isInteractionButtonPressed = false;
        [HideInInspector]
        public bool isReloadButtonPressed = false;

        [HideInInspector]
        public PlayerOverheadUI playerOverheadUI;

        public void ResetInteractionState()
        {
            //isCastingInteraction = false;
            interactingObject = null;
        }
    }
}


