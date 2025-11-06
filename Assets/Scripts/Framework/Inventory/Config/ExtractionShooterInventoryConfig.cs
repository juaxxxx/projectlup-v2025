using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 익스트랙션 슈터 게임용 인벤토리 설정
    /// (타르코프 스타일)
    /// </summary>
    public class ExtractionShooterInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                // TODO: 재화 타입 추가 (예: "Credit", "Scrap", "Insurance")
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                // TODO: 장비 슬롯 추가 (예: "Helmet", "Vest", "PrimaryWeapon", "Backpack")
            };
        }

        public int GetDefaultCapacity()
        {
            return 40; // TODO: 필요한 용량으로 변경
        }

        public Type GetCustomInventoryType()
        {
            return null;
        }
    }
}
