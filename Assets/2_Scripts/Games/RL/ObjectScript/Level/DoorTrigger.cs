using System.Runtime.CompilerServices;
using UnityEngine;
namespace LUP.RL
{


    public class DoorTrigger : MonoBehaviour
    {
        
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StageController stageCenter = FindAnyObjectByType<StageController>();
                if (stageCenter != null && stageCenter.IsCurrentRoomCleared() == false)
                {
                    stageCenter.LoadNextRoom();
                }

            }
        }
    }
}