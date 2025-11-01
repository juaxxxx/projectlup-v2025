using UnityEngine;
using Roguelike.Define;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
public class BuffData : ScriptableObject, IDisplayable
{
    public string buffName;

    [SerializeField]
    public BuffType type = BuffType.None;

    [SerializeField]
    public Sprite buffImage;

    [SerializeField]
    int maxObtainable = 0;



    public int canSeletable = 1;
    public string GetDisplayableName()
    {
        return null;
    }
    public Sprite GetDisplayableImage()
    {
        return buffImage;
    }
    public void SetDisplayableImage(Sprite image)
    {
        buffImage = image;
    }
    public int GetExtraInfo()
    {
        return maxObtainable;
    }

    public void SetExtraInfo(int extraInfo)
    {
        maxObtainable = extraInfo;
    }
}
