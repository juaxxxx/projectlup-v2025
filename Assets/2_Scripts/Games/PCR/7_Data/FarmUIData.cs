
namespace LUP.PCR
{
    public struct FarmUIData
    {
        public int level;
        public string buildingName;
        public int productionTime;
        public int curStorage;
        public int maxStorage;
        public int power;
        public bool isWorkRequested;
        public bool isConstructing;

        // 업그레이드 UI용 데이터
        public int currentLevel;
        public bool isMaxLevel;

        // 효과 (예: "저장 용량", 35, 5) -> "35 + 5"로 표시
        public string effectName;
        public int currentStatValue;
        public int nextStatAddedValue;   // 증가량

        // 비용 (최대 2개라고 가정, 필요시 리스트로 변경)
        public int costType1;
        public int costAmount1;
        public int costType2;
        public int costAmount2;


        public void SetData(int level, string buildingName, int productionTime, int curStorage, int maxStorage, int power
            , bool isWorkRequested, bool isConstructing)
        {
            this.level = level;
            this.buildingName = buildingName;
            this.curStorage = curStorage;
            this.maxStorage = maxStorage;
            this.power = power;
            this.isWorkRequested = isWorkRequested;
            this.isConstructing = isConstructing;
        }
    }
}

