using LUP;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LUP.DSG
{
    public class DataCenter : MonoBehaviour
    {
        [SerializeField]
        private OwnedCharacterTable ownedCharacterTable;
        public CharacterDataTable characterDataTable;
        public CharacterModelDataTable characterModelDataTable;
        public TeamMVPData mvpData;

        List<OwnedCharacterInfo> ownedCharacterList;

        private void OnEnable()
        {
            StageEnterSystem.OnAfterDSGStageEnter += Initialize;
        }

        private void OnDisable()
        {
            StageEnterSystem.OnAfterDSGStageEnter -= Initialize;
        }

        private void Initialize(DeckStrategyStage stage)
        {
            if (stage != null)
            {
                DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;

                ownedCharacterList = runtimeData.OwnedCharacterList;
            }
        }

        public CharacterData FindCharacterData(int ID)
        {
            foreach (CharacterData data in characterDataTable.characterDataList)
            {
                if (data.ID == ID)
                {
                    return data;
                }
            }

            return null;
        }

        public CharacterModelData FindCharacterModel(int ID)
        {
            foreach (CharacterModelData modelData in characterModelDataTable.characterModelDataList)
            {
                if (modelData.ID == ID)
                {
                    return modelData;
                }
            }

            return null;
        }

        public List<OwnedCharacterInfo> GetOwnedCharacterList()
        {
            if(ownedCharacterList != null && ownedCharacterList.Count > 0)
            {
                return ownedCharacterList;
            }
            return ownedCharacterTable.ownedCharacterList;
        }

        public void SetOwnedCharacterList()
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage != null)
            {
                DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;

                ownedCharacterList = runtimeData.OwnedCharacterList;
            }
        }
    }
}