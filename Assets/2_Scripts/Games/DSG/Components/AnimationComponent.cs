using LUP.DSG.Utils.Enums;
using System;
using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class AnimationComponent : MonoBehaviour
    {
        public Animator animator;

        public event Action OnHitAttack;
        public event Action OnShootRangeAttack;
        public event Action OnAttackStart;

        [SerializeField]
        private ParticleSystem HitFVX;

        [SerializeField]
        private ParticleSystem AttackFVX;

        public EAnimStateType currentState { get; private set; }

        void Start()
        {
            currentState = EAnimStateType.Idle;
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
        }

        public void PlayHittedAnimation(float damage)
        {
            currentState = EAnimStateType.Hitted;
            SetAnimationState(currentState);

            ParticleSystem go = Instantiate(HitFVX, transform.position, Quaternion.identity);
            go.Play();
            Destroy(go.gameObject, go.main.duration);
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

            ParticleSystem go = Instantiate(AttackFVX, transform.position, Quaternion.identity);
            go.Play();
            Destroy(go.gameObject, go.main.duration);
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
            ParticleSystem go = Instantiate(AttackFVX, transform.position, Quaternion.identity);
            go.Play();
            Destroy(go.gameObject, go.main.duration);
        }
    }
}