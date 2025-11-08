using LUP.DSG.Utils.Enums;
using System;
using UnityEngine;

namespace LUP.DSG
{
    public class AnimationComponent : MonoBehaviour
    {
        public Animator animator;

        public event Action OnHitAttack;
        public event Action OnAttackEnd;
        public event Action OnEndFwdDash;
        public event Action OnEndBwdDash;

        public EAnimStateType currentState { get; private set; }

        void Start()
        {
            currentState = EAnimStateType.Idle;
        }

        public void StartAttackAnimation(ERangeType type)
        {
            if (type == ERangeType.Range)
            {
                currentState = EAnimStateType.Attack_Range;
            }
            else
            {
                currentState = EAnimStateType.StartDash_Fwd;
            }
            SetAnimationState(currentState);
        }

        public void EndDashLoop(bool attackEnded)
        {
            if (attackEnded)
            {
                currentState = EAnimStateType.EndDash_Bwd;
            }
            else
            {
                currentState = EAnimStateType.EndDash_Fwd;
            }
            SetAnimationState(currentState);
        }

        public void StartMeleeAnimation()
        {
            currentState = EAnimStateType.Attack_Melee;
            SetAnimationState(currentState);
        }

        public void EndMeleeAnimation()
        {
            currentState = EAnimStateType.StartDash_Bwd;
            SetAnimationState(currentState);
        }

        public void PlayHittedAnimation(float damage)
        {
            currentState = EAnimStateType.Hitted;
            SetAnimationState(currentState);
        }

        public void PlayDiedAnimation(int index)
        {
            currentState = EAnimStateType.Died;
            SetAnimationState(currentState);
        }

        private void SetAnimationState(EAnimStateType type)
        {
            animator.SetInteger("CharacterState", (int)type);
        }

        public void OnHitAttackEvent()
        {
            OnHitAttack?.Invoke();
        }

        public void OnAttackEndEvent()
        {
            currentState = EAnimStateType.Idle;
            EndMeleeAnimation();
            //OnAttackEnd?.Invoke();
        }

        public void OnEndFwdDashEvent()
        {
            StartMeleeAnimation();
        }

        public void OnEndBwdDashEvent()
        {
            currentState = EAnimStateType.Idle;
        }
    }
}