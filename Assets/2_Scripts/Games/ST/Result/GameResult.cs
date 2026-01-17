using System.Collections.Generic;

namespace LUP.ST
{
    /// <summary>
    /// GameScene → ResultScene 결과 데이터 전달용 정적 클래스
    /// </summary>
    public static class GameResult
    {
        // 게임 결과
        public static bool IsVictory { get; set; } = true;
        public static int TotalKills { get; set; } = 0;
        public static float PlayTime { get; set; } = 0f;
        public static int WaveCleared { get; set; } = 0;

        // 난이도 배율 (플레이어 레벨 총합 기반)
        public static float DifficultyMultiplier { get; set; } = 1f;

        // 기본 보상 설정
        public static int BaseExpReward { get; set; } = 100;
        public static int BaseGoldReward { get; set; } = 200;

        // 출전 캐릭터 ID 목록
        public static List<int> ParticipatingCharacterIds { get; set; } = new List<int>();

        /// <summary>
        /// 게임 시작 시 초기화
        /// </summary>
        public static void Reset()
        {
            IsVictory = true;
            TotalKills = 0;
            PlayTime = 0f;
            WaveCleared = 0;
            DifficultyMultiplier = 1f;
            BaseExpReward = 100;
            BaseGoldReward = 200;
            ParticipatingCharacterIds.Clear();
        }

        /// <summary>
        /// 총 경험치 계산 (기본 * 배율)
        /// 각 캐릭터에게 동일하게 지급
        /// </summary>
        public static int CalculateTotalExp()
        {
            return (int)(BaseExpReward * DifficultyMultiplier);
        }

        /// <summary>
        /// 총 골드 계산 (기본 * 배율)
        /// </summary>
        public static int CalculateTotalGold()
        {
            return (int)(BaseGoldReward * DifficultyMultiplier);
        }
    }
}