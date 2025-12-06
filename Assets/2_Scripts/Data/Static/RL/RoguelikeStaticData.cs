[System.Serializable]
public class RoguelikeStaticData
{
    [Column("DataType")] public string DataType;
    [Column("ID")] public string ID;
    [Column("Name")] public string Name;
    [Column("HP")] public int HP;
    [Column("ATK")] public string ATK;
    [Column("MOVESPEED")] public string SPEED;
    [Column("ATKRANGE")] public string ATKRANGE;
    [Column("ATKCOLLTIMEDURATION")] public string ATKCOLLTIMEDURATION;
    [Column("ChapterMaxRoomNum")] public string ChapterMaxRoomNum;
    [Column("ChapterPreviewImageId")] public string ChapterPreviewImageId;
    [Column("EnemySpawnPoint")] public int EnemySpawner;
}
