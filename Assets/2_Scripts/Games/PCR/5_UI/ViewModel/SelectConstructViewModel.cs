using R3;

namespace LUP.PCR
{
    public sealed class SelectConstructViewModel
    {
        public Subject<Unit> ClickBack { get; } = new();

        public Subject<BuildingType> OnBuildingSelected { get; } = new();
    }
}

