using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "マスターデータ/プレイヤー")]
public sealed class MasterPlayer : MasterDataBase
{
    [SerializeField, Header(MasterDataModelConstants.Player.MaxHitPointLabel)]
    private int maxHitPoint;

    /// <summary>最大体力</summary>
    public int MaxHitPoint => maxHitPoint;

    [SerializeField, Header(MasterDataModelConstants.Player.MaxStaminaPointLabel)]
    private int maxStaminaPoint;

    /// <summary>最大SP</summary>
    public int MaxStaminaPoint => maxStaminaPoint;

    [SerializeField, Header(MasterDataModelConstants.Player.BaseAutoStaminaRecoveryQuantity)]
    private int baseAutoStaminaRecoveryQuantity;

    /// <summary>未張り付き時の自動SP回復量 (秒間)</summary>
    public int BaseAutoStaminaRecoveryQuantity => baseAutoStaminaRecoveryQuantity;

    [SerializeField, Header(MasterDataModelConstants.Player.BaseMoveSpeedLabel)]
    private float baseMoveSpeed;

    /// <summary>移動速度 (m/s)</summary>
    public float BaseMoveSpeed => baseMoveSpeed;

    [SerializeField, Header(MasterDataModelConstants.Player.NormalDodgeInfoLabel)]
    private DodgeInformation normalDodgeInfo;

    /// <summary>通常回避に関する情報</summary>
    public DodgeInformation NormalDodgeInfo => normalDodgeInfo;

    [SerializeField, Header(MasterDataModelConstants.Player.InAirDodgeInfoLabel)]
    private DodgeInformation inAirDodgeInfo;

    /// <summary>空中回避に関する情報</summary>
    public DodgeInformation InAirDodgeInfo => inAirDodgeInfo;

    [SerializeField, Header(MasterDataModelConstants.Player.EscapingDodgeInfoLabel)]
    private DodgeInformation escapingDodgeInfo;

    /// <summary>離脱回避に関する情報</summary>
    public DodgeInformation EscapingDodgeInfo => escapingDodgeInfo;

    [System.Serializable]
    public sealed class DodgeInformation
    {
        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.MotionSpeedLabel)]
        private float motionSpeed;

        /// <summary>モーション速度 (f)</summary>
        public float MotionSpeed => motionSpeed;

        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.RequireMagicPowerLabel)]
        private int requireMagicPower;

        /// <summary>消費魔力</summary>
        public int RequireMagicPower => requireMagicPower;

        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.RequireStaminaPointLabel)]
        private int requireStaminaPoint;

        /// <summary>消費SP</summary>
        public int RequireStaminaPoint => requireStaminaPoint;

        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.AcquireMagicPowerLabel)]
        private int acquireMagicPower;

        /// <summary>獲得魔力</summary>
        public int AcquireMagicPower => acquireMagicPower;

        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.InvincibilityFrameLabel)]
        private int invincibilityFrame;

        /// <summary>回避後の無敵フレーム</summary>
        public int InvincibilityFrame => invincibilityFrame;

        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.FlinchFrameLabel)]
        private int flinchFrame;

        /// <summary>回避後の硬直フレーム</summary>
        public int FlinchFrame => flinchFrame;
    }
}
