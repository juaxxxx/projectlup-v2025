namespace LUP
{
    /// <summary>
    /// 모든 게임의 아이템 StaticData가 구현해야 하는 인터페이스
    /// ItemManager가 다양한 게임의 아이템을 처리할 수 있도록 함
    /// </summary>
    public interface IItemStaticData : ICustomFieldSupport
    {
        /// <summary>
        /// StaticData를 런타임 ItemData로 변환
        /// </summary>
        LUPItemData ToItemData();
    }
}
