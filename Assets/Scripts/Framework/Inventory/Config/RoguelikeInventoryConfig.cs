using System;
using System.Collections.Generic;

namespace Framework
{
    public class RoguelikeInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                "Gold",           // 기본 재화 (런 내에서 획득)
                "Soul",           // 영혼 (런 종료 시 획득, 영구 업그레이드용)
                "Crystal"         // 크리스탈 (프리미엄 재화)
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                "Weapon",         // 무기
                "Artifact1",      // 아티팩트 슬롯 1
                "Artifact2",      // 아티팩트 슬롯 2
                "Relic"           // 유물
            };
        }

        public int GetDefaultCapacity()
        {
            return 30; // 버프/아이템 많이 수집
        }

        public Type GetCustomInventoryType()
        {
            return null; // 기본 Inventory 사용
        }
    }
}
