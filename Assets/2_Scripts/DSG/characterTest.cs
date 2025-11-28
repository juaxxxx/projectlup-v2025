using UnityEngine;

public class characterTest : MonoBehaviour
{
    [SerializeField]
    private Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SkinnedMeshRenderer>().material.color = color;
    }
}
