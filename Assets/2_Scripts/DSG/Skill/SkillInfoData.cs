using LUP.DSG.Utils.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Info Data", menuName = "Scriptable Objects/Skill Info")]
public class SkillInfoData : ScriptableObject
{
    public int targetCount;
    public EStatusEffectType effectType;
    public EOperationType operationType;
}
