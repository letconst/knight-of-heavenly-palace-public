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
        public sealed class OnSwitchedThrow : EventMessage<OnSwitchedThrow, PlayerActionInfo>
        {
            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            public PlayerActionInfo ActionInfo => param1;
        }

        /// <summary>
        /// 攻撃モード切り替え入力を行ったときのメッセージ
        /// </summary>
        public sealed class OnSwitchedAttack : EventMessage<OnSwitchedAttack, PlayerActionInfo>
        {
            /// <summary>
            /// 入力に関する情報を保持してる変数
            /// </summary>
            public PlayerActionInfo ActionInfo => param1;
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
    /// ステート変更を行うイベント 第二引数(bool)は true:フラグの追加 false:フラグの削除
    /// </summary>
    public sealed class OnStateChangeRequest : EventMessage<OnStateChangeRequest, PlayerStatus.PlayerState, bool>
    {
        public PlayerStatus.PlayerState State => param1;
        public bool IsAdd => param2;
    }

    /// <summary>
    /// 武器が着地した際のメッセージ
    /// </summary>
    public sealed class OnLandingSword : EventMessage<OnLandingSword, PlayerActionInfo>
    {
        /// <summary>
        /// 入力に関する情報を保持してる変数
        /// </summary>
        public PlayerActionInfo ActionInfo => param1;
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
