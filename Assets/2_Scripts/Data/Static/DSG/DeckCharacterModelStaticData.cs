[System.Serializable]
public class DeckCharacterModelStaticData
{
    [Column("ModelId")] public int ModelId;
    [Column("ModelName")] public string ModelName;
    [Column("ModelPath")] public string ModelPath;
    [Column("Material")] public int Material;
}