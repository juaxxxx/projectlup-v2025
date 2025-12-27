namespace LUP.ST
{
    public static class LobbyTeamCache
    {
        private static STCharacterData[] cachedTeam5;

        public static bool HasTeam => cachedTeam5 != null && cachedTeam5.Length == 5;

        public static STCharacterData[] GetCopy()
        {
            if (!HasTeam) return null;
            var copy = new STCharacterData[5];
            System.Array.Copy(cachedTeam5, copy, 5);
            return copy;
        }

        public static void Save(STCharacterData[] team5)
        {
            if (team5 == null || team5.Length < 5) return;
            cachedTeam5 = new STCharacterData[5];
            System.Array.Copy(team5, cachedTeam5, 5);
        }

        public static void Clear()
        {
            cachedTeam5 = null;
        }
    }
}



