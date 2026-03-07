using R3;
using System;
using UnityEngine;

namespace LUP.PCR
{
    public sealed class MainUIViewModel
    {
        public ReadOnlyReactiveProperty<int> Food { get; }
        public ReadOnlyReactiveProperty<int> Power { get; }
        public ReadOnlyReactiveProperty<int> Stone { get; }
        public ReadOnlyReactiveProperty<int> Iron { get; }

        public Subject<Unit> ClickDig { get; } = new();
        public Subject<Unit> ClickConstruct { get; } = new();
        public Subject<Unit> ClickInventory { get; } = new();

        public MainUIViewModel(PCRResourceCenter rc)
        {
            Food = rc.Observe(ResourceType.Food);
            Power = rc.Observe(ResourceType.Power);
            Stone = rc.Observe(ResourceType.Stone);
            Iron = rc.Observe(ResourceType.Iron);
        }
    }
}

