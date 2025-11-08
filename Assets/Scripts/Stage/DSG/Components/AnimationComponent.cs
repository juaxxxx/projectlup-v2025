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
                animator.SetTrigger("rangeAttack");
            }
            else
            {
                animator.SetTrigger("startMelee");
            }
        }

        public void EndDashLoop(bool attackEnded)
        {
            if (attackEnded)
            {
                animator.SetTrigger("reachedOriginPos");
            }
            else
            {
                animator.SetTrigger("reachedAttackPos");
            }
        }

        public void StartMeleeAnimation()
        {
            animator.SetTrigger("meleeAttack");
        }

        public void EndMeleeAnimation()
        {
            animator.SetTrigger("endMelee");
        }

        public void PlayHittedAnimation(float damage)
        {
            animator.SetTrigger("hitted");
        }

        public void PlayDiedAnimation(int index)
        {
            animator.SetTrigger("died");
        }

        public void OnHitAttackEvent()
        {
            OnHitAttack?.Invoke();
        }
    }
}