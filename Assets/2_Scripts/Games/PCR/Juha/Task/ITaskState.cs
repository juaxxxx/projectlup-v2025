using UnityEngine;

namespace LUP.PCR
{
    public interface ITaskState
    {
        public void InputHandle();
        public void Open();
        public void Close();
    }
}


