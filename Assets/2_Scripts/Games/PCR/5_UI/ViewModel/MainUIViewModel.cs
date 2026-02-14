using R3;
using System;
using UnityEngine;

namespace LUP.PCR
{
    public class MainUIViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<int> Food { get; }
        public ReadOnlyReactiveProperty<int> Power { get; }
        public ReadOnlyReactiveProperty<int> Stone { get; }
        public ReadOnlyReactiveProperty<int> Iron { get; }

        // UI Command
        public Subject<Unit> ClickDig { get; } = new();
        public Subject<Unit> ClickConstruct { get; } = new();
        public Subject<Unit> ClickInventory { get; } = new();

        private readonly CompositeDisposable cd = new();

        public MainUIViewModel(PCRResourceCenter rc)
        {
            Food = rc.Observe(ResourceType.Food);
            Power = rc.Observe(ResourceType.Power);
            Stone = rc.Observe(ResourceType.Stone);
            Iron = rc.Observe(ResourceType.Iron);
        }

        public void Dispose() => cd.Dispose();
    }
}

