using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ES
{
    public class EquipmentUI : MonoBehaviour
    {
        public GameObject slotPrefab;
        public Transform slotParent;
        private List<RawImage> slots = new List<RawImage>();

        void Start()
        {
            // 런타임에 30개 슬롯 생성
            CreateSlots(30);
        }

        public void CreateSlots(int count)
        {
            // 기존 슬롯 제거
            foreach (Transform child in slotParent)
                Destroy(child.gameObject);
            slots.Clear();

            // 새 슬롯 생성
            for (int i = 0; i < count; i++)
            {
                GameObject slot = Instantiate(slotPrefab, slotParent);
                slots.Add(slot.GetComponent<RawImage>());
                slot.GetComponent<RawImage>().texture = null; // 빈 슬롯
            }
        }

        // 장비 아이템 들어왔을 때 업데이트
        public void UpdateEquipment(Texture[] equipment)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].texture = (i < equipment.Length) ? equipment[i] : null;
            }
        }
    }

}