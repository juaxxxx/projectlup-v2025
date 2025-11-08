using UnityEngine;
namespace LUP.PCR
{
    public class ANode
    {
        public bool isWalkable;
        public Vector3 worldPos;
        public int indexX;
        public int indexY;

        public int gCost;
        public int hCost;
        public ANode parentNode;


        public ANode(bool nWalkable, Vector3 nWorldPos, int nIndexX, int nIndexY)
        {
            isWalkable = nWalkable;
            worldPos = nWorldPos;
            indexX = nIndexX;
            indexY = nIndexY;
        }

        public int FCost
        {
            get { return gCost + hCost; }
        }
    }
}

