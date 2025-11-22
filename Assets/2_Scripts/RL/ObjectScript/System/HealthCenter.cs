using System;
using UnityEngine;
namespace LUP.RL
{
    public class HealthCenter
    {

        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        public event Action<int, int> OnHpChanged;  // current, max
        private Hpbar hpbar;

        public HealthCenter(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void Damage(int amount)
        {
            CurrentHp -= amount;
            if (CurrentHp < 0) CurrentHp = 0;

            OnHpChanged?.Invoke(CurrentHp, MaxHp);
        }
    }
}