using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{

    public class RusherBT : MonsterBTBase
    {
        protected override BaseNode SetupTree()
        {
            return new Selector(new List<BaseNode>
            {
                // 1. 죽음
                DeadSequence(),

                // 2. 스턴
                StunnedSequence(),

                // 3. 근접 공격 (사거리 짧음)
                AttackSequence(),

                // 4. 돌진 (빠른 이동) - 일반 Move 대신 Rush 사용
                //new ActionNode(() => MonsterActions.Rush(data))
                MoveToPlayerAction()
            });
        }
    }
}