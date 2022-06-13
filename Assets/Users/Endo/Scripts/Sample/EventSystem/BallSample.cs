using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Endo.Sample.EventSystem
{
    public class BallSample : MonoBehaviour
    {
        [SerializeField]
        private PlayerControllerSample player;
        
        [SerializeField]
        private float moveRange;

        [SerializeField]
        private float moveSpeed;

        private Vector3 _basePos;

        private void Start()
        {
            _basePos = transform.position;

            // ④ イベントを発行したい場所で、MessageBrokerが定義されているインスタンスから
            // イベント発行を行う
            this.OnCollisionEnterAsObservable()
                .Subscribe(x =>
                {
                    var p = x.collider.GetComponent<IPlayerSample>();
                    
                    if (p == null) return;
                    
                    // MessageBroker.Publish((①で定義したクラス).GetEvent()) でイベント発行
                    // 渡す値がある場合は GetEvent() の引数として入れる
                    player.Broker.Publish(OnPlayerDamagedSample.GetEvent(10));
                });
        }

        private void Update()
        {
            Animate();
        }

        private void Animate()
        {
            float sin      = Mathf.Sin(2 * Mathf.PI * moveSpeed * Time.time);
            float deltaPos = sin * moveRange;

            Vector3 curPos = transform.position;
            float   newX   = float.IsNaN(_basePos.x + deltaPos) ? _basePos.x : _basePos.x + deltaPos;
            transform.position = new Vector3(newX, curPos.y, curPos.z);
        }
    }
}
