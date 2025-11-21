using UnityEngine;

namespace LUP.PCR
{

    // RequireComponent(typeof(Animator))]
    public class Worker : MonoBehaviour
    {
        public Renderer MeshRenderer;
        private void Awake()
        {
            //MeshRenderer = GetComponentInChildren<Renderer>();
        }
    }
}
