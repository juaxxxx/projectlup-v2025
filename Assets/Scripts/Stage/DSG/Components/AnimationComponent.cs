using LUP.DSG.Utils.Enums;
using System;
using UnityEngine;

namespace LUP.DSG
{
    public class AnimationComponent : MonoBehaviour
    {
        public Animator animator;

        public event Action OnHitAttack;

        void Start()
        {

        }

        public void StartAttackAnimation(ERangeType type)
        {
            if (type == ERangeType.Range)
            {
                SetAnimationState(EAnimStateType.Attack_Range);
            }
            else
            {
                SetAnimationState(EAnimStateType.StartDash_Fwd);
            }
        }

        public void EndDashLoop(bool attackEnded)
        {
            if (attackEnded)
            {
                SetAnimationState(EAnimStateType.EndDash_Bwd);
            }
            else
            {
                SetAnimationState(EAnimStateType.EndDash_Fwd);
            }
        }

        public void StartMeleeAnimation()
        {
            SetAnimationState(EAnimStateType.Attack_Melee);
        }

        public void EndMeleeAnimation()
        {
            SetAnimationState(EAnimStateType.StartDash_Bwd);
        }

        public void PlayHittedAnimation(float damage)
        {
            SetAnimationState(EAnimStateType.Hitted);
        }

        public void PlayDiedAnimation(int index)
        {
            SetAnimationState(EAnimStateType.Died);
        }

        private void SetAnimationState(EAnimStateType type)
        {
            animator.SetInteger("CharacterState", (int)type);
        }

        public void OnHitAttackEvent()
        {
            OnHitAttack?.Invoke();
        }
    }
}