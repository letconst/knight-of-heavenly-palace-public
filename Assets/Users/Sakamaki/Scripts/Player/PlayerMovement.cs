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

    [Header("回避処理部分")]
    [Tooltip("回避を行う際に必要なスタミナ")] public static int _requestDodgeSp = 5;
    [Tooltip("回避が可能になる間隔")] public static float _dodgeIntervalTime = 1f;
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
                Jump(PlayerStatus.jumpPower, rigidbody);

                // ステートチェンジイベントの発行
                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Jump, true));
            }).AddTo(this);

        // 回避
        inputBroker.Receive<PlayerEvent.Input.OnDodge>()
            .Subscribe(x =>
            {
                Dodge(PlayerStatus.dodgeSpeed, rigidbody);

                // ステートチェンジイベントの発行
                inputBroker.Publish(PlayerEvent.OnStateChangeRequest
                    .GetEvent(PlayerStatus.PlayerState.Dodge, true));
            }).AddTo(this);
    }

    void FixedUpdate()
    {
        // 毎フレーム初期化 (stoppingの判定をとるため)
        _moveForward = Vector3.zero;

        // ぶら下がり状態のときは移動を受け付けない
        if (!PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR))
        {
            // 移動処理
            Moved(PlayerStatus.moveSpeed, rigidbody);
        }

        // 落下し始めた際にイベント発行
        if (!PlayerStateManager.HasFlag(PlayerStatus.PlayerState.Falling) && // 落下フラグなし
            !PlayerGrounded.isGrounded                                    && // 空中判定
            rigidbody.velocity.y < 0)                                        // 移動推力が下方向
        {
            PlayerInputEventEmitter.Instance.Broker.Publish(
                PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Falling, true));

            // TODO: OnStateChangeRequestのコールバックとかにしたい (ステート変更に成功したかわからないため)
            PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnFalling.GetEvent());
        }
    }
}
