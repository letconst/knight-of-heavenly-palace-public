using UniRx;
using UnityEngine;

/// <summary>
/// イベントの形に対応したStateManagerクラス
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    private IMessageBroker _inputBroker;

    private Rigidbody rb;

    private void Awake()
    {
        PlayerStatus.playerState = PlayerStatus.PlayerState.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 抜刀と投擲モードにしておく
        StateChange(PlayerStatus.PlayerState.SwordPulling);
        StateChange(PlayerStatus.PlayerState.ThrowingMode);

        _inputBroker = PlayerInputEventEmitter.Instance.Broker;

        rb = GetComponent<Rigidbody>();
        StateChangeEmitter();
    }

    private void Update()
    {
        //MoveAnimation
        //なにかいい処理があったら書き換えてください
        PlayerAnimationManager.Instance.IdleAndRun(rb.velocity.magnitude);
    }

    /// <summary>
    /// 移動先のステートを習得してきて、判別を行った上で変更を行う
    /// </summary>
    private void StateChangeEmitter()
    {
        _inputBroker.Receive<PlayerEvent.OnStateChangeRequest>()
            .Subscribe(x =>
            {
                // trueで飛んできて追加するだった場合
                if (x.StateChangeOptions == PlayerStateChangeOptions.Add)
                {
                    // 各種の例外処理を書く
                    switch (x.State)
                    {
                        case PlayerStatus.PlayerState.None:
                            break;
                        // 移動関係処理
                        case PlayerStatus.PlayerState.Standing:
                            if (HasFlag(PlayerStatus.PlayerState.HangingR))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            // 止まっているということは移動していないので移動のステートを消す
                            RemoveFlag(PlayerStatus.PlayerState.Move);

                            //PlayerAnimationManager.Instance.IdleAndRun(rb.velocity.magnitude);
                            //PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.Move:
                            // ぶら下がり状態
                            if (HasFlag(PlayerStatus.PlayerState.HangingR))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            // 逆に移動しているということは静止していないので停止のステートを消す
                            RemoveFlag(PlayerStatus.PlayerState.Standing);

                            //PlayerAnimationManager.Instance.IdleAndRun(rb.velocity.magnitude);
                            //PlayerMotionController.Instance.SetFloat("IdleAndRun", 1f);
                            break;
                        case PlayerStatus.PlayerState.Jump:
                            // ぶら下がり状態
                            if (HasFlag(PlayerStatus.PlayerState.HangingR))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            break;
                        case PlayerStatus.PlayerState.Dodge:
                            // ぶら下がり状態じゃなければ地上回避モーション再生
                            if (!HasFlag(PlayerStatus.PlayerState.HangingL) && !HasFlag(PlayerStatus.PlayerState.HangingR))
                            {
                                PlayerAnimationManager.Instance.DodgeInGround();
                                //PlayerMotionController.Instance.PlayTriggerMotion(PlayerMotionController.MotionType.DodgeInGround);
                            }
                            break;
                        case PlayerStatus.PlayerState.HangingR:
                            RemoveFlag(PlayerStatus.PlayerState.Move);

                            //PlayerAnimationManager.Instance.IdleAndRun(rb.velocity.magnitude);
                            //PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.HangingL:
                            RemoveFlag(PlayerStatus.PlayerState.Move);

                            //PlayerAnimationManager.Instance.IdleAndRun(rb.velocity.magnitude);
                            //PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.Falling:
                            break;
                        // 共通アクション
                        case PlayerStatus.PlayerState.SwordPulling:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            RemoveFlag(PlayerStatus.PlayerState.SwordDelivery);
                            SoundManager.Instance.PlaySe(SoundDef.SwitchSwordMode);
                            break;
                        case PlayerStatus.PlayerState.SwordDelivery:
                            if (HasFlag(PlayerStatus.PlayerState.SwordDelivery))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            RemoveFlag(PlayerStatus.PlayerState.SwordPulling);
                            SoundManager.Instance.PlaySe(SoundDef.SwitchSwordMode);
                            break;
                        // 攻撃系統 (右手)
                        case PlayerStatus.PlayerState.AttackR:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            break;
                        case PlayerStatus.PlayerState.ThrowingR:
                            break;
                        // 攻撃系統 (左手)
                        case PlayerStatus.PlayerState.AttackL:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            break;
                        case PlayerStatus.PlayerState.ThrowingL:
                            break;
                        case PlayerStatus.PlayerState.ThrowingMode:
                            if (HasFlag(PlayerStatus.PlayerState.ThrowingMode))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            // 投擲モードになるということは攻撃モードではないのでフラグを削除
                            RemoveFlag(PlayerStatus.PlayerState.AttackMode);
                            break;
                        case PlayerStatus.PlayerState.AttackMode:
                            if (HasFlag(PlayerStatus.PlayerState.AttackMode))
                            {
                                x.OnChanged?.Invoke();
                                return;
                            }
                            // 攻撃モードになるということは投擲モードではないのでフラグを削除
                            RemoveFlag(PlayerStatus.PlayerState.ThrowingMode);
                            break;
                        default:
                            break;
                    }
                    // ステートの追加 (変更を行う)
                    StateChange(x.State);

                    // ステートの変更を行った後変更をした後のイベントを発行する
                    _inputBroker.Publish(PlayerEvent.OnStateChanged.GetEvent(x.State));

                    // OnChanged実行できることを通知
                    // (nullで宣言されている場合エラー担ってしまうのでnull許容型に変更)
                    x.OnChanged?.Invoke();
                }
                // falseで飛んできた場合remove
                else if (x.StateChangeOptions == PlayerStateChangeOptions.Delete)
                {
                    RemoveFlag(x.State);

                    // OnChanged実行できることを通知
                    // (nullで宣言されている場合エラー担ってしまうのでnull許容型に変更)
                    x.OnRejected?.Invoke();
                }
            }).AddTo(this);

        _inputBroker.Receive<PlayerEvent.OnLanding>()
                    .Subscribe(_ =>
                    {
                        RemoveFlag(PlayerStatus.PlayerState.Falling);
                    })
                    .AddTo(this);
    }

    /// <summary>
    /// 攻撃ステートを右左判別して変更を行う関数
    /// </summary>
    /// <param name="actionInfo"> 右手か左手か </param>
    /// <param name="state"> 変更を行う先のステート名 </param>
    /*private void StateChange(PlayerActionInfo actionInfo, PlayerStatus.PlayerState state)
    {
        if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Right)
        {
            AddFlag(state);
        }
        else if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Left)
        {
            AddFlag(state);
        }
    }*/

    /// <summary>
    /// ステートの変更を行う関数
    /// </summary>
    /// <param name="state"> 変更を行うステート先 </param>
    private void StateChange(PlayerStatus.PlayerState state)
    {
        AddFlag(state);
    }

    /// <summary>
    /// ステートの変数に指定のステートが存在しているかを判別するbool関数
    /// </summary>
    /// <param name="state"> 検知したいステート </param>
    /// <returns> 指定したステートが存在している </returns>
    public static bool HasFlag(PlayerStatus.PlayerState state)
    {
        return PlayerStatus.playerState.HasFlag(state);
    }

    /// <summary>
    /// ステートの変数に指定のステートを追加する関数
    /// </summary>
    /// <param name="state"></param>
    private void AddFlag(PlayerStatus.PlayerState state)
    {
        PlayerStatus.playerState |= state;
    }

    /// <summary>
    /// ステートの変数に指定のステートを削除する関数
    /// </summary>
    /// <param name="state"></param>
    private void RemoveFlag(PlayerStatus.PlayerState state)
    {
        PlayerStatus.playerState &= ~state;
    }
}
