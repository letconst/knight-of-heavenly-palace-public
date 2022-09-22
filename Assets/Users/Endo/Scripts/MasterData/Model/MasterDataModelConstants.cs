public static class MasterDataModelConstants
{
    public static class Player
    {
        public const string MaxHitPointLabel = "最大HP";

        public const string MaxStaminaPointLabel = "最大SP";

        public const string BaseJumpPowerLabel = "基本ジャンプ力";

        public const string BaseAutoStaminaRecoveryQuantity = "未張り付き時の自動SP回復量 (秒間)";

        public const string BaseMoveSpeedLabel = "基本移動速度 (m/s)";

        public const string AttackIntervalLabel = "攻撃を行う間隔";

        public const string AttackTargetFindRadiusLabel = "張り付き状態での攻撃時に攻撃対象を探す半径";

        public const string DodgeIntervalLabel = "回避を行う間隔";

        public const string NormalDodgeInfoLabel = "通常回避";

        public const string InAirDodgeInfoLabel = "空中回避";

        public const string EscapingDodgeInfoLabel = "離脱回避";
    }

    public static class Enemy
    {
        public const string NameLabel = "名前";

        public const string BodySizeLabel = "体型";

        public const string MaxHitPointLabel = "最大体力";

        public const string BaseAttackPowerLabel = "ベースとなる攻撃力 (ダメージ)";
    }

    public static class Weapon
    {
        public const string BaseAttackPowerLabel = "ベースとなる攻撃力 (ダメージ)";

        public const string BaseComboRatioLabel = "ベースとなるコンボ倍率";

        public const string BaseAttackSpeedLabel = "攻撃速度";

        public const string BaseAttackRangeLabel = "攻撃範囲 (m)";

        public const string CanThrowOnAttackModeLabel = "攻撃モード時に投擲できるか";

        public const string IsMagicWeaponLabel = "魔法武器か";

        public const string MaxMagicPowerLabel = "最大魔力";
    }

    public static class DodgeInfo
    {
        public const string DodgeSpeedLabel = "回避速度";

        public const string RequireMagicPowerLabel = "消費魔力";

        public const string RequireStaminaPointLabel = "消費SP";

        public const string AcquireMagicPowerLabel = "獲得魔力";

        public const string InvincibilityFrameLabel = "回避後の無敵フレーム";

        public const string FlinchFrameLabel = "回避後の硬直フレーム";
    }

    public static class Mission
    {
        public const string MissionNameLabel = "依頼名";

        public const string InitGeneratePrefabLabel = "初期生成するプレハブ";

        public const string ContentImage = "選択時に表示する内容の画像";
    }
}
