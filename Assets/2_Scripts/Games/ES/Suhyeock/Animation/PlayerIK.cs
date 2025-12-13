using UnityEngine;

namespace LUP.ES
{
    public class PlayerIK : MonoBehaviour
    {
        private Animator animator;
        private PlayerBlackboard blackboard;
        public float leftHandWeight = 1.0f;
        public float maxWeight = 1.0f;

        private float currentLeftHandWeight = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            blackboard = GetComponentInParent<PlayerBlackboard>(); // 블랙보드 위치에 따라 수정 필요
        }

        void OnAnimatorIK(int layerIndex)
        {
            if (animator == null || blackboard.weapon == null) return;

            float targetWeight = maxWeight;

            bool shouldDetachHand = (blackboard.weapon.state == WeaponState.RELOADING) ||
                                (blackboard.healthComponent.isDead) ||
                                (blackboard.weapon.leftHandGrip == null);

            if (shouldDetachHand)
            {
                targetWeight = 0f;
            }
            currentLeftHandWeight = Mathf.Lerp(currentLeftHandWeight, targetWeight, Time.deltaTime * 10.0f);

            // 현재 무기에 왼손 그립 위치가 설정되어 있다면
            if (blackboard.weapon.leftHandGrip != null)
            {
                // 1. 왼손 IK 가중치 설정 (위치, 회전 따로 설정 가능)
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, currentLeftHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, currentLeftHandWeight);

                // 2. 왼손을 무기의 'LeftHandGrip' 위치로 이동 및 회전
                animator.SetIKPosition(AvatarIKGoal.LeftHand, blackboard.weapon.leftHandGrip.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, blackboard.weapon.leftHandGrip.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}

