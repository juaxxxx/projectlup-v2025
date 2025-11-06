using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IGameInventoryConfig
    {
        /// 게임에서 사용하는 재화 타입들 (예: "Gold", "Diamond", "Crystal")
        List<string> GetCurrencyTypes();

        /// 게임에서 사용하는 장비 슬롯들 (예: "Weapon", "Armor", "Helmet")
        List<string> GetEquipmentSlots();

        /// 기본 인벤토리 슬롯 용량
        int GetDefaultCapacity();
    }
}
