using System;
using System.Collections.Generic;

namespace LUP.ST
{
    [Serializable]
    public class CharacterLevelData
    {
        public int characterId;
        public int level = 1;
        public int currentExp = 0;
    }

    [Serializable]
    public class TeamSlotData
    {
        public int characterId;
        public int slotIndex;
    }

    [Serializable]
    public class STSaveDataContainer
    {
        public string filename = "shooting_runtime.json";
        public List<CharacterLevelData> characterList = new List<CharacterLevelData>();
        public List<TeamSlotData> currentTeam = new List<TeamSlotData>();
    }
}