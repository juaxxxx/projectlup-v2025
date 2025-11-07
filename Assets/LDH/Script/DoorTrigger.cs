using System.Runtime.CompilerServices;
using UnityEngine;
namespace RL
{


    public class DoorTrigger : MonoBehaviour
    {
        private bool colider = false;
        void OnCollisionEnter(Collision collision)
        {
            if (colider) return;
            if (collision.gameObject.CompareTag("Player"))
            {
                colider = true;

                StageController stageManager = FindAnyObjectByType<StageController>();
                if (stageManager != null)
                {

                    stageManager.LoadNextRoom();
                }

            }
        }
    }
}