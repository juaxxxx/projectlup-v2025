using UnityEngine;

namespace LUP.DSG
{
    public class ChainedTargetSelector : IAttackTargetSelector
    {
        private readonly IAttackTargetSelector[] chain;
        public ChainedTargetSelector(params IAttackTargetSelector[] chain) => this.chain = chain;
        public TargetPatternType PatternType
        {
            get { return TargetPatternType.None; }
        }
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

        public LineupSlot SelectSettingTarget(Character Attacker,TargetPatternType targetPatternType)
        {
            IAttackTargetSelector selector =
       System.Array.Find(chain, s => s.PatternType == targetPatternType); 

            if (selector == null)
                return null;

            // 2) 찾아낸 selector로 실제 타겟 선택
            return selector.SelectEnemyTarget(Attacker);
        }
    }
}