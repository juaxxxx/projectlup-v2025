using LUP.DSG.Utils.Enums;
using System;
using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class AnimationComponent : MonoBehaviour
    {
        private Character owner;

        public Animator animator;

        public event Action OnHitAttack;
        public event Action OnShootRangeAttack;
        public event Action OnAttackStart;

        public EAnimStateType currentState { get; private set; }

        private BattleCameraDirector battleCameraDirector;

        void Start()
        {
            owner = GetComponent<Character>();
            currentState = EAnimStateType.Idle;

            if (!battleCameraDirector)
            {
                Camera camera = Camera.main;
                battleCameraDirector = camera.GetComponent<BattleCameraDirector>();
            }
        }

        public void StartDashAnimation()
        {
            currentState = EAnimStateType.StartDash_Fwd;
            SetAnimationState(currentState);
        }

        public void StartAttackAnimation(EWeaponType weaponType)
        {
            switch (weaponType)
            {
                case EWeaponType.Melee_OneHanded:
                    currentState = EAnimStateType.Attack_Melee_OneHanded;
                    break;
                case EWeaponType.Melee_TwoHanded:
                    currentState = EAnimStateType.Attack_Melee_TwoHanded;
                    break;
                case EWeaponType.Gun_Rifle:
                    currentState = EAnimStateType.Attack_Range_Rifle;
                    break;
                case EWeaponType.Magic:
                    currentState = EAnimStateType.Attack_Range_Magic;
                    break;
                case EWeaponType.Throw:
                    currentState = EAnimStateType.Attack_Range_Throw;
                    break;
                default:
                    break;
            }

            SetAnimationState(currentState);
        }

        public void StartMeleeAnimation()
        {
            SetAnimationState(currentState);
        }

        public void EndMeleeAnimation()
        {
            currentState = EAnimStateType.StartDash_Bwd;
            SetAnimationState(currentState);
            battleCameraDirector.BackToOriginPos();
        }

        public void PlayHittedAnimation(float damage)
        {
            currentState = EAnimStateType.Hitted;
            SetAnimationState(currentState);

            owner.ActioneffectPool.PlayVFX(ActionEffect.GetHitBasic,transform.position, Quaternion.identity,1.0f); //@TODO 캐릭터마다 애니메이션을 갖고있을지 생각해봐야됨
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

        public void OnHitMeleeAttackEvent()
        {
            OnHitAttack?.Invoke();
            OnPunchEffect();
        }

        public void OnShootRangeAttackEvent()
        {
            OnShootRangeAttack?.Invoke();

            owner.ActioneffectPool.PlayVFX(ActionEffect.AttackBasic, transform.position, Quaternion.identity, 1.0f);
        }

        public void OnAttackEndEvent()
        {
            currentState = EAnimStateType.Idle;
            EndMeleeAnimation();
        }

        public void OnEndFwdDashEvent()
        {
            OnAttackStart?.Invoke();
        }

        public void OnEndBwdDashEvent()
        {
            currentState = EAnimStateType.Idle;
            SetAnimationState(currentState);
        }

        public void OnHittedEndEvent()
        {
            currentState = EAnimStateType.Idle;
            SetAnimationState(currentState);

        }

        public void OnPunchEffect()
        {
            owner.ActioneffectPool.PlayVFX(ActionEffect.AttackBasic, transform.position, Quaternion.identity, 1.0f);
        }
    }
}