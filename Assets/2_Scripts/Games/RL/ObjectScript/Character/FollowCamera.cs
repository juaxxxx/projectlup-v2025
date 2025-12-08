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
            //FindTarget();
        }

        public void FindTarget()
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
            if (!player || !room)
                return;

            Vector3 playerPosition = player.gameObject.transform.position;
            Vector3 followedPosition = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z - ViewportZOffset);

            float clampedX = Mathf.Clamp(followedPosition.x, LeftBound.x + L_ROffset, RightBound.x - L_ROffset);
            float clampedZ = Mathf.Clamp(followedPosition.z, DownBound.z - ViewportZOffset + U_DOffset, UpBound.z + ViewportZOffset - U_DOffset);

            this.transform.position = new Vector3(clampedX, this.transform.position.y, clampedZ);
        }

        bool CheckInBound()
        {
            if (LeftBound.x + L_ROffset < this.transform.position.x &&
                RightBound.x - L_ROffset > this.transform.position.x &&
                DownBound.z - ViewportZOffset - U_DOffset < this.transform.position.z &&
                UpBound.z + ViewportZOffset + U_DOffset > this.transform.position.z)
                return true;

            return false;
        }
    }

}
