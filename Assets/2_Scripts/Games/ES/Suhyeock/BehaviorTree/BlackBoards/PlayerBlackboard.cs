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
        public EventBroker eventBroker;
        public int CurrentWeaponID = 2;
        [HideInInspector]
        public Weapon weapon;
        public InteractionDetector InteractionDetector;
        [HideInInspector]
        public Animator animator;
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
        [HideInInspector]
        public WeaponEquip weaponEquip;
        public void ResetInteractionState()
        {
            //isCastingInteraction = false;
            interactingObject = null;
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
        }
    }
}


