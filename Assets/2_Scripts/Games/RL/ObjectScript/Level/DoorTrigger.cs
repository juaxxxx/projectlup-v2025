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
                if (stageCenter == null)
                {
                    return;
                }
                if(stageCenter.IsCurrentRoomCleared() == true)
                {

                    stageCenter.LoadNextRoom();
                }
                else
                {
                    Debug.Log("몬스터남아있음");
                }

            }
        }
    }
}