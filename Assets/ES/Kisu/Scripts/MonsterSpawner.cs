using UnityEngine;

namespace ES
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject MonsterPrefab;
        [SerializeField] private Transform[] SpawnPoints;

        void Start()
        {
            SpawnAllMonsters();
        }
        void Update()
        {

        }

        void SpawnAllMonsters()
        {
            foreach (Transform point in SpawnPoints)
            {
                Instantiate(MonsterPrefab, point.position, point.rotation);
            }
        }
    }
}