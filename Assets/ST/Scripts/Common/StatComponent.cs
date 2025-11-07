using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace ST
{

    public class StatComponent : MonoBehaviour, IDamageable
    {
        [Header("기본 스탯")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float defense = 0f;
        [SerializeField] private float moveSpeed = 5f;

        [Header("전투 스탯")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float bulletSpeed = 500f;
        [SerializeField] private float criticalChance = 0f;
        [SerializeField] private float criticalDamage = 1.5f;

        [Header("버프 설정")]
        [SerializeField] private float buffDuration = 10f;

        [Header("근접 스탯")]
        [SerializeField] private float maxMoveDistance = 5f;

        // 공격 상태 추적
        [HideInInspector] public bool isAttacking = false;
        [HideInInspector] public float attackStartTime = -999f;
        [HideInInspector] public float lastAttackEndTime = -999f;

        // 버프 추적 (중첩 방지)
        private Dictionary<string, Coroutine> activeBuffs = new Dictionary<string, Coroutine>();
        private Dictionary<string, float> buffMultipliers = new Dictionary<string, float>();

        // 이벤트
        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        // 프로퍼티
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public float AttackDamage => attackDamage;
        public float Defense => defense;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;
        public float AttackSpeed => attackSpeed;
        public float AttackCooldown => attackCooldown;
        public float BulletSpeed => bulletSpeed;
        public float MaxMoveDistance => maxMoveDistance;
        public bool IsDead => currentHealth <= 0;
        public bool IsAttacking => isAttacking;

        void Awake()
        {
            currentHealth = maxHealth;
            activeBuffs = new Dictionary<string, Coroutine>();
            buffMultipliers = new Dictionary<string, float>();
        }

        // IDamageable 구현
        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            float actualDamage = Mathf.Max(damage - defense, 0);
            currentHealth -= actualDamage;
            currentHealth = Mathf.Max(currentHealth, 0);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (IsDead)
            {
                OnDeath?.Invoke();
            }
        }

        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        bool IDamageable.IsDead() => IsDead;

        // 체력 회복
        public void Heal(float amount)
        {
            if (IsDead) return;
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        // 체력 퍼센트 회복
        public void HealPercent(float percent)
        {
            float healAmount = maxHealth * percent;
            Heal(healAmount);
            Debug.Log($"{gameObject.name} 체력 {percent * 100}% 회복: +{healAmount:F1}");
        }

        // 공격 관련 함수
        public bool CanStartAttack()
        {
            return !isAttacking && (Time.time - lastAttackEndTime >= attackCooldown);
        }

        public void StartAttack()
        {
            if (!CanStartAttack()) return;
            isAttacking = true;
            attackStartTime = Time.time;
        }

        public AttackState UpdateAttack()
        {
            if (!isAttacking) return AttackState.None;

            float elapsedTime = Time.time - attackStartTime;

            if (elapsedTime >= attackSpeed * 0.5f && elapsedTime < attackSpeed * 0.6f)
            {
                return AttackState.Hit;
            }

            if (elapsedTime >= attackSpeed)
            {
                EndAttack();
                return AttackState.End;
            }

            return AttackState.InProgress;
        }

        private void EndAttack()
        {
            isAttacking = false;
            lastAttackEndTime = Time.time;
        }

        public void CancelAttack()
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
        }

        public float CalculateDamage()
        {
            if (UnityEngine.Random.value < criticalChance)
            {
                return attackDamage * criticalDamage;
            }
            return attackDamage;
        }

        // ===== 버프 시스템
        public void MultiplyAttackDamage(float multiplier)
        {
            string buffKey = "AttackDamage";

            // 이미 버프가 있으면 시간만 연장
            if (activeBuffs.ContainsKey(buffKey))
            {
                StopCoroutine(activeBuffs[buffKey]);
                Debug.Log($"{gameObject.name} 공격력 버프 시간 연장! (+{buffDuration}초)");
            }
            else
            {
                // 처음 버프 적용
                float oldValue = attackDamage;
                attackDamage *= multiplier;
                buffMultipliers[buffKey] = multiplier;
                Debug.Log($"{gameObject.name} 공격력: {oldValue:F1} → {attackDamage:F1} ({multiplier}배, {buffDuration}초)");
            }

            // 새로운 타이머 시작
            activeBuffs[buffKey] = StartCoroutine(RemoveBuff(buffKey, () =>
            {
                attackDamage /= buffMultipliers[buffKey];
                Debug.Log($"{gameObject.name} 공격력 버프 종료: {attackDamage:F1}");
                activeBuffs.Remove(buffKey);
                buffMultipliers.Remove(buffKey);
            }));
        }

        public void MultiplyDefense(float multiplier)
        {
            string buffKey = "Defense";

            if (activeBuffs.ContainsKey(buffKey))
            {
                StopCoroutine(activeBuffs[buffKey]);
                Debug.Log($"{gameObject.name} 방어력 버프 시간 연장! (+{buffDuration}초)");
            }
            else
            {
                float oldValue = defense;
                defense *= multiplier;
                buffMultipliers[buffKey] = multiplier;
                Debug.Log($"{gameObject.name} 방어력: {oldValue:F1} → {defense:F1} ({multiplier}배, {buffDuration}초)");
            }

            activeBuffs[buffKey] = StartCoroutine(RemoveBuff(buffKey, () =>
            {
                defense /= buffMultipliers[buffKey];
                Debug.Log($"{gameObject.name} 방어력 버프 종료: {defense:F1}");
                activeBuffs.Remove(buffKey);
                buffMultipliers.Remove(buffKey);
            }));
        }

        public void MultiplyAttackSpeed(float multiplier)
        {
            string buffKey = "AttackSpeed";

            if (activeBuffs.ContainsKey(buffKey))
            {
                StopCoroutine(activeBuffs[buffKey]);
                Debug.Log($"{gameObject.name} 공격속도 버프 시간 연장! (+{buffDuration}초)");
            }
            else
            {
                float oldCooldown = attackCooldown;
                attackSpeed /= multiplier;
                attackCooldown /= multiplier;
                attackSpeed = Mathf.Max(attackSpeed, 0.05f);
                attackCooldown = Mathf.Max(attackCooldown, 0.05f);
                buffMultipliers[buffKey] = multiplier;
                Debug.Log($"{gameObject.name} 공격속도 {multiplier}배! (쿨타임: {oldCooldown:F2}초 → {attackCooldown:F2}초, {buffDuration}초)");
            }

            activeBuffs[buffKey] = StartCoroutine(RemoveBuff(buffKey, () =>
            {
                attackSpeed *= buffMultipliers[buffKey];
                attackCooldown *= buffMultipliers[buffKey];
                Debug.Log($"{gameObject.name} 공격속도 버프 종료 (쿨타임: {attackCooldown:F2}초)");
                activeBuffs.Remove(buffKey);
                buffMultipliers.Remove(buffKey);
            }));
        }

        public void MultiplyMoveSpeed(float multiplier)
        {
            string buffKey = "MoveSpeed";

            if (activeBuffs.ContainsKey(buffKey))
            {
                StopCoroutine(activeBuffs[buffKey]);
                Debug.Log($"{gameObject.name} 이동속도 버프 시간 연장! (+{buffDuration}초)");
            }
            else
            {
                float oldValue = moveSpeed;
                moveSpeed *= multiplier;
                buffMultipliers[buffKey] = multiplier;
                Debug.Log($"{gameObject.name} 이동속도: {oldValue:F1} → {moveSpeed:F1} ({multiplier}배, {buffDuration}초)");
            }

            activeBuffs[buffKey] = StartCoroutine(RemoveBuff(buffKey, () =>
            {
                moveSpeed /= buffMultipliers[buffKey];
                Debug.Log($"{gameObject.name} 이동속도 버프 종료: {moveSpeed:F1}");
                activeBuffs.Remove(buffKey);
                buffMultipliers.Remove(buffKey);
            }));
        }

        private IEnumerator RemoveBuff(string buffKey, Action onRemove)
        {
            yield return new WaitForSeconds(buffDuration);
            onRemove?.Invoke();
        }

        public void ResetStats()
        {
            currentHealth = maxHealth;
            isAttacking = false;
            attackStartTime = -999f;
            lastAttackEndTime = -999f;
        }
    }

}