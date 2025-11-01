#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    public Transform plane;
    [SerializeField]
    public GameObject CellPrefab;

    public int gridX = 10;
    public int gridZ= 10;
    void Start()
    {
        float mapWidth = plane.localScale.x;
        float mapHeight = plane.localScale.z;

        Vector3 origin = plane.position - new Vector3(mapWidth / 2f, 0, mapHeight / 2f);
        float cellWidth = mapWidth / gridX;
        float cellHeight = mapHeight / gridZ;

        for(int z = 0; z < gridZ; ++z)
        {
            for(int x = 0; x < gridX; ++x)
            {
                Vector3 center = origin + new Vector3(
                   cellWidth * (x + 0.5f),
                   0,
                   cellHeight * (z + 0.5f)
               );
                if(CellPrefab)
                {
                    Instantiate(CellPrefab, center, Quaternion.identity, transform);
                   
                }
       
            }

        }

    }

}
