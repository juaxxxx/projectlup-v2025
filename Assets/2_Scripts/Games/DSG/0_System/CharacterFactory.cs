using UnityEngine;

namespace LUP.DSG
{
    public interface ICharacterFactory
    {
        Character CreateCharacter(OwnedCharacterInfo info, Transform parentTransform, bool isEnemy);
    }

    public class CharacterFactory : ICharacterFactory
    {
        private DeckStrategyStage deckStage;

        public CharacterFactory(DeckStrategyStage stage)
        {
            deckStage = stage;
        }

        public Character CreateCharacter(OwnedCharacterInfo info, Transform parentTransform, bool isEnemy)
        {
            if (info == null || deckStage == null) return null;

            GameObject prefab = deckStage.GetCharacterPrefab(info.characterModelID);
            if (prefab == null) return null;

            GameObject characterObject = Object.Instantiate(prefab, parentTransform);

            characterObject.transform.localPosition = Vector3.zero;
            characterObject.transform.localRotation = Quaternion.identity;

            characterObject.transform.localScale = Vector3.one / parentTransform.lossyScale.x;

            Character character = characterObject.GetComponent<Character>();
            if (character == null)
            {
                Object.Destroy(characterObject);
                return null;
            }

            character.ManualInitializeAfterSpawn();
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);
            character.gameObject.SetActive(true);

            return character;
        }

    }
}