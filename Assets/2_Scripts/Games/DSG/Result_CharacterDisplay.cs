using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LUP.DSG
{
    public class ResultCharacterDisplay : MonoBehaviour
    {
        private TeamMVPData mvpData;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void PostInitialize(DeckStrategyStage stage)
        {
            mvpData = Resources.Load<TeamMVPData>("TeamMVPData");
            if (mvpData == null)
            {
                Debug.LogError("TeamMVPData.asset 을 찾을 수 없습니다! (Assets/Resources/TeamMVPData.asset)");
                return;
            }

            // 각 슬롯 자동 세팅
            SetSlot("MVP1", mvpData.char1Name, mvpData.char1Color);
            SetSlot("MVP2", mvpData.char2Name, mvpData.char2Color);
            SetSlot("MVP3", mvpData.char3Name, mvpData.char3Color);
            SetSlot("MVP4", mvpData.char4Name, mvpData.char4Color);
            SetSlot("MVP5", mvpData.char5Name, mvpData.char5Color);
        }

        private void SetSlot(string slotName, string charName, Color color)
        {
            Transform slot = transform.Find($"CharacterList_test/{slotName}");
            if (slot == null) return;

            Image image = slot.Find("Image1")?.GetComponent<Image>();
            TMP_Text text = slot.Find("Text_Name1")?.GetComponent<TMP_Text>();

            if (image != null)
            {
                image.color = color; // 캐릭터 색상 표시
            }

            if (text != null)
            {
                text.text = string.IsNullOrEmpty(charName) ? "-" : charName;
            }
        }
    }
}