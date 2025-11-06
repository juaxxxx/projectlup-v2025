using System.IO;
using UnityEngine;

public class AssetBundleSample : MonoBehaviour
{
    public RoguelikeStaticData characterdata;


    void Start()
    {
        AssetBundle runtimedataAB = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("Resources/AssetBundles", "staticdatas")));

        if (runtimedataAB == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        characterdata = runtimedataAB.LoadAsset<RoguelikeStaticData>("RoguelikeStaticData");

    }
}