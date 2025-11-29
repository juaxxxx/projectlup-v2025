using LUP;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LUP.DSG
{
    public class DataCenter : MonoBehaviour
    {
        public CharacterModelDataTable characterModelDataTable;
        public TeamMVPData mvpData;

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

        public GameObject GetCharacterPrefab(int modelID)
        {
            CharacterModelData modelData = FindCharacterModel(modelID);

            if (modelData == null)
            {
                Debug.LogWarning($"[DataCenter] modelID {modelID} 에 해당하는 CharacterModelData가 없습니다.");
                return null;
            }

            if (modelData.prefab == null)
            {
                Debug.LogWarning($"[DataCenter] modelID {modelID} 에 prefab이 연결되어 있지 않습니다.");
                return null;
            }

            return modelData.prefab;
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