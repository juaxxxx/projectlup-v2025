using System.Collections.Generic;
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
        public List<LineupSlot> SelectEnemyTargets(Character Attacker, int count)
        {
            for (int i = 0; i < chain.Length; i++)
            {
                List<LineupSlot> slots = chain[i].SelectEnemyTargets(Attacker,count);

                if (slots != null)
                {
                    //Debug.Log($"Pattern : {chain[i]}");
                    return slots;
                }

            }
            return null;
        }

        public List<LineupSlot> SelectSettingTarget(Character Attacker,TargetPatternType targetPatternType,int count)
        {
            IAttackTargetSelector selector =
       System.Array.Find(chain, s => s.PatternType == targetPatternType); 

            if (selector == null)
                return null;

            // 2) 찾아낸 selector로 실제 타겟 선택
            return selector.SelectEnemyTargets(Attacker,count);
        }
    }
}