[System.Serializable]
public class RoguelikeScriptData
{
    [Column("DataType")] public string DataType;
    [Column("ID")] public string ID;
    [Column("Name")] public string Name;
    [Column("HP")] public int HP;
    [Column("ATK")] public string ATK;
    [Column("SPEED")] public string SPEED;
    [Column("ChapterMaxRoomNum")] public string ChapterMaxRoomNum;
}
              