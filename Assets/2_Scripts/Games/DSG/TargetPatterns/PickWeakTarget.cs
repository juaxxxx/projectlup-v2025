using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUP.DSG
{
    public class PickWeakTarget : AttackTargetSelectorBase
    {
        public PickWeakTarget(BattleSystem battle) : base(battle) { }

        public override TargetPatternType PatternType => TargetPatternType.Weak;
        public override List<LineupSlot> SelectEnemyTargets(Character Attacker, int count)
        {
            if (battle == null || Attacker == null)
                return null;

            List<LineupSlot> Alive = GetAliveTargetList(Attacker);
            List<LineupSlot> Slots = new List<LineupSlot>();
            if (Alive == null)
                return null;

            int mincount = Mathf.Min(Alive.Count, count);

            Utils.Enums.EAttributeType type = Attacker.characterData.type;
            int k = 0;

            switch (type)
            {
                case EAttributeType.ROCK: //공격자의 type을 가져와 약점인 적이있는지 5번(Front)부터 확인함
                    {
                        return GetWeakTargets(Alive, EAttributeType.SCISSORS, mincount);
                    }
                case EAttributeType.PAPER:
                    {
                        return GetWeakTargets(Alive, EAttributeType.ROCK, mincount);
                    }
                case EAttributeType.SCISSORS:
                    {
                        //for (int i = Alive.Count - 1; i >= 0; i--)
                        //{
                        //    if (Alive[i].character.characterData.type == EAttributeType.PAPER)
                        //    {
                        //        Slots[k++] = Alive[i];
                        //        if(k >= mincount)
                        //        {
                        //            return Slots;
                        //        }
                        //    }
                        //}

                        return GetWeakTargets(Alive, EAttributeType.PAPER, mincount);
                    }
            }
            return null;
        }

        private List<LineupSlot> GetWeakTargets(List<LineupSlot> Alive,EAttributeType type, int count)
        {
            List<LineupSlot> Slots = new List<LineupSlot>();
            int k = 0;

            for (int i = Alive.Count - 1; i >= 0; i--)
            {
                if (Alive[i].character.characterData.type == type)
                {
                    LineupSlot lineupSlot = Alive[i];
                    Slots.Add(lineupSlot);
                    if (Slots.Count >= count)
                    {
                        return Slots;
                    }
                }
            }
            return null;
        }
    }
}