using UnityEngine;

public class ArcherData
{
    public BaseStats intrinscData;
    public BaseStats currentData;
    public int xp;
    public int level;


    public ArcherData(CharacterData data, int customHp, int CustomAttack)
    {
        intrinscData = data.stats;
        currentData = data.stats;
        currentData.Hp = customHp;
        currentData.Attack = CustomAttack;
        xp = 0;
         level = 1;
    }
}


