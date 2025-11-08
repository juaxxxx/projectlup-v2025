using LUP.DSG.Utils.Enums;

namespace LUP.DSG
{
    [System.Serializable]
    public class CharacterData
    {
        public int ID;

        public float maxHp;
        public string characterName;
        public EAttributeType type;
        public ERangeType rangeType;
        public float attack;
        public float defense;
        public float speed;
    }
}