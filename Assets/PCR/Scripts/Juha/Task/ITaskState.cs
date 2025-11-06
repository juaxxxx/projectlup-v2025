using UnityEngine;

public interface ITaskState
{
    public void InputHandle(TaskController controller);
    public void Open(TaskController controller);
    public void Close(TaskController controller);
}
