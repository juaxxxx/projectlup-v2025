using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorTree : MonoBehaviour
{
    private BTNode rootNode;
    private EnemyBehaviorTree behaviorTree;

    private void Awake()
    {
        behaviorTree = GetComponent<EnemyBehaviorTree>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBehaviorTree();
    }

    // Update is called once per frame
    void Update()
    {
        rootNode.Evaluate();
    }

    private void SetupBehaviorTree()
    {
        rootNode = new Selector(new List<BTNode> { });
    }
}
