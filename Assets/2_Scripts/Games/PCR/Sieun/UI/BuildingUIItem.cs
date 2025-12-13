using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro; //@TODO : TMP 폰트로 변경
using System;

namespace LUP.PCR
{
    public class BuildingUIItem : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Button btn;

        public void Setup(BuildingBase building, Action onClick)
        {
            if (nameText != null)
            {
                nameText.text = building.buildingName; // //@TODO : building.buildingName으로 가져오기
            }

            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}

