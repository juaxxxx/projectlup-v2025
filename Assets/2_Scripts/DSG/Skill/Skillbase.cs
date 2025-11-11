using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using LUP.DSG.Utils;
using LUP.DSG.Utils.Enums;

namespace LUP.DSG
{
    public abstract class Skillbase : MonoBehaviour
    {
        void Excute(List<Character> Targets, IStatusEffect StatusEffect) { }
    }
}