using UnityEngine;

/// <summary>
/// Playerイベントの定義を行うクラス
/// </summary>
public static class PlayerEvent
{
    public static class Input
    {
        /// <summary>
        /// 投擲モード切り替え入力を行ったときのメッセージ
        /// </summary>
        public sealed class OnSwitchedThrow : EventMessage<OnSwitchedThrow /*, PlayerActionInfo*/>
        {
            // 両方の手モード変更を行うので左右の情報いらないと思ったためコメントアウト

            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            /*public PlayerActionInfo ActionInfo => param1;*/
        }

        /// <summary>
        /// 攻撃モード切り替え入力を行ったときのメッセージ
        /// </summary>
        public sealed class OnSwitchedAttack : EventMessage<OnSwitchedAttack /*, PlayerActionInfo*/>
        {
            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            /*public PlayerActionInfo ActionInfo => param1;*/
        }

        /// <summary>
        /// 武器の投擲入力を行ったときのメッセージ
        /// </summary>
        public sealed class OnThrowWeapon : EventMessage<OnThrowWeapon, PlayerActionInfo>
        {
            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            public PlayerActionInfo ActionInfo => param1;
        }

        /// <summary>
        /// 武器での攻撃入力をした際のメッセージ
        /// </summary>
        public sealed class OnAttackWeapon : EventMessage<OnAttackWeapon, PlayerActionInfo, JoyConAngleCheck.Position>
        {
            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            public PlayerActionInfo ActionInfo => param1;

            /// <summary>
            /// 攻撃する方向
            /// </summary>
            public JoyConAngleCheck.Position AttackDirection => param2;
        }

        /// <summary>
        /// 抜刀入力をした際のメッセージ
        /// </summary>
        public sealed class OnPulling : EventMessage<OnPulling>
        {
        }

        /// <summary>
        /// 抜刀入力をした際のメッセージ
        /// </summary>
        public sealed class OnDelivery : EventMessage<OnDelivery>
        {
        }

        /// <summary>
        /// ジャンプ入力をした際のメッセージ
        /// </summary>
        public sealed class OnJump : EventMessage<OnJump>
        {
        }

        /// <summary>
        /// 回避入力をした際のメッセージ
        /// </summary>
        public sealed class OnDodge : EventMessage<OnDodge>
        {
        }
    }

    /// <summary>
    /// ステート変更を行うイベント
    /// </summary>
    public sealed class OnStateChangeRequest : EventMessage<OnStateChangeRequest, PlayerStatus.PlayerState,
        PlayerStateChangeOptions, System.Action, System.Action>
    {
        // 移動先のステート
        public PlayerStatus.PlayerState State => param1;

        // 第二引数、ステート変更のenum (Add:追加 Delete:削除 Changed:変更)
        public PlayerStateChangeOptions StateChangeOptions => param2;

        // 第三は変更された時、第四は実行されなかった時
        public System.Action OnChanged  => param3;
        public System.Action OnRejected => param4;
    }

    /// <summary>
    /// ステート変更が行われた時のイベント (変更後のイベント)
    /// </summary>
    public sealed class OnStateChanged : EventMessage<OnStateChanged, PlayerStatus.PlayerState>
    {
        public PlayerStatus.PlayerState State => param1;
    }

    public sealed class OnBeginThrowSword : EventMessage<OnBeginThrowSword, PlayerInputEvent.PlayerHand, RaycastHit, Transform,
        float>
    {
        /// <summary>
        /// 投げた手
        /// </summary>
        public PlayerInputEvent.PlayerHand Hand => param1;

        /// <summary>
        /// カーソル先にあったオブジェクトへのヒット情報
        /// </summary>
        public RaycastHit Hit => param2;

        /// <summary>
        /// 親オブジェクト
        /// </summary>
        public Transform Parent => param3;

        /// <summary>
        /// 投擲速度
        /// </summary>
        public float Speed => param4;
    }

    /// <summary>
    /// 武器が着地した際のメッセージ
    /// </summary>
    public sealed class OnLandingSword : EventMessage<OnLandingSword, Vector3, float, PlayerActionInfo>
    {
        /// <summary>
        /// 着地位置
        /// </summary>
        public Vector3 LandingPosition => param1;

        /// <summary>
        /// 着地した角度
        /// </summary>
        public float LandingAngle => param2;

        /// <summary>
        /// 入力に関する情報を保持してる変数
        /// </summary>
        public PlayerActionInfo ActionInfo => param3;
    }

    /// <summary>
    /// 武器が着弾している時に剣を手物に戻すメッセージ
    /// </summary>
    public sealed class OnWallResetSword : EventMessage<OnWallResetSword, PlayerActionInfo>
    {
        public PlayerActionInfo ActionInfo => param1;
    }

    /// <summary>
    /// 移動ステートが切り替わった際のメッセージ
    /// </summary>
    public sealed class OnChangedMoveState : EventMessage<OnChangedMoveState, PlayerStatus.PlayerMoveState>
    {
        /// <summary>
        /// 次のステート
        /// </summary>
        public PlayerStatus.PlayerMoveState NextState => param1;
    }

    /// <summary>
    /// 攻撃ステートが切り替わった際のメッセージ
    /// </summary>
    public sealed class OnChangedAttackState : EventMessage<OnChangedAttackState, PlayerStatus.PlayerMoveState>
    {
        /// <summary>
        /// 次のステート
        /// </summary>
        public PlayerStatus.PlayerMoveState NextState => param1;
    }

    /// <summary>
    /// 敵からダメージを受けた際のメッセージ
    /// </summary>
    public sealed class OnHitEnemyAttack : EventMessage<OnHitEnemyAttack, int>
    {
        /// <summary>
        /// ダメージ量
        /// </summary>
        public int DamageValue => param1;
    }

    /// <summary>
    /// プレイヤーが着地した際のメッセージ
    /// </summary>
    public sealed class OnLanding : EventMessage<OnLanding, float>
    {
        /// <summary>滞空していた時間 (秒)</summary>
        public float InAirTime => param1;
    }

    /// <summary>
    /// プレイヤーが落下し始めた際のメッセージ
    /// </summary>
    public sealed class OnFalling : EventMessage<OnFalling>
    {
    }
}
