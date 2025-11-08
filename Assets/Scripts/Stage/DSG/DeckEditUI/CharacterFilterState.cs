using LUP.DSG.Utils.Enums;
using System.Collections.Generic;

namespace LUP.DSG
{
    public class CharacterFilterState
    {
        public HashSet<EAttributeType> checkedAttributes = new();
        public HashSet<ERangeType> checkedRanges = new();


        public bool ContainsCheckedFilters()
        {
            return (checkedAttributes.Count > 0 || 
                    checkedRanges.Count > 0);
        }
    }
}