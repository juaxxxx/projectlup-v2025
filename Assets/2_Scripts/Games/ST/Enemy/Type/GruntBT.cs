using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{
    public class GruntBT : MonsterBTBase
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

                // 4. 공격
                AttackSequence(),

                // 5. 이동
                MoveToPlayerAction()
            });
        }
    }
}