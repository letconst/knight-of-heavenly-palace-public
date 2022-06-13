using System.Collections;
using System.Collections.Generic;
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
    public static float moveSpeed = PlayerConstParams.DEFAULT_MOVESPEED;

    // プレイヤーの移動管理ステート
    public enum PlayerMoveState
    {
        None,               
        Dash,           // 走り
        Jump,           // ジャンプ
        Dodge           // 回避
    }
    public static PlayerMoveState playerMoveState = PlayerMoveState.None;
    
    // プレイヤーの攻撃モード管理ステート
    public enum PlayerAttackState
    {
        None,
        SwordPulling,   　  // 抜刀モード
        Throwing,         //　投擲モード
        SwordDelivery,     // 納刀モード
    }
    public static PlayerAttackState playerAttackState = PlayerAttackState.None;
    public static PlayerAttackState playerAttackStateL = PlayerAttackState.None;
    
}
