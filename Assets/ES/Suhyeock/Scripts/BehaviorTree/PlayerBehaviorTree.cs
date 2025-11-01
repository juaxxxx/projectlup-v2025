using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorTree : MonoBehaviour
{   
    private BTNode rootNode;
    private PlayerBlackboard blackboard;
    private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        blackboard = GetComponent<PlayerBlackboard>();
        blackboard.playerOverheadUI = GetComponent<PlayerOverheadUI>();
        blackboard.InteractionDetector = GetComponent<InteractionDetector>();
        blackboard.HP = blackboard.MaxHP;
    }
    void Start()
    {
        
        SetupBehaviorTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (blackboard.isDead == true)
            return;
        rootNode.Evaluate();
    }
    private void SetupBehaviorTree()
    {
        DeadCondition deadCondition = new DeadCondition(blackboard);
        DeathAction deathAction = new DeathAction(blackboard);

        Sequence checkIsDeadSequence = new Sequence(new List<BTNode> {
            deadCondition,
            deathAction,
        });
        CastingInteractionCondition castingInteractionCondition = new CastingInteractionCondition(blackboard);

        HitCondition hitCondition = new HitCondition(blackboard);
        HitAction hitAction = new HitAction(blackboard);
        AbortCastingInteractionAction abortCastingInteractionAction = new AbortCastingInteractionAction(blackboard);

        Sequence handleHitSequence = new Sequence(new List<BTNode> { hitCondition, hitAction, castingInteractionCondition, abortCastingInteractionAction });

        MovingCondition movingCondition = new MovingCondition(blackboard);
        AttackingCondition attackingCondition = new AttackingCondition(blackboard);
        Selector movingOrAttackingSelector = new Selector(new List<BTNode> { movingCondition, attackingCondition });
        

        Sequence abortIfCastingInteractionSequence= new Sequence(new List<BTNode> {
            castingInteractionCondition,
            abortCastingInteractionAction,
        });

        Succeeder AbortIfCastingInteractionSucceeder = new Succeeder(abortIfCastingInteractionSequence);
        
        ReloadCondition reloadCondition = new ReloadCondition(blackboard);
        ReloadAction reloadAction = new ReloadAction(blackboard);
        Sequence handleReloadSequence = new Sequence(new List<BTNode> { reloadCondition, reloadAction});

        Failer handleReloadSequenceFailer = new Failer(handleReloadSequence);
        FireAction fireAction = new FireAction(blackboard, characterController);

        Selector handleGunActionsSelector = new Selector(new List<BTNode> { reloadCondition, fireAction});
       
        MoveAction moveAction = new MoveAction(blackboard, characterController);
        
        Parallel fireAndMoveParallel = new Parallel(new List<BTNode> { handleGunActionsSelector, moveAction });


        Sequence handleMovementAndAttackSequence = new Sequence(new List<BTNode> {
            movingOrAttackingSelector,
            AbortIfCastingInteractionSucceeder,
            fireAndMoveParallel
        });

        
        InteractionButtonPressedCondition interactionButtonPressedCondition = new InteractionButtonPressedCondition(blackboard);
        TryInteractAction tryInteractAction = new TryInteractAction(blackboard, characterController);
        Sequence tryInitiateInteractionSequence = new Sequence(new List<BTNode> {
            interactionButtonPressedCondition ,
            tryInteractAction
        });

        CastingInteractionAction castingInteractionAction = new CastingInteractionAction(blackboard);
        Sequence performInteractionCastingSequence = new Sequence(new List<BTNode> {
            castingInteractionCondition,
            castingInteractionAction
        });

        rootNode = new Selector(new List<BTNode>
        {
            checkIsDeadSequence,
            handleHitSequence,
            handleReloadSequenceFailer,
            handleMovementAndAttackSequence,
            tryInitiateInteractionSequence ,
            performInteractionCastingSequence,
        });
    }
}
