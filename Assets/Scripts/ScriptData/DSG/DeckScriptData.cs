[System.Serializable]
public class DeckScriptData
{
    [Column("name")] public string name;
    [Column("description")] public string description;
    [Column("stat")] public string stat;
    [Column("gold")] public int gold;
}
