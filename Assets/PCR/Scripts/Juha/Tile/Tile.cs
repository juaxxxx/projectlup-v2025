using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileInfo tileInfo;

    [SerializeField]
    private GameObject CanActMark;
    [SerializeField]
    private GameObject CanNotActMark;

    private void Start()
    {
        HideCanDigWallMark();
        HideCanNotDigWallMark();
    }

    public void SetTileInfo(TileInfo tileInfo)
    {
        this.tileInfo = tileInfo;
    }

    public void ShowCanDigWallMark()
    {
        if (CanActMark)
        {
            CanActMark.SetActive(true);
        }
    }
    public void HideCanDigWallMark()
    {
        if (CanActMark)
        {
            CanActMark.SetActive(false);
        }
    }
    public void ShowCanNotDigWallMark()
    {
        if (CanNotActMark)
        {
            CanNotActMark.SetActive(true);
        }
    }
    public void HideCanNotDigWallMark()
    {
        if (CanNotActMark)
        {
            CanNotActMark.SetActive(false);
        }
    }
}
