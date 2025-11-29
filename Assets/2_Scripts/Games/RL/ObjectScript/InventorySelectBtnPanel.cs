using UnityEngine;
using Roguelike.Util;

namespace LUP.RL
{
    public class InventorySelectBtnPanel : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public InventorPanel inventorPanel;
        CircleButton[] selectionButtons;
        void Start()
        {
            selectionButtons = GetComponentsInChildren<CircleButton>();

            StartCoroutine(RoguelikeUtil.DelayOneFrame(() => BindButtonsFunction()));

        }

        void BindButtonsFunction()
        {
            for (int i = 0; i < selectionButtons.Length; i++)
            {
                int index = i;

                if (selectionButtons[i].button == null)
                    selectionButtons[i].ManualAwkae();

                selectionButtons[i].button.onClick.AddListener(() => OnBtnclicekd(index));
            }
        }

        void OnBtnclicekd(int index)
        {
            inventorPanel.ReciveBtnActioFromSelectPanel(index);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

