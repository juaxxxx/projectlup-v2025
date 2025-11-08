[System.Serializable]
public class DeckStaticData
{
    [Column("TableId")] public int tableId;
    [Column("CharacterId")] public int characterId;
    [Column("Level")] public int level;
    [Column("HP")] public float hp;
    [Column("Attack")] public float attack;
    [Column("Defense")] public float defense;
    [Column("Speed")] public float speed;
}
