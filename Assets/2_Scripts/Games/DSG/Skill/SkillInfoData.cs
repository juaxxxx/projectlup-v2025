using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    [CreateAssetMenu(fileName = "Skill Info Data", menuName = "DSG/Scriptable Objects/Skill Info")]
    public class SkillInfoData : ScriptableObject
    {
        public string Skillname;

        public int targetCount;
        public Vector3 AttackPosition;
        public bool bIsDamaged;
        public float damage;

        public bool bIsStatusEffect;
        public EStatusEffectType effectType;
        public EOperationType operationType;
        public int stack;
        public int turn;

        public ActionEffect attackEffect;
        public ActionEffect gethitEffect;
    }
}