using R3;
using UnityEngine;

namespace LUP.PCR
{
    public sealed class ConstructionDecisionViewModel
    {
        public Subject<Unit> OnClickAccept { get; } = new();
        public Subject<Unit> OnClickReject { get; } = new();
    }
}
