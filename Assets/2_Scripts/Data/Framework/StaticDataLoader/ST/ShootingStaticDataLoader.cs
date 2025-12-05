using UnityEngine;

[CreateAssetMenu(fileName = "ShootingStaticData", menuName = "Scriptable Objects/ShootingStaticData")]
public class ShootingStaticDataLoader : BaseStaticDataLoader<ShootingStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1rwLdR5cOTk5i262bj6VY-WPTY3YJRJB28WbkxAzpcHE/export?format=csv&gid=839444384#gid=839444384";
}
