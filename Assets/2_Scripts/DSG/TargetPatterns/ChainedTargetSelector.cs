using UnityEngine;

namespace LUP.DSG
{
    public class ChainedTargetSelector : IAttackTargetSelector
    {
        private readonly IAttackTargetSelector[] chain;
        public ChainedTargetSelector(params IAttackTargetSelector[] chain) => this.chain = chain;

        public LineupSlot SelectEnemyTarget(Character Attacker)
        {
            for (int i = 0; i < chain.Length; i++)
            {
                LineupSlot slot = chain[i].SelectEnemyTarget(Attacker);

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