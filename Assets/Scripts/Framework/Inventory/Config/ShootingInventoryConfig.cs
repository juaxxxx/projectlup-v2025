using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 슈팅 게임용 인벤토리 설정
    /// </summary>
    public class ShootingInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                // TODO: 재화 타입 추가 (예: "Credit", "BattlePoint")
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                // TODO: 장비 슬롯 추가 (예: "PrimaryWeapon", "SecondaryWeapon")
            };
        }

        public int GetDefaultCapacity()
        {
            return 20; // TODO: 필요한 용량으로 변경
        }

        public Type GetCustomInventoryType()
        {
            return null;
        }
    }
}
