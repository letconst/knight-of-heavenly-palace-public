using UniRx;
using UnityEngine;

namespace Endo.Sample.EventSystem
{
    public class GameControllerSample : MonoBehaviour
    {
        [SerializeField]
        private PlayerControllerSample player;

        private void Start()
        {
            // ⑤ イベント発行を受け取りたい場所で、MessageBrokerが定義されているインスタンスから
            // 購読を行う
            player.Broker.Receive<OnEndPlayerMove>()
                  .Subscribe(_ =>
                  {
                      // パラメータを受け取らないイベントは↑の引数を _ にする
                      // ここにイベントを受け取った際の処理を書く
                      Debug.Log("プレイヤーが止まりました");
                  });

            player.Broker.Receive<OnPlayerDamagedSample>()
                  .Subscribe(x =>
                  {
                      // ↑のx (名前は自由でOK) にイベント用クラスが入ってくる
                      // ここにイベントを受け取った際の処理を書く
                      Debug.Log($"プレイヤーが{x.DamageValue}ダメージを受けました");
                  });
        }
    }
}
