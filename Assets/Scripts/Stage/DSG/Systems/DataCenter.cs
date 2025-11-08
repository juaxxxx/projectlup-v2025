using Manager;
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

        //List<DeckScriptData> dataList;


        private void Awake()
        {
            //DeckStrategyStage stage = Manager.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            //DeckStaticData staticData = (DeckStaticData)stage.StaticData;
            //dataList = staticData.GetDataList();
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
            return ownedCharacterTable.ownedCharacterList;
        }

        //public DeckScriptData GetCharacterStatus(int id, int level)
        //{
        //    string keyString = (id*100 + level).ToString();
        //    int key = int.Parse(keyString);

        //    foreach(var data in dataList)
        //    {
        //        if(data.tableId == key)
        //        {
        //            return data;
        //        }
        //    }

        //    return null;
        //}

    }
}