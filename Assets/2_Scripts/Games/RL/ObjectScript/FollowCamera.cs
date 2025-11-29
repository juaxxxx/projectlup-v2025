using UnityEngine;

namespace LUP.RL
{
    public class FollowCamera : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private BaseRoom room;
        private PlayerMove player;

        public float L_ROffset = 0.0f;
        public float U_DOffset = 0.0f;

        private Vector3 LeftBound;
        private Vector3 RightBound;
        private Vector3 UpBound;
        private Vector3 DownBound;

        private float ViewportZOffset;

        void Start()
        {
            player = FindFirstObjectByType<PlayerMove>();
            room = FindFirstObjectByType<BaseRoom>();

            if (player == null || room == null)
            {
                Debug.LogWarning("Fail To Find player or BaseRoom(RoomCenter)");
                return;
            }

            ViewportZOffset = player.gameObject.transform.position.z - this.gameObject.transform.position.z;

            if (room)
            {
                LeftBound = room.leftWall.WorldPosition;
                RightBound = room.rightWall.WorldPosition;
                UpBound = (room.frontLeftWall.WorldPosition + room.frontRightWall.WorldPosition) / 2;
                DownBound = room.backWall.WorldPosition;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 playerPosition = player.gameObject.transform.position;
            Vector3 followedPosition = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z - ViewportZOffset);

            if (CheckInBound())
                this.transform.position = followedPosition;
        }

        bool CheckInBound()
        {
            if (LeftBound.x < this.transform.position.x &&
                RightBound.x > this.transform.position.x &&
                DownBound.z - ViewportZOffset < this.transform.position.z &&
                UpBound.z + ViewportZOffset > this.transform.position.z)
                return true;

            return false;
        }
    }

}
