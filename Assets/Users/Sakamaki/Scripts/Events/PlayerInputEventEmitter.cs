using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// JoyConの入力を習得し、習得した状態に応じてイベントを発行するクラス
/// </summary>
public class PlayerInputEventEmitter : SingletonMonoBehaviour<PlayerInputEventEmitter>
{
    // SwitchInputManagerのキャッシュ用変数
    private SwitchInputManager _switchInput;

    // イベント通知送受信用
    private readonly MessageBroker _broker = new MessageBroker();

    // イベント通知送受信の公開用
    public IMessageBroker Broker => _broker;

    private float _dodgeCheckTime = 0f;
    private bool _isTimerActive = false;

    private void Start()
    {
        // インスタンスの初期化
        _switchInput = SwitchInputManager.Instance;

        // OnDestroyAsObservableはクラスが破棄(Destroy)された時に
        // Subscribe(_broker.Dispose)の中身の処理を行う(デストラクターみたいなもん)
        // UniRxでイベント受信の処理を書くとこのオブジェクト自体が破棄(Destroy)
        // された時でも動いてしまうため、_brokerを破棄する
        this.OnDestroyAsObservable()
            .Subscribe(_ => _broker.Dispose())
            .AddTo(this);
    }

    private void Update()
    {
        InputHandler();
        Timer();
    }

    /// <summary>
    /// JoyConの入力とプレイヤーの状態に応じて、分岐と入力操作のイベントを発行する関数
    /// </summary>
    private void InputHandler()
    {
        // 左手の入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.L))
        {
            // イベント発行が可能かどうかの判別

            // UI操作中かどうかの判別
            // if (/* UI操作中か */) return;

            // 発行可能なら、準備をして発行

            // 入力情報の用意
            var actionInfo = new PlayerActionInfo
            {
                actHand = PlayerInputEvent.PlayerHand.Left
            };

            // 左手の投擲モードなら
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.ThrowingL))
            {
                // イベントを発行(通知を行う)
                _broker.Publish(PlayerEvent.Input.OnSwitchedThrow.GetEvent(actionInfo));
            }
            // 攻撃モードなら
            else if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.AttackL))
            {
                // イベントを発行
                _broker.Publish(PlayerEvent.Input.OnSwitchedThrow.GetEvent(actionInfo));
            }
        }
        // 右入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.R))
        {
            /* イベントを発行可能（操作を実行可能）かを、まず確認する */

            // UI操作中なら終了
            // if (/* UI操作中か */) return;

            var actionInfo = new PlayerActionInfo()
            {
                actHand = PlayerInputEvent.PlayerHand.Right
            };

            // 投擲モードなら
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.ThrowingR))
            {
                _broker.Publish(PlayerEvent.Input.OnSwitchedAttack.GetEvent(actionInfo));
            }
            // 攻撃モードなら
            else if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.AttackR))
            {
                _broker.Publish(PlayerEvent.Input.OnSwitchedThrow.GetEvent(actionInfo));
            }
        }
        // ZLの入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.ZL))
        {
            var actionInfo = new PlayerActionInfo()
            {
                actHand = PlayerInputEvent.PlayerHand.Left
            };

            // 壁張り付きしていたらリセット処理(剣を離す)
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL))
            {
                _broker.Publish(PlayerEvent.OnWallResetSword.GetEvent(actionInfo));
            }
            // それ以外は剣を投げる処理
            else
            {
                OnZLZRInput(actionInfo);
            }
        }
        // ZRの入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.ZR))
        {
            var actionInfo = new PlayerActionInfo()
            {
                actHand = PlayerInputEvent.PlayerHand.Right
            };

            // 壁張り付きしていたらリセット処理(剣を離す)
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR))
            {
                _broker.Publish(PlayerEvent.OnWallResetSword.GetEvent(actionInfo));
            }
            else
            {
                OnZLZRInput(actionInfo);
            }

        }
        // Bの入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.B))
        {
            Vector2 switchInput = SwitchInputManager.Instance.GetAxis(SwitchInputManager.AnalogStick.Left);
            // switchInputの長さを求めて絶対値に変換する
            float switchInputAbs = Mathf.Abs(switchInput.magnitude);

            // UI操作中かどうか
            /*if (UI操作中)
            {
                _broker.Publish(GameEvent.UI.Input.OnBack.GetEvent());
            }*/
            // 回避可能か (2022.07.01 デットゾーン対応)
            if (switchInputAbs > PlayerStatus.ControllerDeadZone.magnitude)
            {
                // スタミナの減少値よりプレイヤーのスタミナがあったらイベント発行
                if (PlayerStatus.playerSp > PlayerMovement._requestDodgeSp)
                {
                    // タイマーが0以下だった場合イベントの発行
                    if (_dodgeCheckTime <= 0.0f)
                    {
                        // 壁に張り付いている
                        if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
                            PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL))
                        {
                            // 壁に張り付いていたら剣をリセットするイベントを飛ばす
                            _broker.Publish(PlayerEvent.OnWallResetSword.GetEvent(null));
                            _broker.Publish(PlayerEvent.Input.OnDodge.GetEvent());
                        }
                        // 張り付いていない
                        else
                        {
                            _broker.Publish(PlayerEvent.Input.OnDodge.GetEvent());
                        }

                        // タイマーを起動する用のフラグをtrueにする
                        _isTimerActive = true;
                    }
                }
            }
            // ジャンプ可能
            else
            {
                _broker.Publish(PlayerEvent.Input.OnJump.GetEvent());
            }
        }
        // X入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.X))
        {
            /*if (抜刀中かどうか 壁に張り付いてるかどうか) return;*/
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.SwordPulling) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL)) return;

            // 抜刀のイベントを発行
            _broker.Publish(PlayerEvent.Input.OnPulling.GetEvent());
            _broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.SwordPulling, true));
        }
        // Y入力
        if (_switchInput.GetKeyDown(SwitchInputManager.JoyConButton.Y))
        {
            /*if (納刀中かどうか 壁に張り付いてるかどうか) return;*/
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.SwordDelivery) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL)) return;

            // 納刀のイベントを発行
            _broker.Publish(PlayerEvent.Input.OnDelivery.GetEvent());
            _broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.SwordDelivery, true));
        }
    }

    /// <summary>
    /// ZRとZRが入力されたときに分岐とイベントを発行する関数
    /// </summary>
    /// <param name="actionInfo"></param>
    private void OnZLZRInput(PlayerActionInfo actionInfo)
    {
        // if (魔法剣を装備しているか)
        {
            // 張り付き状態かどうか
            /*if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL))
            {
                _broker.Publish(PlayerEvent.OnWallResetSword.GetEvent());
                return;
            }*/

            // 投擲モード中かどうか
            if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.ThrowingR) ||
                PlayerStateManager.HasFlag(PlayerStatus.PlayerState.ThrowingL))
            {
                // 投擲入力イベントの発行
                _broker.Publish(PlayerEvent.Input.OnThrowWeapon.GetEvent(actionInfo));
            }
            else if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.SwordPulling))
            {
                _broker.Publish(PlayerEvent.Input.OnThrowWeapon.GetEvent(actionInfo));
            }

            // else if (必要魔力があるかどうか)
            {
                // if (突き刺し状態か)
                {
                    //魔力爆破離脱入力イベント
                }
                // else
                {
                    // 魔力爆破入力イベント発行
                }
            }
        }
    }

    /// <summary>
    /// 回避可能のタイマーの処理を記述する関数
    /// </summary>
    private void Timer()
    {
        // 回避入力を行ったのを関知したらタイマーを増加させる
        if (_isTimerActive)
        {
            _dodgeCheckTime += Time.deltaTime;
        }
        // タイマーが回避の制限時間を超えたら初期化
        if (!(_dodgeCheckTime >= PlayerMovement._dodgeIntervalTime)) return;
        _dodgeCheckTime = 0.0f;
        _isTimerActive = false;
    }
}
