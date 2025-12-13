using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{
    public class SniperBT : MonsterBTBase
    {
        protected override BaseNode SetupTree()
        {
            return new Selector(new List<BaseNode>
            {
                // 1. 죽음
                DeadSequence(),

                // 2. 스턴
                StunnedSequence(),

                // 4. 사거리 내 공격
                AttackSequence(),

                // 6. 적정 거리 유지하며 이동
                MoveToPlayerAction()
            });
        }
    }
}