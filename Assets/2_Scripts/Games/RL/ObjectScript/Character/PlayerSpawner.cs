using LUP.ST;
using UnityEngine;
using UnityEngine.InputSystem;
namespace LUP.RL
{
    public class PlayerSpawner : MonoBehaviour
    {
        public JoyStickSC joystick;
        private GridGenerator grid;
        public Vector2Int fixedSpawnGrid;
        private GameObject playerInstance;
        [Header("캐릭터 종류별 프리팹")]
        [SerializeField] private GameObject ArcherPrefab;
        //[SerializeField] private GameObject Warrior;
        void Start()
        {
            grid = FindFirstObjectByType<GridGenerator>();
            //grid = GridGenerator.Instance;
        }
        public GameObject playerSpawn()
        {
            Debug.Log("spawn");

            Vector3 spawnPos = GetGridWorldPos(fixedSpawnGrid);
            playerInstance = Instantiate(ArcherPrefab, spawnPos, Quaternion.identity);

            var move = playerInstance.GetComponent<PlayerMove>();

            // 플레이어 세팅 먼저
            JoyStickSC.Instance.SetPlayer(move);
            InGameCenter.Instance.RegisterPlayer(playerInstance);

            // 카메라 세팅을 마지막에
            FollowCamera cam = FindFirstObjectByType<FollowCamera>();
            cam.SetTarget(move); 
            cam.FindTarget();
            return playerInstance;
        }

  
        private Vector3 GetGridWorldPos(Vector2Int gridPos)
        {
            if (grid == null)
            {
                Debug.LogError("GridGenerator is NULL!");
                return Vector3.zero;
            }

            var tile = grid.GetTile(gridPos.x, gridPos.y);
            if (tile == null)
            {
                Debug.LogError($"Tile is NULL at grid pos: {gridPos}");
                return Vector3.zero;
            }

            return tile.worldPos;
        }
    }
}
