using UnityEngine;

namespace LUP.PCR
{
    public class CompletedState : IBuildState
    {
        private CompletedData completedData;

        public void Enter(BuildingBase building)
        {
            Debug.Log("CompletedState Enter");
        }
        public void Exit(BuildingBase building)
        {
            Debug.Log("CompletedState Exit");
        }
        public void Tick(BuildingBase building, float deltaTime)
        {

        }
        public void Interact(BuildingBase building)
        {
            Debug.Log("CompletedState Interact");
        }
    }
}
