using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public static class CharacterIconCache
    {
        private static readonly Dictionary<int, Sprite> _cache = new();

        public static void Set(int characterId, Sprite sprite)
        {
            if (sprite == null) return;
            _cache[characterId] = sprite;
        }

        public static bool TryGet(int characterId, out Sprite sprite)
            => _cache.TryGetValue(characterId, out sprite);
    }
}