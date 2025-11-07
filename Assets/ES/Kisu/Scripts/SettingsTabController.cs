using UnityEngine;
using UnityEngine.UI;

namespace ES
{
    public class SettingsTabController : MonoBehaviour
    {
        [Header("패널들")]
        public GameObject[] panels;

        [Header("탭 버튼들")]
        public Button[] buttons;

        void Start()
        {
            // 버튼 클릭 이벤트 연결
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => ShowPanel(index));
            }

            ShowPanel(0); // 초기 활성 패널
        }

        public void ShowPanel(int index)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(i == index);
                buttons[i].interactable = i != index;
            }
        }
    }
}


