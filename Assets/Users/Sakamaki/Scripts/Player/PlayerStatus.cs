using UnityEngine;

public static class PlayerStatus
{
    // プレイヤー系統のステート管理クラス

    // プレイヤーHP
    public static int playerHp = PlayerConstParams.DEFAULT_HP;
    // プレイヤーSP (武器を投げたりするのに必要な数値)
    public static int playerSp = PlayerConstParams.DEFAULT_SP;
    // 自動回復するSP
    public static int recoverySp = PlayerConstParams.AUTO_RECOVERY_SP;
    // 自動回復倍率
    public static int groundedRecoverySpRatio = PlayerConstParams.GROUND_RECOVERY_SP_RATIO;
    // プレイヤーの移動速度　
    public static float moveSpeed = PlayerConstParams.DEFAULT_MOVE_SPEED;
    // プレイヤーのジャンプパワー
    public static float jumpPower = PlayerConstParams.DEFAULT_JUMP_POWER;
    // プレイヤーの回避速度
    public static float dodgeSpeed = PlayerConstParams.DEFAULT_DODGE_SPEED;

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
        // 共通アクション (11 ~ 20)
        SwordPulling = 1L << 11,
        SwordDelivery = 1L << 12,
        // 攻撃とかアクション系統 (21 ~ 30)
        AttackR = 1L << 21,
        ThrowingR = 1L << 22,
        // 攻撃とかアクション系統 (31 ~ 40)
        AttackL = 1L << 31,
        ThrowingL = 1L << 32,
    }
    public static PlayerState playerState = PlayerState.None;
}
