using UnityEngine;

[CreateAssetMenu(menuName = "LUPData/UpgradeTable")]
public class UpgradeTable : ScriptableObject
{
    public LevelRow[] rows;
    [System.Serializable]
    public struct LevelRow
    {
        public int hp;
        public int powerConsume;
        public CostEntry[] cost;
    }
}
