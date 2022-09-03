using System;
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
            .Subscribe(x =>
            {
                float dodgeSpeed = 0f;

                // 地上にいる場合の回避速度
                if (PlayerGrounded.isGrounded)
                {
                    dodgeSpeed = PlayerStatus.playerMasterData.NormalDodgeInfo.DodgeSpeed;
                }
                // 空中にいる場合の回避速度
                else
                {
                    dodgeSpeed = PlayerStatus.playerMasterData.InAirDodgeInfo.DodgeSpeed;
                }

                Dodge(dodgeSpeed, rigidbody);

                // ステートチェンジイベントの発行
                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Dodge, PlayerStateChangeOptions.Add,
                        null,null));
            }).AddTo(this);
    }

    void FixedUpdate()
    {
        // 毎フレーム初期化 (stoppingの判定をとるため)
        _moveForward = Vector3.zero;

        // ぶら下がり・転移・UI操作状態のときは移動を受け付けない
        if (!PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.TransferringL) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.TransferringR) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.UIHandling))
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
