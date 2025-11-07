using UnityEngine;

namespace ES
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject Player;
        [SerializeField] private Transform[] SpawnPoints;

        void Start()
        {
            SpawnPlayer();
        }

        private void Update()
        {

        }

        void SpawnPlayer()
        {

            int randomIndex = Random.Range(0, SpawnPoints.Length);

            Player.transform.position = SpawnPoints[randomIndex].position;
            Player.transform.rotation = SpawnPoints[randomIndex].rotation;
        }
    }
}

