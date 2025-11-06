using UnityEngine;

[CreateAssetMenu(fileName = "CropsData", menuName = "LUPData/CropsData")]
public class CropsData : ScriptableObject
{
    public CropType cropsType;
    public float productionTime;
    public int productionAmount;
    public ResourceType resourceType;
    public float resourceConsume;
}