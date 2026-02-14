using R3;

namespace LUP.PCR
{
    public sealed class SelectConstructViewModel
    {
        public Subject<Unit> ClickBack { get; } = new();

        // 건설 종류 선택
        public Subject<BuildingType> SelectBuildType { get; } = new();

        // 필요하면 현재 선택 상태를 UI에 보여주기 위해 상태도 제공
        public ReactiveProperty<BuildingType> CurrentSelection { get; } = new(BuildingType.NONE);
    }
}

