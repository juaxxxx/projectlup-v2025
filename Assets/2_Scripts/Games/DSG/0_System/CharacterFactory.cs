using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public interface ICharacterFactory
    {
        Character GetCharacter(CharacterInfo info, Transform parentTransform, bool isEnemy);
        void ReturnCharacter(Character character);
    }

    public class CharacterFactory : ICharacterFactory
    {
        private DeckStrategyStage deckStage;

        private readonly Dictionary<int, Queue<Character>> characterPool = new();

        public CharacterFactory(DeckStrategyStage stage)
        {
            deckStage = stage;
        }

        public Character GetCharacter(CharacterInfo info, Transform parentTransform, bool isEnemy)
        {
            if (info == null || deckStage == null) return null;

            int modelId = info.characterModelID;
            Character character = null;

            if (characterPool.TryGetValue(modelId, out Queue<Character> pool) && pool.Count > 0)
            {
                character = pool.Dequeue();
                character.transform.SetParent(parentTransform);
            }
            else
            {
                GameObject prefab = deckStage.GetCharacterPrefab(modelId);
                if (prefab == null) return null;

                GameObject characterObject = Object.Instantiate(prefab, parentTransform);
                if (!characterObject.TryGetComponent(out character))
                {
                    Object.Destroy(characterObject);
                    return null;
                }
            }

            character.transform.localPosition = Vector3.zero;
            character.transform.localRotation = Quaternion.identity;

            float parentScaleX = parentTransform.lossyScale.x;
            if (Mathf.Approximately(parentScaleX, 0f)) parentScaleX = 1f;
            character.transform.localScale = Vector3.one / parentScaleX;

            character.ManualInitializeAfterSpawn();
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);
            character.gameObject.SetActive(true);

            return character;
        }

        public void ReturnCharacter(Character character)
        {
            if (character == null || character.characterData == null) return;

            int modelId = character.characterPrefabData.ID;

            if (!characterPool.ContainsKey(modelId))
            {
                characterPool[modelId] = new Queue<Character>();
            }

            character.gameObject.SetActive(false);

            character.transform.SetParent(deckStage.transform);

            characterPool[modelId].Enqueue(character);
        }
    }
}