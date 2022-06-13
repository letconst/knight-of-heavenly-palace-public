using UniRx;
using UnityEngine;

namespace Endo.Sample.PlayerInputEvent
{
    public class PlayerActionController : MonoBehaviour
    {
        private void Start()
        {
            IMessageBroker inputBroker = PlayerInputEventEmitter.Instance.Broker;

            // 武器を投げる入力イベントの受信
            inputBroker.Receive<PlayerEvent.Input.OnThrowWeapon>()
                       .Subscribe(x =>
                       {
                           // x.ActionInfo.actHandに左手か右手かの情報が入ってる
                           // 武器を投げる処理
                       })
                       .AddTo(this);

            // 武器で攻撃する入力イベント
            inputBroker.Receive<PlayerEvent.Input.OnAttackWeapon>()
                       .Subscribe(x =>
                       {
                           // 武器で攻撃する処理
                       })
                       .AddTo(this);
        }
    }
}
