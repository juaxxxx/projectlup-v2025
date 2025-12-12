using UnityEngine;
using LUP.DSG.Utils.Enums;
using System.Collections.Generic;

namespace LUP.DSG
{
    public class PickWeakTarget : AttackTargetSelectorBase
    {
        public PickWeakTarget(BattleSystem battle) : base(battle) { }

        public override TargetPatternType PatternType => TargetPatternType.Weak;
        public override LineupSlot SelectEnemyTarget(Character Attacker)
        {
            if (battle == null || Attacker == null)
                return null;

            List<LineupSlot> slots = GetAliveTargetList(Attacker);
            if (slots == null)
                return null;

            Utils.Enums.EAttributeType type = Attacker.characterData.type;

            switch (type)
            {
                case EAttributeType.ROCK: //공격자의 type을 가져와 약점인 적이있는지 5번(Front)부터 확인함
                    {
                        for (int i = slots.Count - 1; i >= 0; i--)
                        {
                            if (slots[i].character.characterData.type == EAttributeType.SCISSORS)
                            {
                                return slots[i];
                            }
                        }
                    }
                    break;//goto case EAttributeType.PAPER; //원래는 약점상대가 없으면 다른 상대 공격
                case EAttributeType.PAPER:
                    {
                        for (int i = slots.Count - 1; i >= 0; i--)
                        {
                            if (slots[i].character.characterData.type == EAttributeType.ROCK)
                            {
                                return slots[i];
                            }
                        }
                    }
                    break;//goto case EAttributeType.SCISSORS;
                case EAttributeType.SCISSORS:
                    {
                        for (int i = slots.Count - 1; i >= 0; i--)
                        {
                            if (slots[i].character.characterData.type == EAttributeType.PAPER)
                            {
                                return slots[i];
                            }
                        }
                    }
                    break; //goto case EAttributeType.ROCK;
            }

            return null;
        }
    }
}