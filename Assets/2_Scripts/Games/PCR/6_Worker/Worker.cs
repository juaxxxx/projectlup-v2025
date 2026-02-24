using LUP.RL;
using UnityEngine;

namespace LUP.PCR
{

    // RequireComponent(typeof(Animator))]
    public class Worker : MonoBehaviour
    {
        private Animator anim;
        private UnitMover mover;

        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int IsClimbingHash = Animator.StringToHash("IsClimbing");
        private void Awake()
        {
            mover = GetComponent<UnitMover>();
        }

        private void Update()
        {
            //@TODO : 행동트리에서 이동할 때만 호출하게 하기
            UpdateAnimationState();
        }

        public void InitAnimator()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void UpdateAnimationState()
        {
            if (mover == null || anim == null)
            {
                return;
            }

            anim.SetBool(IsClimbingHash, mover.IsClimbing);
            anim.SetBool(IsMovingHash, mover.IsMoving && !mover.IsClimbing);
        }

        /*
         public void TriggerWork()
        {
            anim.SetTrigger("Work");
        }
         */
    }


}
