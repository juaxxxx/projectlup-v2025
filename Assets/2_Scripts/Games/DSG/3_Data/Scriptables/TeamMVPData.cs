using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [CreateAssetMenu(fileName = "TeamMVPData", menuName = "DSG/Team MVP Data", order = int.MaxValue)]
    public class TeamMVPData : ScriptableObject
    {
        public string battleResult;
        public float char1Score, char2Score, char3Score, char4Score, char5Score;
        public string char1Name, char2Name, char3Name, char4Name, char5Name;
        public GameObject char1Prefab, char2Prefab, char3Prefab, char4Prefab, char5Prefab;
        public int char1CharacterId; public int char1ModelId;
        public int char2CharacterId; public int char2ModelId;
        public int char3CharacterId; public int char3ModelId;
        public int char4CharacterId; public int char4ModelId;
        public int char5CharacterId; public int char5ModelId;
        public Sprite char1Icon, char2Icon, char3Icon, char4Icon, char5Icon;
    }
}