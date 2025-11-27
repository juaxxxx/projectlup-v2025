using LUP.PCR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerDataCenter : MonoBehaviour
{
    PCRDataCenter pcrDataCenter;
    AGridMap aGrid;

    IEnumerator Start()
    {
        // PCRDataCenter가 준비될 때까지 대기 (데이터 센터에 IsInitialized 같은 플래그가 있다면 베스트)
        // 플래그가 없다면 tileInfoes가 null이 아닐 때까지 대기

        // 예시: 외부에서 pcrDataCenter 참조를 할당받았다고 가정
        while (pcrDataCenter.tileInfoes == null)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 데이터가 들어온 것을 확인 후 맵 생성
        //aGrid.InitMap(pcrDataCenter.tileInfoes);
    }
}
