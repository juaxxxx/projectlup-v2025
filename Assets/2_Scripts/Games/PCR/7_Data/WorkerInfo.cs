using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class WorkerInfo
    {
        public float hunger;
        bool hasTask;
        BuildingInfo currentTaskBuildingInfo;

        public int id;
        public string name;
        public StructureBase initPlace;
//      LastWorkEndTime = Time.time;   // 게임 시작 시점 or 일 끝난 시점 기록
    }
}
