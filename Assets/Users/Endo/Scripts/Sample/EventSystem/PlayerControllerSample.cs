using UniRx;
using UnityEngine;

namespace Endo.Sample.EventSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerControllerSample : MonoBehaviour, IPlayerSample
    {
        [SerializeField, Range(.1f, 20)]
        private float moveSpeed;

        private Rigidbody _selfRig;
        private float     _prevMoveX;
        private float     _prevMoveZ;

        // ② MessageBrokerとして、イベントの仲介場所を定義する
        // こいつに対してイベントの発行 (Publish) および購読 (Receive, Subscribe) が行われる
        private readonly MessageBroker _broker = new();

        // ③ 外部からイベントの発行・購読が行えるように公開する
        public IMessageBroker Broker => _broker;

        private void Start()
        {
            _selfRig = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            InputHandler();
        }

        private void InputHandler()
        {
            (float h, float v) = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            _selfRig.velocity = new Vector3(h, 0, v) * moveSpeed;

            // ④ イベントを発行したい場所で、MessageBrokerが定義されているインスタンスから
            // イベント発行を行う
            if (_prevMoveX + _prevMoveZ > 0 && h + v == 0)
            {
                // MessageBroker.Publish((①で定義したクラス).GetEvent()) でイベント発行
                // 渡す値がある場合は GetEvent() の引数として入れる
                Broker.Publish(OnEndPlayerMove.GetEvent());
            }

            _prevMoveX = Mathf.Abs(h);
            _prevMoveZ = Mathf.Abs(v);
        }
    }
}
