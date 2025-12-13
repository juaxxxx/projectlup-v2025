using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{
    /// <summary>
    /// Tank (탱커)
    /// - 느린 이동 속도
    /// - 높은 HP
    /// - 엄폐 적극 활용 (HP 50% 이하부터)
    /// - 엄폐 상태에서도 사격
    /// </summary>
    public class TankBT : MonsterBTBase
    {
        protected override BaseNode SetupTree()
        {
            return new Selector(new List<BaseNode>
            {
                // 1. 죽음
                DeadSequence(),

                // 2. 스턴
                StunnedSequence(),

                // 3. 스킬 사용
                UsingSkillSequence(),

                // 4. 일반 공격
                AttackSequence(),

                // 5. 느리게 이동
                MoveToPlayerAction()
            });
        }
    }
}