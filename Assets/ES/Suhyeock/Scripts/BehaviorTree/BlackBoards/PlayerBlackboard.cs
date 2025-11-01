using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveState
{
    IDLE,
    MOVING,
}


public class PlayerBlackboard : MonoBehaviour
{
    public float HP = 0.0f;
    public float MaxHP = 100.0f;
    public float speed = 5.0f;
    public float InteractionRadius = 2.0f; 
    public FixedJoystick leftJoystick;
    public FixedJoystick rightJoystick;
    public LayerMask InteractableLayer;
    public EventBroker eventBroker;
    public Gun gun;

    [HideInInspector]
    public MoveState moveState = MoveState.IDLE;
    //[HideInInspector]
    //public AttackState attackState = AttackState.NONE;
    [HideInInspector]
    public IInteractable interactingObject = null;
    [HideInInspector]
    public bool isInteractionButtonPressed = false;
    [HideInInspector]
    public bool isReloadButtonPressed = false;
    //public bool isCastingInteraction = false;
    [HideInInspector] 
    public bool isHit = false;
    [HideInInspector]
    public bool isDead = false;

    [HideInInspector]
    public PlayerOverheadUI playerOverheadUI;
    [HideInInspector]
    public InteractionDetector InteractionDetector;
    public void ResetInteractionState()
    {
        //isCastingInteraction = false;
        interactingObject = null;
    }
}
