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
            Debug.Log("그리드할당");
            //grid = GridGenerator.Instance;
        }
        public GameObject Spawn()
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
            cam.SetTarget(move);   // 이게 Update보다 늦게 실행될 수 있음 → null 체크 필수

            return playerInstance;
        }

        //public void RepositionPlayer()
        //{
        //    if (playerInstance == null)
        //        return;

        //    Vector3 pos = GetGridWorldPos(fixedSpawnGrid);
        //    playerInstance.transform.position = pos;
        //}

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
