namespace Endo.Sample.PlayerInputEvent
{
    /// <summary>
    /// プレイヤーに関するEventMessageをまとめたクラス
    /// <remarks>子クラスとして各種イベントを定義していく</remarks>
    /// </summary>
    public sealed class PlayerEvent
    {
        public sealed class Input
        {
            /// <summary>
            /// 投擲モードへの切り替え入力した際のメッセージ
            /// </summary>
            public sealed class OnSwitchedThrow : EventMessage<OnSwitchedThrow, PlayerActionInfo>
            {
                /// <summary>入力に関する情報</summary>
                public PlayerActionInfo ActionInfo => param1;
            }

            /// <summary>
            /// 攻撃モードへの切り替え入力した際のメッセージ
            /// </summary>
            public sealed class OnSwitchedAttack : EventMessage<OnSwitchedAttack, PlayerActionInfo>
            {
                /// <summary>入力に関する情報</summary>
                public PlayerActionInfo ActionInfo => param1;
            }

            /// <summary>
            /// 武器の投擲入力した際のメッセージ
            /// </summary>
            public sealed class OnThrowWeapon : EventMessage<OnThrowWeapon, PlayerActionInfo>
            {
                /// <summary>入力に関する情報</summary>
                public PlayerActionInfo ActionInfo => param1;
            }

            /// <summary>
            /// 武器での攻撃入力した際のメッセージ
            /// </summary>
            public sealed class OnAttackWeapon : EventMessage<OnAttackWeapon, PlayerActionInfo>
            {
                /// <summary>入力に関する情報</summary>
                public PlayerActionInfo ActionInfo => param1;
            }

            /// <summary>
            /// 抜刀入力した際のメッセージ
            /// </summary>
            public sealed class OnPulling : EventMessage<OnPulling>
            {
            }

            /// <summary>
            /// 納刀入力した際のメッセージ
            /// </summary>
            public sealed class OnDelivery : EventMessage<OnDelivery>
            {
            }

            /// <summary>
            /// ジャンプ入力した際のメッセージ
            /// </summary>
            public sealed class OnJump : EventMessage<OnJump>
            {
            }

            /// <summary>
            /// 回避入力した際のメッセージ
            /// </summary>
            public sealed class OnDodge : EventMessage<OnDodge>
            {
            }
        }

        /// <summary>
        /// 移動ステートが切り替わった際のメッセージ
        /// </summary>
        public sealed class OnChangedMoveState : EventMessage<OnChangedMoveState, PlayerStatus.PlayerMoveState>
        {
            /// <summary>次のステート</summary>
            public PlayerStatus.PlayerMoveState NextState => param1;
        }

        /// <summary>
        /// 攻撃ステートが切り替わった際のメッセージ
        /// </summary>
        public sealed class OnChangedAttackState : EventMessage<OnChangedAttackState, PlayerStatus.PlayerAttackState>
        {
            /// <summary>次のステート</summary>
            public PlayerStatus.PlayerAttackState NextState => param1;
        }

        /// <summary>
        /// 魔物からダメージを受けた際のメッセージ
        /// </summary>
        public sealed class OnHitEnemyAttack : EventMessage<OnHitEnemyAttack, int>
        {
            /// <summary>ダメージ量</summary>
            public int DamageValue => param1;
        }
    }
}
