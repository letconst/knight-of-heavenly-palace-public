using UniRx;
using UnityEngine;

namespace Endo.Sample.PlayerInputEvent
{
    public class PlayerStateManager : MonoBehaviour
    {
        private void Start()
        {
            PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordDelivery;

            IMessageBroker inputBroker = PlayerInputEventEmitter.Instance.Broker;

            // 投擲モードへのステート切り替え入力イベントを受信
            inputBroker.Receive<PlayerEvent.Input.OnSwitchedThrow>()
                       .Subscribe(x =>
                       {
                           // x.ActionInfo.actHandに左手か右手かの情報が入ってる
                           // ステートを投擲モードに変更
                           PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.Throwing;
                       })
                       .AddTo(this);

            // 攻撃モードへのステート切り替え入力
            inputBroker.Receive<PlayerEvent.Input.OnSwitchedAttack>()
                       .Subscribe(x =>
                       {
                           // ステートを攻撃モードに変更
                       })
                       .AddTo(this);

            // 納刀モードへのステート切り替え入力
            inputBroker.Receive<PlayerEvent.Input.OnPulling>()
                       .Subscribe(_ =>
                       {
                           // ステートを納刀に変更
                           PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordDelivery;
                       })
                       .AddTo(this);

            // 抜刀モードへのステート切り替え入力
            inputBroker.Receive<PlayerEvent.Input.OnDelivery>()
                       .Subscribe(_ =>
                       {
                           // ステートを抜刀に変更
                           PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordPulling;
                       })
                       .AddTo(this);
        }
    }
}
