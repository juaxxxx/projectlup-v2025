using UnityEngine;
using System.Collections.Generic;
namespace RL
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
    public class StageData : ScriptableObject
    {
        [SerializeField]
        public string StageName;
        public GameObject roomprefab;
        public Vector2Int playerSpawn;
        public List<Vector3Int> enemySpawn;
        public List<Vector2Int> obstacles;
    }

}