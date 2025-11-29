using LUP.DSG.Utils.Enums;
using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
using System.Runtime.CompilerServices;
using System.Linq;

namespace LUP.DSG
{
    public class BattleComponent : MonoBehaviour
    {
        private Character owner;

        public float currHp;
        public float maxSkillGauge { get; private set; }
        public float currGauge = 0f;

        public bool isAttacking = false;
        public bool isUsingSkill = false;

        private LineupSlot targetSlot;
        private List<LineupSlot> SkillTargetSlot;
        private Vector3 originPosition;
        private Vector3 targetPosition;

        [SerializeField]
        private GameObject bulletPrefab;

        private GameObject bullet;
        private float bulletSpeed = 0.8f;

        private bool impactApplied = false;

        private Vector3 knockbackTarget;
        private float knockbackDistance = 0.4f;
        private float knockbackDuration = 0.2f;
        private float knockbackTimer = 0f;
        private bool isKnockback = false;

        public bool isAlive { get; private set; } = true;
        public bool isSkillOn { get; private set; } = false;

        public SkillInfoData skillInfo;

        public event Action<float> OnDamaged;
        public event Action<int> OnDie;
        public event Action<float> OnChangeGauge;

        public event Action<ERangeType> OnAttackStarted;
        public event Action<bool> OnReachedTargetPos;

        [SerializeField]
        private GameObject damageLogPrefab;

        private void Awake()
        {
            owner = GetComponent<Character>();
            originPosition = owner.gameObject.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isKnockback)
            {
                knockbackTimer += Time.deltaTime;
                float t = knockbackTimer / knockbackDuration;

                transform.position = Vector3.Lerp(knockbackTarget, originPosition, t);

                if (knockbackTimer >= knockbackDuration)
                {
                    isKnockback = false;
                    transform.position = originPosition;

                    if (!isAlive)
                    {
                        owner.ClearCharacterInfo();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (owner.AnimationComp.currentState == EAnimStateType.StartDash_Fwd)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.5f);
                if (Vector3.Distance(transform.position,targetSlot.AttackedPosition.position) < 0.01f)
                {
                    if (!impactApplied)
                    {
                        OnReachedTargetPos?.Invoke(false);

                        targetPosition = originPosition;
                        impactApplied = true;
                    }
                }
                else if (isUsingSkill && Vector3.Distance(transform.position, skillInfo.AttackPosition) < 0.01f)
                {
                    if (!impactApplied)
                    {
                        OnReachedTargetPos?.Invoke(false);

                        targetPosition = originPosition;
                        impactApplied = true;
                    }
                }
            }
            else if (owner.AnimationComp.currentState == EAnimStateType.StartDash_Bwd)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.5f);
                if (Vector3.Distance(transform.position, originPosition) < 0.01f)
                {
                    impactApplied = false;
                    isUsingSkill = false;
                    isAttacking = false;
                    OnReachedTargetPos?.Invoke(true);
                }
            }
            else if (bullet != null)
            {
                Vector3 dir = targetPosition - bullet.transform.position;
                float distanceToTarget = dir.magnitude;
                float moveDistance = bulletSpeed;

                if (distanceToTarget <= moveDistance)
                {
                    bullet.transform.position = targetPosition;

                    if (!impactApplied)
                    {
                        ApplyDamageOnce();
                        impactApplied = true;
                    }

                    StartCoroutine(WaitForRangeAttackEnd());
                    impactApplied = false;
                    Destroy(bullet);
                    bullet = null;
                    return;
                }
                else
                {
                    bullet.transform.position += dir.normalized * moveDistance;
                }
            }
        }

        private IEnumerator WaitForRangeAttackEnd()
        {
            yield return new WaitForSecondsRealtime(1f);

            isAttacking = false;
        }

        public void SetHp(float hp)
        {
            currHp = hp;
        }

        public void SetMaxGauge(float gauge)
        {
            maxSkillGauge = gauge;
        }

        public void Attack(LineupSlot target)
        {
            //if (isAttacking) return;
            if (owner.AnimationComp.currentState != EAnimStateType.Idle) return;

            if (target == null)
                return;

            targetSlot = target;
            targetPosition = targetSlot.AttackedPosition.position;
            HandleAttackStart();

            isAttacking = true;
        }
        public void ApplyDamageOnce()
        {
            if (isUsingSkill)
            {
                ApplySkillDamage();
                return;
            }

            if (targetSlot == null)
                return;

            var targetChar = targetSlot.character;

            if (targetSlot == null || targetSlot.character == null || targetSlot.character.BattleComp == null)
                return;

            float damage = owner.characterData.attack;

            targetChar.BattleComp.TakeDamage(1);
            owner.ScoreComp.UpdateDamageDealt(damage);

            PlusGuage(50);
        }
        public void ApplySkillDamage()
        {
            if (SkillTargetSlot == null || SkillTargetSlot.Count <= 0)
                return;

            for (int i = 0; i < SkillTargetSlot.Count; i++)
            {
                if (skillInfo.bIsDamaged)
                {
                    float damage = owner.characterData.attack + skillInfo.damage;
                    SkillTargetSlot[i].character.BattleComp.TakeDamage(1);
                    owner.ScoreComp.UpdateDamageDealt(damage);
                }

                if (skillInfo.bIsStatusEffect)
                {
                    IStatusEffect Status = owner.StatusEffectComp.CreateStatusEffect(skillInfo.effectType, skillInfo.operationType, skillInfo.stack, skillInfo.turn);
                    SkillTargetSlot[i].character.StatusEffectComp.AddEffect(Status);
                }
            }

            InitGuage();
        }

        public virtual void TakeDamage(float amount)
        {
            if (!isAlive)
                return;

            currHp -= amount;

            OnDamaged?.Invoke(currHp);
            owner.ScoreComp.UpdateDamageTaken(amount);
            TriggerKnockback();

            if (damageLogPrefab != null)
            {
                Vector3 headPos = transform.position + Vector3.up * 0.8f;
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);
                GameObject log = Instantiate(damageLogPrefab, headPos, rot);
                log.GetComponent<DamageLog>()?.Setup(amount);
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var shaker = mainCam.GetComponent<CameraShake>();
                if (shaker == null)
                    shaker = mainCam.gameObject.AddComponent<CameraShake>();

                shaker.StartCoroutine(shaker.Shake(0.2f, 0.2f));
            }

            FindFirstObjectByType<HitVignetteEffect>()?.PlayDamageEffect();

            if (currHp <= 0)
            {
                currHp = 0;
                Die();
            }
        }

        public void Skill(List<LineupSlot> targetList)
        {
            if (isAttacking || isUsingSkill) return;

            SkillTargetSlot = targetList;
            targetPosition = skillInfo.AttackPosition;
            HandleAttackStart();

            isUsingSkill = true;
            isAttacking = true;
        }
        private void HandleAttackStart()
        {
            OnAttackStarted?.Invoke(owner.characterData.rangeType);
        }
        public void TrySpawnProjectileForRangedAttack()
        {
            if (owner.characterData.rangeType != ERangeType.Range)
                return;

            bullet = Instantiate(bulletPrefab, originPosition, Quaternion.identity);
        }

        public virtual void Die()
        {
            isAlive = false;
            if (owner != null && owner.characterData != null && owner.characterModelData != null && owner.ScoreComp != null)
            {
                float score = owner.ScoreComp.CalculateMVPScore();
                Color color = owner.characterModelData.material.GetColor("_BaseColor");
                BattleSystem.Instance?.BackupDeadCharacter(owner.characterData.characterName, color, score, owner.characterModelData.prefab);
            }
            OnDie?.Invoke(owner.battleIndex);
        }
        private void TriggerKnockback()
        {
            if (isKnockback) return;

            originPosition = transform.position;

            float dirX = owner.isEnemy ? 1f : -1f;
            knockbackTarget = originPosition + new Vector3(dirX * knockbackDistance, 0f, 0f);

            knockbackTimer = 0f;
            isKnockback = true;
        }

        private void OnDisable()
        {
            OnDamaged = null;
            OnDie = null;
        }

        public void PlusGuage(float amount)
        {
            currGauge = Mathf.Min(maxSkillGauge, currGauge + amount);

            if (currGauge >= maxSkillGauge)
            {
                isSkillOn = true;   // "다음 턴에 스킬 쓸 수 있음"
            }

            OnChangeGauge?.Invoke(currGauge);
        }

        private void InitGuage()
        {
            maxSkillGauge = 100;
            currGauge = 0;
            isSkillOn = false;
            OnChangeGauge?.Invoke(currGauge);
        }
    }
}