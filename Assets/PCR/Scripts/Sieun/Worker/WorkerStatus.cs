using UnityEngine;

public class WorkerStatus
{
    private float hunger;
    private bool isHunger;

    public float Hunger { get; set; } = 0f;
    public bool IsHunger { get; set; } = false;


    public bool IsMoving { get; set; } = false;
    public bool IsEating { get; set; } = false;



}
