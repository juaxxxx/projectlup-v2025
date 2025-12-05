[System.Serializable]
public class DeckCharacterAnimationStaticData
{
    [Column("TableId")] public int TableId;
    [Column("ModelId")] public int ModelId;
    [Column("AnimationId")] public int AnimationId;
    [Column("AnimationName")] public string AnimationName;
    [Column("AnimationPath")] public string AnimationPath;
    
}