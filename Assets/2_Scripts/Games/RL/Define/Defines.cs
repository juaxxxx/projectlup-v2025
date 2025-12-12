
namespace Roguelike.Define
{
    public enum CharacterType
    {
        None,
        Long,
        Middle,
        Short
    }

    public enum WeaponType
    {
        None,
        Gun,
        OneHandSword,
        TwoHandSword,
        Throw,
        Magic
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
        public const string LobbyScene = "RKLobbyScene";
        public const string GameScene = "RKInGameScene";
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

    public enum ItemType
    {
        Commodities = 1,
        equipment = 2,

        Max

    }
    public enum RLSpanableItem
    {
        Wood = 10001,
        Meat = 10002,
        Coin = 10003,

        Gun_N = 20001,
        Javelin_N = 20002,

        Max
    }
}

