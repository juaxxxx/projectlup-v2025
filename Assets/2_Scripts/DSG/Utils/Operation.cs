using LUP.DSG.Utils.Enums;
using OpenCvSharp.Aruco;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public delegate float BinaryOp(float a, float b);
    static class Operation
    {
        public static readonly Dictionary<EOperationType, BinaryOp> Calc =
            new Dictionary<EOperationType, BinaryOp>
            {
                [EOperationType.Plus] = (a,b) => a + b,
                [EOperationType.Minus] = (a,b) => a - b,
                [EOperationType.Multiply] = (a,b) => a * b,
                [EOperationType.Division] = (a,b) => a / b,
                [EOperationType.Calm] = (a,b) => a = b,
            };
        public static bool TryEval(EOperationType op, float a, float b, out float result)
        {
            if(Calc.TryGetValue(op,out var f))
            {
                result = f(a, b);
                return true;
            }

            result = default;
            return false;
        }
    }
}