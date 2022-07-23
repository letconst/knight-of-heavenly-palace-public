using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConstParams
{
    // プレイヤーのステータス数値管理クラス
    
    // プレイヤーの初期HP
    public const int DEFAULT_HP = 100;
    // プレイヤーSP (武器を投げたりするのに必要な数値)
    public const int DEFAULT_SP = 100;
    // 自動回復するSP
    public const int AUTO_RECOVERY_SP = 5;
    // 地面に設置している時の回復
    public const int GROUND_RECOVERY_SP_RATIO = 2;
    // プレイヤーの移動速度
    public const float DEFAULT_MOVE_SPEED = 10f;
    // プレイヤーのジャンプパワー
    public const float DEFAULT_JUMP_POWER = 150f;
    // プレイヤーの回避速度
    public const float DEFAULT_DODGE_SPEED = 2f;

}