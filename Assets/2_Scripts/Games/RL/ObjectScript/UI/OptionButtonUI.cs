using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LUP.RL
{
    public class OptionButtonUI : MonoBehaviour
    {
        [SerializeField] private Image buffIcon;  // 버튼 안의 이미지
        [SerializeField] private TMP_Text buffNameText;  // 버튼 안의 이름 텍스트

        private Button button;
        private BuffData buffData;
        private Archer archer;
        private PlayerBuff playerBuff;
        private BuffSelectionUI selectionUI;
        private void Awake()
        {
            ActivateAllComponents();
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }

        }
        void Update()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                GetComponent<RectTransform>(), Input.mousePosition, null))
            {

            }
        }
        private void ActivateAllComponents()
        {
            // 오브젝트 자체 켜기
            gameObject.SetActive(true);

            // 자식 오브젝트 전부 켜기
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);

            // 모든 Behaviour 컴포넌트 활성
            var behaviours = GetComponentsInChildren<Behaviour>(true);
            foreach (var comp in behaviours)
                comp.enabled = true;

        }



        public void SetData(BuffData data, PlayerBuff buff, BuffSelectionUI ui)
        {
            buffData = data; //어떤버프인지 
            playerBuff = buff; //누구한테 적용할지.
            selectionUI = ui;
            if (buffIcon != null)
            {
                buffIcon.sprite = buffData.GetDisplayableImage();//이미지 갖고오기
                buffIcon.enabled = true;              // 컴포넌트  활성화
                buffIcon.gameObject.SetActive(true);  // 오브젝트도 활성
            }


        }

        public void OnClick()
        {
            if (playerBuff != null && buffData != null)
            {
                Debug.Log("클릭");
                playerBuff.ApplyBuff(buffData);
                selectionUI.Close();
            }

            //if (archer != null && buffData != null)
            //{
            //    archer.ApplyBuff(buffData);
            //}

        }

    }
}