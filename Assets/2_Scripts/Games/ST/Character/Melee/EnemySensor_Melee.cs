using UnityEngine;

namespace LUP.ST
{
    public class EnemySensor_Melee : MonoBehaviour
    {
        public LayerMask targetMask;
        public float detectRange = 15f;
        MeleeBlackBoard bb;

        void Awake() 
        { 
            bb = GetComponent<MeleeBlackBoard>();
        }

        void Update()
        {
            if (bb.IsAttackingFlag)
                return;

            Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, targetMask);
            Transform best = null; float bestDist = float.MaxValue;
            foreach (Collider h in hits)
            {
                float d = Vector3.Distance(transform.position, h.transform.position);
                if (d < bestDist) { bestDist = d; best = h.transform; }
            }
            bb.Target = best;

            if (best != null)
            {
                bb.ReportEnemySeen();
            }
        }
    }
}