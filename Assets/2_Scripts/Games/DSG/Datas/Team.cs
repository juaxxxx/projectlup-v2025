namespace LUP.DSG
{
    [System.Serializable]
    public class Team
    {
        private const int maxSize = 5;

        public OwnedCharacterInfo[] characters = new OwnedCharacterInfo[maxSize];
    }
}