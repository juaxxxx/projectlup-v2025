using UnityEngine;

[CreateAssetMenu(fileName = "Sample Item", menuName = "Inventory/Sample Item")]
public class SampleItemData : Framework.BaseItemData
{
    // 현재 BaseItemData가 익스트랙션에서 사용중임 논의해야됨

    // 부모에게 공용필드 정의
    // 추가 필드가 필요하면 여기에서 선언
    [SerializeField]
    public string description;

    // 사용가능한 아이템이면 return true 아니면 false 하세요
    public override bool CanUse()
    {
        return true;
    }

    // 반드시 재정의 후 부모함수 호출
    public override ItemRuntimeData ToSaveData()
    {
        return base.ToSaveData();
    }
}
