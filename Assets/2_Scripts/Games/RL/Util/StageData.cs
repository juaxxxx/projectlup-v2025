using UnityEngine;
using System.Collections.Generic;
namespace LUP.RL
{
    [CreateAssetMenu(fileName = "StageData", menuName = "RLGame/StageData")]
    public class StageData : ScriptableObject
    {
        [SerializeField]
        public string StageName;
        [Header("·ë ÇÁ¸®ÆÕ")]
        public GameObject roomprefab;

        public Vector2Int playerSpawnPoint;

        public List<EnemySpawnEntry> enemySpawn;

        public List<Vector2Int> obstacles;
    }

}