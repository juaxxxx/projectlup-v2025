using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Character;

public class HealthComponent : MonoBehaviour, IBattleable
{
    private Character owner;

    public float currHp;
    private Vector3 originalPosition;
    private Vector3 knockbackTarget;
    private float knockbackDistance = 0.4f;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer = 0f;
    private bool isKnockback = false;

    public bool isAlive { get; private set; } = true;

    public event Action<float> OnDamaged;
    public event Action OnDie; 
    private void Start()
    {
        owner = GetComponent<Character>();
    }

    private void Update()
    {
        if (isKnockback)
        {
            knockbackTimer += Time.deltaTime;
            float t = knockbackTimer / knockbackDuration;

            transform.position = Vector3.Lerp(knockbackTarget, originalPosition, t);

            if (knockbackTimer >= knockbackDuration)
            {
                isKnockback = false;
                transform.position = originalPosition;

                if(!isAlive)
                {
                    owner.ClearCharacterInfo(); // ���ݹ��� ��� ������ Clear
                }
            }
        }
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

        if(currHp <= 0)
        {
            currHp = 0;
            Die();
        }
    }
    public virtual void Die()
    {
        isAlive = false;
        OnDie?.Invoke();
    }
    private void TriggerKnockback()
    {
        if (isKnockback) return;

        originalPosition = transform.position;

        float dirX = owner.isEnemy ? 1f : -1f;
        knockbackTarget = originalPosition + new Vector3(dirX * knockbackDistance, 0f, 0f);

        knockbackTimer = 0f;
        isKnockback = true;
    }

    void OnDisable()
    {
        OnDamaged = null; // ��� ������ ����
        OnDie = null;
    }
}