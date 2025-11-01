[System.Serializable]
public class OwnedCharacterInfo
{
    public int characterID;
    public int characterModelID;
    public int characterLevel;

    public OwnedCharacterInfo(int characterId, int characterModelId, int level)
    {
        characterID = characterId;
        characterModelID = characterModelId;
        characterLevel = level;
    }
}