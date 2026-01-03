using UnityEngine;

[CreateAssetMenu(fileName = "ItemLoader", menuName = "StaticData/PCR Item Loader")]
public class PCRItemStaticDataLoader : BaseStaticDataLoader<LUP.PCRItemStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1_8bIM_IAx1r9BhxDEX2Pn3jaf5O42iigmWJmDKKXs6k/export?format=csv&gid=595493037#gid=595493037";

}
