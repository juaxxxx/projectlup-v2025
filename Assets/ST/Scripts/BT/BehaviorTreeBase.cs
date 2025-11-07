using UnityEngine;
namespace ST
{

    public abstract class BehaviorTreeBase : MonoBehaviour
    {
        protected BaseNode rootNode;

        protected abstract BaseNode SetupTree();

        void Start()
        {
            rootNode = SetupTree();
        }

        void Update()
        {
            if (rootNode != null)
            {
                rootNode.Evaluate();
            }

        }
    }

}