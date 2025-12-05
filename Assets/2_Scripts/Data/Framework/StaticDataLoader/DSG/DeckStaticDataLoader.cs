using UnityEngine;

[CreateAssetMenu(fileName = "DeckStaticData", menuName = "Scriptable Objects/DeckStaticData")]
public class DeckStaticDataLoader : BaseStaticDataLoader<DeckStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=1593366084#gid=1593366084";
}
