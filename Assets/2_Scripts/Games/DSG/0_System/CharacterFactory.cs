using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LUP.DSG
{
    public class CharacterFactory : MonoBehaviour
    {
        private readonly DeckStrategyStage deckStage;
        private readonly Dictionary<int, IObjectPool<Character>> characterPools = new();

        public CharacterFactory(DeckStrategyStage stage)
        {
            deckStage = stage;
        }

        public Character GetCharacter(CharacterInfo info, Transform parentTransform, bool isEnemy)
        {
            if (info == null || deckStage == null) return null;

            int modelId = info.characterModelID;
            IObjectPool<Character> pool = GetOrCreatePool(modelId, parentTransform);

            Character character = pool.Get();
            if (character == null) return null;

            character.transform.SetParent(parentTransform);
            character.transform.localPosition = Vector3.zero;
            character.transform.localRotation = Quaternion.identity;

            float parentScaleX = parentTransform.lossyScale.x;
            if (Mathf.Approximately(parentScaleX, 0f)) parentScaleX = 1f;
            character.transform.localScale = Vector3.one / parentScaleX;

            character.ManualInitializeAfterSpawn();
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);

            return character;
        }

        public void ReturnCharacter(Character character)
        {
            if (character == null || character.characterData == null) return;

            int modelId = character.characterPrefabData.ID;

            if (characterPools.TryGetValue(modelId, out var pool))
                pool.Release(character);
            else
                Object.Destroy(character.gameObject);
        }

        private IObjectPool<Character> GetOrCreatePool(int modelId, Transform defaultParent)
        {
            if (characterPools.TryGetValue(modelId, out var pool))
                return pool;

            pool = new ObjectPool<Character>(
                createFunc: () => CreateCharacter(modelId, defaultParent),
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 20
            );

            characterPools[modelId] = pool;
            return pool;
        }

        private Character CreateCharacter(int modelId, Transform parentTransform)
        {
            GameObject prefab = deckStage.GetCharacterPrefab(modelId);
            if (prefab == null) return null;

            GameObject characterObject = Object.Instantiate(prefab, parentTransform);
            if (characterObject.TryGetComponent(out Character character))
                return character;

            Object.Destroy(characterObject);
            return null;
        }

        private void OnGetFromPool(Character character)
        {
            character.gameObject.SetActive(true);
        }

        private void OnReturnedToPool(Character character)
        {
            character.gameObject.SetActive(false);
            character.transform.SetParent(deckStage.transform);
        }

        private void OnDestroyPoolObject(Character character)
        {
            Object.Destroy(character.gameObject);
        }
    }
}