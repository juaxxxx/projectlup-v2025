using UnityEngine;
namespace ST
{
    // 게임 결과 데이터 (static으로 씬 간 전달)
    public static class GameResult
    {
        public static bool IsVictory { get; set; }
        public static int TotalKills { get; set; }
        public static float PlayTime
        {
            get

                ; set;
        }
        public static int WaveCleared { get; set; }

        // 나중에 추가할 데이터
        public static int Experience { get; set; }
        public static int Gold { get; set; }

        public static void Reset()
        {
            IsVictory = false;
            TotalKills = 0;
            PlayTime = 0f;
            WaveCleared = 0;
            Experience = 0;
            Gold = 0;
        }
    }
}