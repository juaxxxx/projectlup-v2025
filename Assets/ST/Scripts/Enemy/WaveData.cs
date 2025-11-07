using UnityEngine;
namespace ST
{

    [System.Serializable]
    public class WaveData
    {
        public string waveName = "Wave 1";
        public int monsterCount = 5;           // 이 웨이브에서 스폰할 몬스터 수
        public float spawnInterval = 2f;       // 몬스터 스폰 간격 (초)
        public float delayBeforeNextWave = 5f; // 다음 웨이브까지 대기 시간
    }
}