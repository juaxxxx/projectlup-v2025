using UnityEngine;

namespace LUP.ES
{
    public class PlayerSpawner : MonoBehaviour
    {
        //[SerializeField] private GameObject Player;
        [SerializeField] private Transform[] SpawnPoints;

        public Transform GetPlayerSpawnPoint()
        {

            int randomIndex = Random.Range(0, SpawnPoints.Length);

            return SpawnPoints[randomIndex];
        }
    }
}

