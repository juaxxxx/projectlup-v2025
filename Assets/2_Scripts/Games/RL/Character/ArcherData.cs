using UnityEngine;

public class RunTimeData
{
    public BaseStats intrinscData;
    public BaseStats currentData;
    public int xp;
    public int level;


    public RunTimeData(RLCharacterData data)
    {
        intrinscData = data.stats;
        currentData = data.stats;
        xp = 0;
        level = 1;
    }
}


