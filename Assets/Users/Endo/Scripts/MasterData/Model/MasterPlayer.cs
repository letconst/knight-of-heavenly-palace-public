using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "マスターデータ/プレイヤー")]
public sealed partial class MasterPlayer : MasterDataBase
{
    #region ステータス情報

    [SerializeField, Header(MasterDataModelConstants.Player.MaxHitPointLabel)]
    private int maxHitPoint;

    /// <summary>最大体力</summary>
    public int MaxHitPoint => maxHitPoint;

    [SerializeField, Header(MasterDataModelConstants.Player.MaxStaminaPointLabel)]
    private int maxStaminaPoint;

    /// <summary>最大SP</summary>
    public int MaxStaminaPoint => maxStaminaPoint;

    [SerializeField, Header(MasterDataModelConstants.Player.BaseJumpPowerLabel)]
    private float baseJumpPower;

    /// <summary>ジャンプ力</summary>
    public float BaseJumpPower => baseJumpPower;

    [SerializeField, Header(MasterDataModelConstants.Player.BaseAutoStaminaRecoveryQuantity)]
    private int baseAutoStaminaRecoveryQuantity;

    /// <summary>未張り付き時の自動SP回復量 (秒間)</summary>
    public int BaseAutoStaminaRecoveryQuantity => baseAutoStaminaRecoveryQuantity;

    [SerializeField, Header(MasterDataModelConstants.Player.BaseMoveSpeedLabel)]
    private float baseMoveSpeed;

    /// <summary>移動速度 (m/s)</summary>
    public float BaseMoveSpeed => baseMoveSpeed;

    #endregion

    #region 攻撃情報

    [SerializeField, Header(MasterDataModelConstants.Player.AttackIntervalLabel)]
    private float attackInterval;

    /// <summary>攻撃を行う間隔</summary>
    public float AttackInterval => attackInterval;

    [SerializeField]
    private float attackTargetFindRadius;

    /// <summary>張り付き状態での攻撃時に攻撃対象を探す半径</summary>
    public float AttackTargetFindRadius => attackTargetFindRadius;

    #endregion

    #region 回避情報

    [SerializeField, Header(MasterDataModelConstants.Player.DodgeIntervalLabel)]
    private float dodgeInterval;

    /// <summary>回避を行う間隔</summary>
    public float DodgeInterval => dodgeInterval;

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

    #endregion

    [System.Serializable]
    public sealed class DodgeInformation
    {
        [SerializeField, Header(MasterDataModelConstants.DodgeInfo.DodgeSpeedLabel)]
        private float dodgeSpeed;

        /// <summary>回避速度</summary>
        public float DodgeSpeed => dodgeSpeed;

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
