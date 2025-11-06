using UnityEngine;

public interface IBuildState
{
    void Enter(BuildingBase building);
    void Exit(BuildingBase building);
    void Tick(BuildingBase building, float deltaTime);
    void Interact(BuildingBase building);
}