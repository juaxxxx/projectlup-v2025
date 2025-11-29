using LUP.DSG;
using LUP.PCR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerDataCenter : MonoBehaviour
    {
        [SerializeField] PCRDataCenter pcrDataCenter;
        [SerializeField] AGridMap aGrid;
        [SerializeField] WorkerAI workerAI;

        //IEnumerator Start()
        //{
        //    // 스크립트 실행순서를 PCRDataCenter보다 뒤로 설정
        //    // PCRDataCenter가 준비될 때까지 대기
        //    // 데이터 센터에 IsInitialized 같은 플래그가 있거나,
        //    // 플래그가 없다면 tileInfoes가 null이 아닐 때까지 대기

        //    // 예시: 외부에서 pcrDataCenter 참조를 할당받았다고 가정
        //    while (pcrDataCenter.tileInfoes == null)
        //    {
        //        yield return null; // 다음 프레임까지 대기
        //    }

        //    // 데이터가 들어온 것을 확인 후 맵 생성
        //    aGrid.InitMap(pcrDataCenter.tileInfoes);

        //    workerAI.InitBTReferences();
        //}

        private void Awake()
        {
            pcrDataCenter = GetComponent<PCRDataCenter>();
        }

        private void Start()
        {
            TestInitData(); // from PCRDataCenter

            aGrid.InitMap(pcrDataCenter.tileInfoes);

            workerAI.InitBTReferences();
        }

        private void Update()
        {
            workerAI.UpdateBT();
        }


        public void TestInitData() //@TODO:혼자 테스트 하는 용. 테스트한 후에 지울것. notWalls 가 Null이라서 만듬.
        {
            pcrDataCenter.tileInfoes = new TileInfo[28, 15];

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    pcrDataCenter.tileInfoes[i, j] = new TileInfo(TileType.NONE, BuildingType.NONE, WallType.NONE, new Vector2Int(i, j), 1);
                }
            }

        }


    }
}

