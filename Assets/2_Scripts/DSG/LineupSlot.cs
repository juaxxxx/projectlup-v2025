using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace LUP.DSG
{
    public class LineupSlot : MonoBehaviour
    {
        [SerializeField]
        Character CharacterModelPrefab;

        Transform slotTransform;
        public bool isPlaced = false;
        public Character character { get; private set; }

        public Transform AttackedPosition;

        public event System.Action OnCPUpdated;
        public OwnedCharacterInfo characterInfo { get; private set; }

        public DataCenter dataCenter;

        [SerializeField] private int modelID;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStageInitialize += Initialize;
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStageInitialize -= Initialize;
        }

        private void Initialize(DeckStrategyStage stage)
        {
            slotTransform = this.transform;
            character = Instantiate(CharacterModelPrefab, slotTransform.position, slotTransform.rotation);

            //character.transform.localPosition = Vector3.zero;
            character.transform.localScale = Vector3.one;

            character.gameObject.SetActive(false);
        }

        public void SetSelectedCharacter(OwnedCharacterInfo info, bool isEnemy)
        {
            isPlaced = true;
            characterInfo = info;
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);
            OnCPUpdated?.Invoke();
        }

        public void DeselectCharacter()
        {
            isPlaced = false;
            characterInfo = null;
            character.ClearCharacterInfo();
            OnCPUpdated?.Invoke();
        }

        public void ActivateBattleUI()
        {
            character.ActiveBattleUI();
        }

        public void ClearCharacter()
        {
            character = null;
        }
    }

}