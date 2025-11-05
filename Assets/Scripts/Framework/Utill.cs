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

    public enum StageType // 임시로 일단 이걸 사용해봤음
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum RoguelikeStageKind : int
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum ShootingStageKind : int
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum DeckStrategyStageKind : int
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum ExtractionShooterStageKind : int
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum ProductionStageKind : int
    {
        Lobby = 1,
        InGame = 2,
        Result = 3
    }

    public enum RuntimeDataType
    {
        RoguelikeRuntime,
        ShootingRuntime,
        DeckStrategyRuntime,
        ExtractionShooterRuntime,
        ProductionRuntime
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
                RuntimeDataType.ProductionRuntime => "production_runtime.json"
            };
        }

    }


}

