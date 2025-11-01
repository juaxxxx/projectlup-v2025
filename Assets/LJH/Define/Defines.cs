
namespace Roguelike.Define
{
    public enum CharacterType
    {
        None,
        Long,
        Middle,
        Short
    }
    public enum DisplayableDataType
    {
        None,
        CharacterData,
        ChapterData,
        ItemData,
        BuffData
    }

    public static class RoguelikeScene
    {
        public const string LobbyScene = "LobbyScene";
        public const string GameScene = "InGameScene";
        public const string ResultScene = "ResultScene";
    }

    public enum LayoutDirection
    {
        None,
        Horizontal,
        Vertical,
        Grid
    }

    public enum BuffType
    {
        None,

        AddAtkLow,
        AddAtkMiddle,
        AddAtkHigh,

        AddSpeed,

        AddMaxHp,

        Max
    }

    public enum ButtonRole
    {
        None,

        //Lobby
        BackToMainBtn,
        ChapterSelectionBtn,
        CharacterSelectionBtn,
        QuestselectionBtn,
        GameStartBtn,
        ShopBuyBtn1,
        ShopButBtn2,

        ShopTabBtn1,
        ShopTabBtn2,
        ShopTabBtn3,

        InventoryAlignBtn,
        BagPreferBtn,
        BlackSmithBtn,

        //InGame
        BackToLobbyBtn,
        BackToGameBtn,
        PauseGameBtn


    }
}

