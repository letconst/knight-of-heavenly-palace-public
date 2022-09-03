using Cysharp.Threading.Tasks;
using UnityEngine;

public static class PlayerStatus
{
    // プレイヤー系統のステート管理クラス

    // プレイヤーHP
    public static int playerHp;
    // プレイヤーSP (武器を投げたりするのに必要な数値)
    public static int playerSp;
    // 自動回復するSP
    public static int recoverySp;
    // 自動回復倍率
    public static int groundedRecoverySpRatio = PlayerConstParams.GROUND_RECOVERY_SP_RATIO;

    /// <summary>
    /// プレイヤーのマスターデータ。<c>Awake()</c> や <c>Start()</c> などで参照する場合は、
    /// <see cref="WaitForLoadMasterData"/>
    /// をawaitしてから行うこと
    /// <example>
    /// <code>
    /// private void Start()
    /// {
    ///     UniTask.Create(async () =>
    ///     {
    ///        await PlayerStatus.WaitForLoadMasterData();
    ///
    ///         // PlayerStatus.playerMasterDataを参照する処理
    ///     });
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static MasterPlayer playerMasterData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static async void Initialize()
    {
        // ゲーム開始時にマスターデータを読み込む
        playerMasterData = await MasterDataManager.Instance.GetPlayerMasterDataAsync();

        playerHp   = playerMasterData.MaxHitPoint;
        playerSp   = playerMasterData.MaxStaminaPoint;
        recoverySp = playerMasterData.BaseAutoStaminaRecoveryQuantity;
    }

    public static UniTask WaitForLoadMasterData()
    {
        return UniTask.WaitWhile(() => playerMasterData == null);
    }

    // コントローラー入力のデッドゾーン設定用 プロパティ
    public static Vector2 ControllerDeadZone => new Vector3(0.2f, 0.2f, 0.2f);

    // プレイヤーの移動管理ステート
    public enum PlayerMoveState
    {
        None,
        Standing,       // 立ち
        WeaponHanging,  // ぶら下がり
        Dash,           // 走り
        Jump,           // ジャンプ
        Dodge           // 回避
    }/*
    public static PlayerMoveState playerMoveState = PlayerMoveState.None;*/

    // プレイヤーの攻撃モード管理ステート
    public enum PlayerAttackState
    {
        None,
        SwordPulling,   　    // 抜刀モード
        Attack,              // 攻撃モード
        Throwing,           //　投擲モード
        SwordDelivery,     //  納刀モード
    }
    public static PlayerAttackState playerAttackStateR = PlayerAttackState.None;
    /*public static PlayerAttackState playerAttackStateL = PlayerAttackState.None;*/

    /// <summary>
    /// Playerの各種Stateをbit演算を使って管理するenum
    /// </summary>
    [System.Flags]
    public enum PlayerState : ulong
    {
        None = 1L << 0,
        // 移動系統 (1 ~ 10)
        Standing = 1L << 1,
        Move = 1L << 2,
        Jump = 1L << 3,
        Dodge = 1L << 4,
        HangingR = 1L << 5,
        HangingL = 1L << 6,
        Falling = 1L << 7,
        TransferringR = 1L << 8,
        TransferringL = 1L << 9,
        // 共通アクション (11 ~ 20)
        SwordPulling = 1L << 11,
        SwordDelivery = 1L << 12,
        AttackMode = 1L << 13,
        ThrowingMode = 1L << 14,
        UIHandling = 1L << 15,
        // 右手の攻撃とかアクション系統 (21 ~ 30)
        AttackR = 1L << 21,
        ThrowingR = 1L << 22,
        // 左手の攻撃とかアクション系統 (31 ~ 40)
        AttackL = 1L << 31,
        ThrowingL = 1L << 32,
    }
    public static PlayerState playerState = PlayerState.None;
}

/// <summary>
/// ステートの変更をする際に行う挙動を管理するステート
/// </summary>
public enum PlayerStateChangeOptions
{
    None,
    Add,            // 追加
    Delete,         // 削除
    Switching       // 切り替え
}
