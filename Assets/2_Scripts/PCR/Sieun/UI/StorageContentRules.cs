using UnityEngine;
namespace LUP.PCR
{
    public class StorageContentRules : MonoBehaviour
    {
        [SerializeField] GameObject emptySlot;

        [Header("생산 스탯")]
        int outputTime;
        int currMainResourceStorageLimits; // 생산물 저장 상한 
        int currSlotSumCount;

        void Start()
        {

        }
    }

    //currSlotSumCount = currMainResourceStorageLimits + currSubResourceStorageLimits;
    //int currSubResourceStorageLimits;  // 생산 에너지원 저장 상한(슬롯 하나에 최대 10개)

}
