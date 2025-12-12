using UnityEngine;

[CreateAssetMenu(fileName = "ItemLoader", menuName = "StaticData/LUP Item Loader")]
public class ESItemStaticDataLoader : BaseStaticDataLoader<LUP.ESItemStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1R-q7I41tJMOg7_Melx8lQYMLczS27u-pGseCk-DLxeE/export?format=csv&gid=1480693328#gid=1480693328";
}
