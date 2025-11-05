using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ExtractionStaticData", menuName = "Scriptable Objects/ExtractionStaticData")]
public class ExtractionStaticData : BaseStaticData<ExtractionScriptData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/11yM9l6g4opxVTflwsOVV0nZoIPUQ9VnA0rhkasLEi7I/export?format=csv&gid=831461229";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[ExtractionStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}
