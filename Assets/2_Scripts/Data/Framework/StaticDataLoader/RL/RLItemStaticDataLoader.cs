using UnityEngine;

[CreateAssetMenu(fileName = "ItemLoader", menuName = "StaticData/RL Item Loader")]
public class RLItemStaticDataLoader : BaseStaticDataLoader<LUP.RLItemStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1OcIRuu5efxWr63dNE0e9DQ6OD5gPnNWCoPh_nN8Oe6A/export?format=csv&gid=1498747128#gid=1498747128";
    
}

