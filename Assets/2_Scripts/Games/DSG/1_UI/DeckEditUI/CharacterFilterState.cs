using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace LUP.DSG
{
    public class CharacterFilterState
    {
        private readonly Dictionary<Type, int> filterMasks = new Dictionary<Type, int>();

        public void AddFilter<T>(T value) where T : unmanaged, Enum
        {
            Type type = typeof(T);
            filterMasks.TryGetValue(type, out int currentMask);

            int intValue = UnsafeUtility.EnumToInt(value);
            filterMasks[type] = currentMask | (1 << intValue);
        }

        public bool ContainsCheckedFilters()
        {
            return filterMasks.Count > 0;
        }

        public bool CheckFilterMatch<T>(T trait) where T : unmanaged, Enum
        {
            if (!filterMasks.TryGetValue(typeof(T), out int mask) || mask == 0)
                return true;

            int intValue = UnsafeUtility.EnumToInt(trait);
            return (mask & (1 << intValue)) != 0;
        }

        public int GetFilterMask<T>() where T : unmanaged, Enum
        {
            return filterMasks.TryGetValue(typeof(T), out int mask) ? mask : 0;
        }
    }
}