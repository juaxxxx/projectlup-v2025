using System;
using TMPro;
using UnityEngine;
using DSG.Utils.Enums;
using static UnityEngine.UI.GridLayoutGroup;

namespace DSG
{
    public class BattleComponent : MonoBehaviour
    {
        private Character owner;

    public float currHp;

    public bool isAttacking = false;

    private LineupSlot targetSlot;
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

        public event Action<float> OnDamaged;
        public event Action<int> OnDie;

    void Start()
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
        if(isAttacking)
        {
            if (owner.characterData.rangeType == ERangeType.Melee)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.5f);
                if(transform.position == targetSlot.AttackedPosition.position)
                {
                    if (!impactApplied)
                    {
                        ApplyDamageOnce();
                        impactApplied = true;
                    }
                    targetPosition = originPosition;
                }
                else if (transform.position == originPosition)
                {
                    isAttacking = false;
                    impactApplied = false;
                }
            }
            else
            {
                if (bullet == null) return;

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

                    isAttacking = false;
                    impactApplied = false;
                    Destroy(bullet);
                    bullet = null;
                    return;
                }
                bullet.transform.position += dir.normalized * moveDistance;
            }
        }
    }

    public void SetHp(float hp)
    {
        currHp = hp;
    }

    public void Attack(LineupSlot target)
    {
        if (isAttacking) return;

        if (target == null)
            return;

        targetSlot = target;
        targetPosition = targetSlot.AttackedPosition.position;
        if (owner.characterData.rangeType == ERangeType.Range)
        {
            bullet = Instantiate(bulletPrefab, originPosition, Quaternion.identity);
        }

        isAttacking = true;
    }

    private void ApplyDamageOnce()
    {
        if (targetSlot == null)
            return;

        var targetChar = targetSlot.character;

        if (targetSlot == null || targetSlot.character == null || targetSlot.character.BattleComp == null)
            return;

        float damage = owner.characterData.attack;
        targetChar.BattleComp.TakeDamage(damage);
        owner.ScoreComp.UpdateDamageDealt(damage);
    }

    public virtual void TakeDamage(float amount)
    {
        if (!isAlive)
            return;

        currHp -= amount;
        //print($"{name} ������ ����");
        OnDamaged?.Invoke(currHp);
        owner.ScoreComp.UpdateDamageTaken(amount);
        TriggerKnockback();

            if (currHp <= 0)
            {
                currHp = 0;
                Die();
            }
        }
        public virtual void Die()
        {
            isAlive = false;
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
    }
}