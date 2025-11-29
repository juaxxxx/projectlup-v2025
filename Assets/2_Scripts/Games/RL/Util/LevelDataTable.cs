using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataTable", menuName = "Scriptable Objects/LevelDataTable")]
public class LevelDataTable : ScriptableObject
{
    public LevelData[] levelList;

    public LevelData  GetLevelData(int level)
    {
        if (level - 1 < 0 || level - 1 >= levelList.Length)
        {
            return null;
        }
        return levelList[level - 1];
    }


}
