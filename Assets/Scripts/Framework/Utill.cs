using UnityEngine;

namespace Define
{
    /// <summary>
    /// Resource
    /// </summary>
    public enum VideoResourceType
    {
        Sample,
    }
    public enum SoundBGMResourceType
    {
        Sample,
    }
    public enum SoundSFXResourceType
    {
        Sample,
    }

    /// <summary>
    /// Stage
    /// </summary>
    public enum StageKind
    {
        Unknown = 0,
        Debug = 1,      // 디버그 씬 (개발용)
        Main = 2,       // 메인 화면
        Intro = 3,      // 인트로
        RL = 4,  // 로그라이크
        ST = 5,   // 슈팅
        ES = 6, // 익스트랙션 슈터
        PCR = 7,  // 생산/건설/강화
        DSG = 8, // 덱 전략
        Tutorial=9,
    }
    public enum RuntimeDataType
    {
        RoguelikeRuntime,
        ShootingRuntime,
        DeckStrategyRuntime,
        ExtractionShooterRuntime,
        ProductionRuntime,
        Versions
    }

    public static class RuntimeDataTypes
    {
        public static string ToFilename(this RuntimeDataType type)
        {
            return type switch
            {
                RuntimeDataType.RoguelikeRuntime => "roguelike_runtime.json",
                RuntimeDataType.ShootingRuntime => "shooting_runtime.json",
                RuntimeDataType.DeckStrategyRuntime => "deckstrategy_runtime.json",
                RuntimeDataType.ExtractionShooterRuntime => "extractionshooter_runtime.json",
                RuntimeDataType.ProductionRuntime => "production_runtime.json",
                RuntimeDataType.Versions => "Versions.json"
            };
        }

    }

    /// <summary>
    /// Inventory System
    /// </summary>
    public enum ItemType
    {
        None = 0,
        Weapon = 1,      // 무기
        Armor = 2,       // 방어구
        Consumable = 3,  // 소비 아이템
        Material = 4,    // 재료
        KeyItem = 5,     // 퀘스트/키 아이템
        Currency = 6,    // 화폐
        Equipment = 7,   // 장비 (일반)
        Buff = 8,        // 버프 아이템
        Character = 9,   // 캐릭터 (덱 빌딩용)
        Custom = 100     // 게임별 커스텀 (100 이상 사용)
    }


}

