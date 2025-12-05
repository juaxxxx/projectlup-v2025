using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace LUP.DSG
{
    public class LineupSlot : MonoBehaviour
    {
        private Transform slotTransform;
        public bool isPlaced = false;
        public Character character { get; private set; }

        public Transform AttackedPosition;

        public event System.Action OnCPUpdated;
        public OwnedCharacterInfo characterInfo { get; private set; }

        private DeckStrategyStage deckStage;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStageInitialize += Initialize;
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
        }

        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStageInitialize -= Initialize;
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void Initialize(DeckStrategyStage stage)
        {
            Debug.Log("Initialize");
            deckStage = stage;
            slotTransform = transform;
        }
        private void PostInitialize(DeckStrategyStage stage)
        {
            Debug.Log("PostInitialize");
        }

        public void SetSelectedCharacter(OwnedCharacterInfo info, bool isEnemy)
        {
            Debug.Log("SetSelectedCharacter");
            if (info == null)
            {
                return;
            }

            if (deckStage == null)
            {
                return;
            }

            // 이미 캐릭터가 배치되어 있으면 제거 (다른 캐릭터로 교체하는 경우)
            if (character != null)
            {
                Destroy(character.gameObject);
                character = null;
            }

            // 이번에 배치할 캐릭터의 모델 ID 사용
            int modelId = info.characterModelID;
            GameObject prefab = deckStage.GetCharacterPrefab(modelId);
            if (prefab == null)
            {
                return;
            }

            GameObject go = Instantiate(
                prefab,
                slotTransform.position,
                slotTransform.rotation,
                slotTransform
            );

            go.transform.localScale = Vector3.one / slotTransform.lossyScale.x;

            character = go.GetComponent<Character>();
            if (character == null)
            {
                Destroy(go);
                return;
            }
            character.ManualInitializeAfterSpawn();
            isPlaced = true;
            characterInfo = info;
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);

            character.gameObject.SetActive(true);

            OnCPUpdated?.Invoke();
        }

        public void DeselectCharacter()
        {
            isPlaced = false;
            characterInfo = null;

            if (character != null)
            {
                character.DestroyUI();
                Destroy(character.gameObject);
                character = null;
            }

            OnCPUpdated?.Invoke();
        }

        public void ActivateBattleUI()
        {
            if (character != null)
                character.ActiveBattleUI();
        }

        public void ClearCharacter()
        {
            if (character != null)
            {
                Destroy(character.gameObject);
                character = null;
            }
            isPlaced = false;
            characterInfo = null;
        }
    }
}