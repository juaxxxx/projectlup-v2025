using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace DSG
{
    public enum ELinePosition
    {
        BACK1 = 1, BACK2 = 2, BACK3 = 3,
        FRONT1 = 4, FRONT2 = 5,
        MAX,
    };
    public class TargetingBrain : MonoBehaviour //º¸·ù
    {
        private readonly int[] front = { 4, 5 };
        private readonly int[] back = { 1, 2, 3 };
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private bool isfrontAllDie = false;
        BattleSystem battleSystem;

        public void Start()
        {
            battleSystem = GetComponent<BattleSystem>();
        }

        public LineupSlot GetFrontGuardSlot(bool isEnemy)
        {
            if (isEnemy)
            {
                if (!isfrontAllDie)
                {
                    int slotNum = front[UnityEngine.Random.Range(0, front.Length)];
                    ELinePosition pos = (ELinePosition)slotNum;

                    switch (pos)
                    {
                        case ELinePosition.FRONT1:
                            {
                                LineupSlot Front1 = battleSystem.friendlySlots[(int)ELinePosition.FRONT1].GetComponent<LineupSlot>();
                                if (Front1.character == null)
                                {
                                    LineupSlot Front2 = battleSystem.friendlySlots[(int)ELinePosition.FRONT2].GetComponent<LineupSlot>();
                                    if (Front2.character == null)
                                    {
                                        isfrontAllDie = true;
                                    }
                                    else
                                    {
                                        return Front2;
                                    }
                                }
                                else
                                {
                                    return Front1;
                                }
                                break;
                            }
                        case ELinePosition.FRONT2:
                            {
                                LineupSlot Front2 = battleSystem.friendlySlots[(int)ELinePosition.FRONT2].GetComponent<LineupSlot>();
                                if (Front2.character == null)
                                {
                                    LineupSlot Front1 = battleSystem.friendlySlots[(int)ELinePosition.FRONT1].GetComponent<LineupSlot>();
                                    if (Front1.character == null)
                                    {
                                        isfrontAllDie = true;
                                    }
                                    else
                                    {
                                        return Front1;
                                    }
                                }
                                else
                                {
                                    return Front2;
                                }
                            }
                            break;
                    }
                }

                if (isfrontAllDie)
                {

                }

            }
            else
            {

            }

            return null;
        }
    }
}