using UnityEngine;

[CreateAssetMenu(fileName = "RoguelikeStaticData", menuName = "Scriptable Objects/RoguelikeStaticData")]
public class RoguelikeStaticDataLoader : BaseStaticDataLoader<RoguelikeStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1OcIRuu5efxWr63dNE0e9DQ6OD5gPnNWCoPh_nN8Oe6A/export?format=csv&gid=926345530#gid=926345530";
}

