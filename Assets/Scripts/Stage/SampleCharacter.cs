using Manager;
using System.Collections.Generic;
using UnityEngine;

public class SampleCharacter : MonoBehaviour
{
    [SerializeField]
    private RoguelikeRuntimeData runtimedata;

    public RoguelikeScriptData characterdata;

    void Awake()
    {
        RoguelikeStage stage = Manager.StageManager.Instance.GetCurrentStage() as RoguelikeStage;
        if (stage != null)
        {
            RoguelikeStaticData staticdata = (RoguelikeStaticData)stage.StaticData;
            RoguelikeRuntimeData runtimeData = (RoguelikeRuntimeData)stage.RuntimeData;
            List<RoguelikeScriptData> datalist = staticdata.GetDataList();

            characterdata.name = datalist[0].name;
            characterdata.description = datalist[0].description;
            characterdata.stat = datalist[0].stat;
            characterdata.gold = datalist[0].gold;

            runtimedata = runtimeData;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runtimedata == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            runtimedata.id += 100;  
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            runtimedata.ResetData();
            runtimedata.SaveData(); 
        }
    }
}
