using UnityEngine;
namespace LUP.RL
{


    public class RouletteTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject roulettePanel;  // Canvas 안의 RoulletPanel

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("충돌");
            // 플레이어 태그 체크
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어");
                // UI 패널 활성화
                roulettePanel.SetActive(true);
            }
        }
    }
}