using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ST
{

    public class CharacterBT : BehaviorTreeBase
    {
        private CharacterInfo character;
        private CharacterActions characterActions;

        protected override BaseNode SetupTree()
        {
            character = GetComponent<CharacterInfo>();
            characterActions = GetComponent<CharacterActions>();

            NodeState Retire() => characterActions.Retire(character);
            NodeState FireManual() => characterActions.FireManual(character);
            NodeState FireAuto() => characterActions.FireAuto(character);
            NodeState Cover() => characterActions.Cover(character);
            NodeState Reload() => characterActions.Reload(character);

            // 수동 모드 행동트리
            Selector manualSelector = new Selector(new List<BaseNode>
        {
            // 1. 재장전 중이면 재장전 완료까지 대기
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => characterActions.IsReloading),
                new ActionNode(Reload)
            }),
            
            // 2. 플레이어 입력 + 탄약 있으면 발사
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => character.IsPlayerInputExists()),
                new ConditionNode(() => character.HasAmmo()),
                new ConditionNode(() => !characterActions.IsReloading),
                new ActionNode(FireManual)
            }),
            
            // 3. 기본 대기
            new ActionNode(Cover)
        });

            // 자동 모드 행동트리 
            Selector autoSelector = new Selector(new List<BaseNode>
        {
            // 1. 재장전 중이면 재장전 완료까지 대기
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => characterActions.IsReloading),
                new ActionNode(Reload)
            }),
            
            // 2. 적 있고 탄약 있으면 자동 공격
            new Sequence(new List<BaseNode>
            {
                new ConditionNode(() => character.IsEnemyInRange()),
                new ConditionNode(() => character.HasAmmo()),
                new ConditionNode(() => !characterActions.IsReloading),
                new ActionNode(FireAuto)
            }),
            
            // 3. 기본 대기 
            new ActionNode(Cover)
        });

            // 메인 행동트리
            return new Selector(new List<BaseNode>
        {
            // HP 0 이하면 리타이어
            new Decorator(character.IsHpZero, new ActionNode(Retire)),
            
            // 모드에 따른 분기
            new Selector(new List<BaseNode>
            {
                new Decorator(character.IsManualMode, manualSelector),
                autoSelector
            })
        });
        }
    }
}
