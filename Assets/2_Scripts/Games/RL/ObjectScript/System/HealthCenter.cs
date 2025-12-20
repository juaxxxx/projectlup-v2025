using LUP.ST;
using System;
using UnityEngine;
namespace LUP.RL
{
    public class HealthCenter
    {

        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        public event Action<int, int> OnHpChanged;  // current, max
        public event Action OnDamaged;
        public event Action OnDead;
        private Hpbar hpbar;

        public HealthCenter(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void Damage(int amount)
        {
            Debug.Log("healcenter :  µ¥¹ÌÁö");
            CurrentHp -= amount;
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
            if (CurrentHp <= 0)
            {

                CurrentHp = 0;
                OnDead?.Invoke();

            }
        }
        public void Heal(int amount)
        {

            CurrentHp += amount;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
        }

    }
}