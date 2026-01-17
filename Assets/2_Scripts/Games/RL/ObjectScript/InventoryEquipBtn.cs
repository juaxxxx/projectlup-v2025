using Roguelike.Define;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{

    public class InventoryEquipBtn : MonoBehaviour, IPanelContentAble
    {

        [SerializeField]
        private Sprite WeaponTypeIconImage;

        [SerializeField]
        private Sprite ArmorTypeIconImage;

        [SerializeField]
        public Button button;

        [SerializeField]
        private Image buttonBorder;

        [SerializeField]
        private Image ButtonBackGroundImage;

        [SerializeField]
        private Image ItemIcon;

        [SerializeField]
        private Image TierImage;
        [SerializeField]
        private Image TypeBorder;
        [SerializeField]
        private Image TypeIconImage;

        public bool Init()
        {

            if (button != null)
                return true;
            
            return false;
        }

        public void SetEquipButton(Sprite itemIcon, RLEquipPos equipPos, RLItemTier equipTier, TierColorData tierColor)
        {
            ButtonBackGroundImage.color = tierColor.BaseColor;
            buttonBorder.color = tierColor.BorderColor;

            ItemIcon.sprite = itemIcon;

            TypeBorder.color = tierColor.BorderColor;


            //좌상단 부위별 아이콘 설정
            switch(equipPos)
            {
                case RLEquipPos.Hand:
                    TypeIconImage.sprite = WeaponTypeIconImage;
                    break;

                case RLEquipPos.Body:
                    TypeIconImage.sprite = ArmorTypeIconImage;
                    break;
            }

            //장비 티어 이미지
            if (equipTier != RLItemTier.Epic)
                TierImage.gameObject.SetActive(false);

        }
    }
}

