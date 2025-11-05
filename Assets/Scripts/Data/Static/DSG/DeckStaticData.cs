using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "DeckStaticData", menuName = "Scriptable Objects/DeckStaticData")]
public class DeckStaticData : BaseStaticData<DeckScriptData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/11yM9l6g4opxVTflwsOVV0nZoIPUQ9VnA0rhkasLEi7I/export?format=csv&gid=1544281943";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[DeckStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}
