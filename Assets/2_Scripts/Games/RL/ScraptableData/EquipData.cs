using LUP.Define;
using Roguelike.Define;
using UnityEngine;

namespace LUP.RL
{
    public struct EquipStat
    {
        public string statName;
        public int value;
    }

    [CreateAssetMenu(fileName = "EquipData", menuName = "Scriptable Objects/EquipData")]
    public class EquipData : ScriptableObject, IDisplayable
    {
        [SerializeField]
        private string equipName;

        [SerializeField]
        private Sprite equipImage;

        [SerializeField]
        private RLItemTier equipTier;

        public string equipDescription;
        public EquipStat[] equipStats;
        public RWeaponType weaponType = RWeaponType.None;
        public RLEquipPos equipPos = RLEquipPos.None;

        [HideInInspector]
        public int inventorySlotKey = -1;

        public string GetDisplayableName() { return equipName; }
        public Sprite GetDisplayableImage() { return equipImage; }

        public void SetDisplayableImage(Sprite image) { equipImage = image; }
        public void SetItemName(string name) { equipName = name; }

        public int GetExtraInfo() { return (int)equipTier; }
        public void SetExtraInfo(int tier) { equipTier = (RLItemTier)tier; }
    }
}


