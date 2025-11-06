using UnityEngine;

public class WallDust : WallBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InteractForTouch()
    {
        Debug.Log("WallDust");
        Destroy(gameObject);
    }
}
