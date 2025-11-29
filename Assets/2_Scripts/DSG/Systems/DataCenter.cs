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
    }
}