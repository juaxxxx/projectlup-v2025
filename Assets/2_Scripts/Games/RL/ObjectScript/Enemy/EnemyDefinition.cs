using UnityEngine;
namespace LUP.RL
{
    [CreateAssetMenu(fileName = "EnemyData",menuName = "RLGame/EnemyData")]
    public class EnemyDefinition : ScriptableObject
    {
        public EnemyType type;
        public GameObject prefab;
    }
}
