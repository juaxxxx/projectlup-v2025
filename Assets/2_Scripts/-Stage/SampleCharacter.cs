using UnityEngine;

public class SampleCharacter : MonoBehaviour
{
    //[SerializeField]
    //private RoguelikeRuntimeData runtimedata;
    //public RoguelikeStaticData characterdata;

    //void Awake()
    //{
    //    RoguelikeStage stage = LUP.StageManager.Instance.GetCurrentStage() as RoguelikeStage;
    //    if (stage != null)
    //    {
    //        RoguelikeStaticDataLoader staticdataloader = (RoguelikeStaticDataLoader)stage.StaticDataLoader;
    //        RoguelikeRuntimeData runtimeData = (RoguelikeRuntimeData)stage.RuntimeData;
    //        List<RoguelikeStaticData> datalist = staticdataloader.GetDataList();

    //        //characterdata.SPEED = datalist[0].SPEED;
    //        //characterdata.description = datalist[0].description;
    //        //characterdata.stat = datalist[0].stat;
    //        //characterdata.gold = datalist[0].gold;

    //        runtimedata = runtimeData;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (runtimedata == null) return;

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        runtimedata.id += 100;  
    //    }

    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        runtimedata.ResetData();
    //        runtimedata.SaveData(); 
    //    }
    //}
}
