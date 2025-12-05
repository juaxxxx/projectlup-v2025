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

                // 필요하면 플레이어 입력 막기 등 추가 가능
                // Time.timeScale = 0; // 게임 멈추고 싶으면
            }
        }
    }
}