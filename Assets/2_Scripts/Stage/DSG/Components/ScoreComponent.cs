using System;
using UnityEngine;

namespace LUP.DSG
{
    public class ScoreComponent : MonoBehaviour
    {
        public float totalDamageDealt { get; private set; }
        public float totalDamageTaken { get; private set; }
        public float totalHealingDone { get; private set; }

        public void UpdateDamageDealt(float damage)
        {
            totalDamageDealt += damage;
        }
        public void UpdateDamageTaken(float damage)
        {
            totalDamageTaken += damage;
        }
        public void UpdateHealingDone(float damage)
        {
            totalHealingDone += damage;
        }
        public float CalculateMVPScore()
        {
            return totalDamageDealt + totalDamageTaken + totalHealingDone;
        }
    }
}