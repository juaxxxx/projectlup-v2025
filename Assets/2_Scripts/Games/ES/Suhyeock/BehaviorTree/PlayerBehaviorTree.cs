using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace LUP.ES
{
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
            
        }
        void Start()
        {
            blackboard.weaponEquip = GetComponent<WeaponEquip>();
            blackboard.weaponEquip.Init();
            SetupBehaviorTree();
            
        }

        // Update is called once per frame
        void Update()
        {
            rootNode.Evaluate();
        }
        private void SetupBehaviorTree()
        {
            //1. 사망
            DeadCondition deadCondition = new DeadCondition(blackboard);
            DeathAction deathAction = new DeathAction(blackboard);

            Sequence handleDeathSequence = new Sequence(new List<BTNode> {
                deadCondition,
                deathAction,
            });

            //2. 피격시 행동
            CastingInteractionCondition castingInteractionCondition = new CastingInteractionCondition(blackboard);

            HitCondition hitCondition = new HitCondition(blackboard);
            HitAction hitAction = new HitAction(blackboard);
            AbortCastingInteractionAction abortCastingInteractionAction = new AbortCastingInteractionAction(blackboard);

            Sequence handleHitSequence = new Sequence(new List<BTNode> { hitCondition, hitAction, castingInteractionCondition, abortCastingInteractionAction });

            //3. 상호작용
            InteractionButtonPressedCondition interactionButtonPressedCondition = new InteractionButtonPressedCondition(blackboard);
            TryInteractAction tryInteractAction = new TryInteractAction(blackboard, characterController);
            CastingInteractionAction castingInteractionAction = new CastingInteractionAction(blackboard);
            Sequence InteractionSequence = new Sequence(new List<BTNode> {
                interactionButtonPressedCondition ,
                tryInteractAction,
                castingInteractionAction,
            });

         
            //4. 재장전, 공격, 이동
            AttackingCondition attackingCondition = new AttackingCondition(blackboard);
            AttackAction attackAction = new AttackAction(blackboard, characterController);
            Sequence handleAttackSequence = new Sequence(new List<BTNode> { attackingCondition, attackAction });
           
            ReloadCondition reloadCondition = new ReloadCondition(blackboard);
            ReloadAction reloadAction = new ReloadAction(blackboard);
            Sequence handleReloadSequence = new Sequence(new List<BTNode> { reloadCondition, reloadAction});
            
            Selector handleActionsSelector = new Selector(new List<BTNode> { handleReloadSequence, handleAttackSequence });
       
            MovingCondition movingCondition = new MovingCondition(blackboard);
            MoveAction moveAction = new MoveAction(blackboard, characterController);
            UpdateAimDirectionAction updateAimDirectionAction = new UpdateAimDirectionAction(blackboard);
            Parallel combatParallel = new Parallel(new List<BTNode> { updateAimDirectionAction, handleActionsSelector, movingCondition, moveAction });


            rootNode = new Selector(new List<BTNode>
            {
                handleDeathSequence,
                handleHitSequence,
                InteractionSequence ,
                combatParallel,
            });
        }


    }
}
