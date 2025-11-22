[System.Serializable]
public class DeckCharacterStaticData
{
    [Column("CharacterId")] public int CharacterId;
    [Column("CharacterName")] public string CharacterName;
    [Column("AttributeType")] public int AttributeType;
    [Column("RangeType")] public int RangeType;
}
