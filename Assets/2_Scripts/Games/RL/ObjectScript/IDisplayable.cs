using UnityEngine;

public interface IDisplayable
{
    string GetDisplayableName();
    Sprite GetDisplayableImage();
    void SetDisplayableImage(Sprite image);
    int GetExtraInfo();

    void SetExtraInfo(int extraInfo);
}
