using UnityEngine;

namespace DSG
{
    public class ChainedTargetSelector : IAttackTargetSelector
    {
        private readonly IAttackTargetSelector[] chain;
        public ChainedTargetSelector(params IAttackTargetSelector[] chain) => this.chain = chain;

        public LineupSlot SelectTarget(Character Attacker)
        {
            for (int i = 0; i < chain.Length; i++)
            {
                LineupSlot slot = chain[i].SelectTarget(Attacker);

                if (slot != null)
                {
                    //Debug.Log($"Pattern : {chain[i]}");
                    return slot;
                }

            }
            return null;
        }
    }
}