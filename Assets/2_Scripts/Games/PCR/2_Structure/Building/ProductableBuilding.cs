using NUnit.Framework.Constraints;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUP.PCR
{
    public abstract class ProductableBuilding : BuildingBase
    {
        // 읽기전용 데이터
        public PCRProductionStaticData currentProductionData;

        protected ProductionInfo productionInfo;

        protected IBuildState constructState;
        protected IBuildState productableState;

        public int maxStorage;
        
        public abstract void SetupProductionData();

        public abstract void StartProduction();

        public abstract void StopProduction();

        public abstract void CompleteProduction();

        public ProductionInfo GetProductionInfo()
        {
            return productionInfo;
        }

        public void SetProductionInfo(ProductionInfo info)
        {
            productionInfo = info;
        }
    }

}
