using UnityEngine;

namespace LUP.PCR
{
    public interface IUnitMoveable
    {
        void SetDestination(Vector3 destination);
        void SetDestination(Vector2Int destination);
        bool IsArrived();
        void Stop();


        Vector3 CurrentDestination { get; }
    }
}

