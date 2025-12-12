using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public static class CharacterIconCache
    {
        private static readonly Dictionary<int, Sprite> _byCharacterId = new();
        private static readonly Dictionary<int, Sprite> _byModelId = new();

        public static void SetByCharacterId(int characterId, Sprite sprite)
        {
            if (sprite == null) return;
            _byCharacterId[characterId] = sprite;
        }

        public static bool TryGetByCharacterId(int characterId, out Sprite sprite)
            => _byCharacterId.TryGetValue(characterId, out sprite);

        public static void SetByModelId(int modelId, Sprite sprite)
        {
            if (sprite == null) return;
            _byModelId[modelId] = sprite;
        }

        public static bool TryGetByModelId(int modelId, out Sprite sprite)
            => _byModelId.TryGetValue(modelId, out sprite);
        public static bool TryGet(int characterId, int modelId, out Sprite sprite)
        {
            if (characterId != 0 && _byCharacterId.TryGetValue(characterId, out sprite))
                return true;

            if (modelId != 0 && _byModelId.TryGetValue(modelId, out sprite))
                return true;

            sprite = null;
            return false;
        }
    }
}