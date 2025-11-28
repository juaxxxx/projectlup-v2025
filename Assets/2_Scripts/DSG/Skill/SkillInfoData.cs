using LUP.DSG.Utils.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Info Data", menuName = "Scriptable Objects/Skill Info")]
public class SkillInfoData : ScriptableObject
{
    public int targetCount;
    public Vector3 AttackPosition;
    public bool bIsDamaged;
    public float damage;

    public bool bIsStatusEffect;
    public EStatusEffectType effectType;
    public EOperationType operationType;
    public int stack;
    public int turn;
}
