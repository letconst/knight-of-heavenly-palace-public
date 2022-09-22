using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーの移動関連を処理するクラス
/// </summary>
public partial class PlayerMovement : MonoBehaviour
{
    // 移動関係
    [SerializeField, Header("移動させるオブジェクト")]
    private GameObject _playerObject;
    [SerializeField, Header("正面設定を行うカメラオブジェクト")]
    private GameObject _cameraObject;

    private Rigidbody rigidbody;

    // プレイヤーの計算後速度(向きとか)
    private Vector3 _moveForward = Vector3.zero;

    private bool _isDead;

    void Start()
    {
        rigidbody = _playerObject.GetComponent<Rigidbody>();

        IMessageBroker inputBroker = PlayerInputEventEmitter.Instance.Broker;

        // イベント受信後の処理
        // ジャンプ
        inputBroker.Receive<PlayerEvent.Input.OnJump>()
            .Subscribe(x =>
            {
                Jump(PlayerStatus.playerMasterData.BaseJumpPower, rigidbody);

                // ステートチェンジイベントの発行
                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Jump, PlayerStateChangeOptions.Add,
                        null, null));
            }).AddTo(this);

        // 回避
        inputBroker.Receive<PlayerEvent.Input.OnDodge>()
            .Subscribe(async x =>
            {
                float dodgeSpeed = 0f;
                int reduceNum = 0;

                if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
                    PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL))
                {
                    dodgeSpeed = PlayerStatus.playerMasterData.NormalDodgeInfo.DodgeSpeed;
                }
                // 地上にいる場合の回避速度
                else if (PlayerGrounded.isGrounded)
                {
                    dodgeSpeed = PlayerStatus.playerMasterData.NormalDodgeInfo.DodgeSpeed;
                }
                // 空中にいる場合の回避速度
                else if (!PlayerGrounded.isGrounded)
                {
                    dodgeSpeed = PlayerStatus.playerMasterData.InAirDodgeInfo.DodgeSpeed;
                }

                Dodge(dodgeSpeed, rigidbody, x.reduceStamina.Value);

                // ステートチェンジイベントの発行
                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Dodge, PlayerStateChangeOptions.Add,
                        null,null));

                await UniTask.Delay(1000);

                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Dodge, PlayerStateChangeOptions.Delete,
                        null,null));
            }).AddTo(this);

        // 死亡イベントが飛ばされた時の死亡処理
        inputBroker.Receive<OnStatusDestroy>()
                   .Where(_ => !_isDead)
                   .Subscribe(_ => Death())
                   .AddTo(this);
    }

    void FixedUpdate()
    {
        // 毎フレーム初期化 (stoppingの判定をとるため)
        _moveForward = Vector3.zero;

        // ぶら下がり・転移・UI操作・死んでいる状態のときは移動を受け付けない
        if (!PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.TransferringL) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.TransferringR) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.UIHandling)　&&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.Dead))
        {
            if (PlayerStatus.playerMasterData)
            {
                // 移動処理
                Moved(PlayerStatus.playerMasterData.BaseMoveSpeed, rigidbody);
            }
        }

        // 落下し始めた際にイベント発行
        if (!PlayerStateManager.HasFlag(PlayerStatus.PlayerState.Falling) && // 落下フラグなし
            !PlayerGrounded.isGrounded                                    && // 空中判定
            rigidbody.velocity.y < 0)                                        // 移動推力が下方向
        {
            PlayerInputEventEmitter.Instance.Broker.Publish(
                PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Falling, PlayerStateChangeOptions.Add,
                    null, null));

            // TODO: OnStateChangeRequestのコールバックとかにしたい (ステート変更に成功したかわからないため)
            PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnFalling.GetEvent());
        }
    }
}
