using UnityEngine;

namespace LUP
{
    public interface IQuestTarget
    {
        //로그라이크 1000시작  
        //슈팅 2000시작,   
        //익스트랙션 슈터 3000시작, 
        //생산/건설/강화 4000시작,  
        //덱 전략 5000시작, 
        int QuestTargetId { get; }
        void Trigger(int value);
    }
}

