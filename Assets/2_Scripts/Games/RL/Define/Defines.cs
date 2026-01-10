
namespace Roguelike.Define
{
    public enum CharacterAtkRangeType
    {
        None,
        Long,
        Middle,
        Short
    }

    public enum RWeaponType
    {
        None = 0,
        Gun = 1,
        OneHandSword = 2,
        TwoHandSword = 3,
        Throw = 4,
        Magic = 5
    }

    public enum WeaponHandType
    {
        None = 0,
        OneHand = 1,
        TowHand = 2
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

    public enum RLDropItemType
    {
        Commodities = 1,
        equipment = 2,

        Max

    }

    public enum RLEquipPos
    {
        None = 0,
        Hand = 1,
        Body = 2,
        Finger = 3,
        Shorder = 4,
        Arm = 5,
        Neck = 6
    }

    public enum RLItemTier
    {
        Common = 1,
        Rare = 2,
        Epic = 3
    }

    public enum RLItemID
    {
        Wood = 10001,
        Meat = 10002,
        Coin = 10003,

        Gun_N = 20001,
        Gun_R = 20002,
        Gun_E = 20003,
        Javelin_N = 20004,

        LongSword_N = 20005,
        LongSword_R = 20006,
        LongSword_E = 20007,

        Armor_N = 20008,
        Armor_R = 20009,
        Armor_E = 20010,

        Magic_Fire = 20011,
        Magic_Acane = 20012,
        Magic_Shadow = 20013,

        Max
    }

    public enum CharacterType
    {
        F001 = 0,
        F002 = 1,
        F003 = 2,
        M001 = 3,
        M002 = 4,
        Max
    }

    public enum FloatingImageState
    {
        Sleep,
        MovingLeft,
        Pause,
        Counting,
        WaitingDisappear,
        MovingRight,
        MovingDown,
        MovingRightDown
    }
}

