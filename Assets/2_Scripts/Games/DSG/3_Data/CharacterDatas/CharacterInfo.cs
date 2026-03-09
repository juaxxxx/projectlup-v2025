
namespace LUP.DSG
{
    [System.Serializable]
    public class CharacterInfo
    {
        public int characterID;
        public int characterModelID;
        public int characterLevel;

        public CharacterInfo(int characterId, int characterModelId, int level)
        {
            characterID = characterId;
            characterModelID = characterModelId;
            characterLevel = level;
        }
    }
}