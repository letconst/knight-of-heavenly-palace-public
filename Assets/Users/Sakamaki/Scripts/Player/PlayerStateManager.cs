using UniRx;
using UnityEngine;

/// <summary>
/// イベントの形に対応したStateManagerクラス
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    private IMessageBroker _inputBroker;

    // Start is called before the first frame update
    void Start()
    {
        // 抜刀にしておく
        StateChange(PlayerStatus.PlayerState.SwordPulling);

        _inputBroker = PlayerInputEventEmitter.Instance.Broker;

        StateChangeEmitter();
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
                if (x.IsAdd)
                {
                    // 各種の例外処理を書く
                    switch (x.State)
                    {
                        case PlayerStatus.PlayerState.None:
                            break;
                        // 移動関係処理
                        case PlayerStatus.PlayerState.Standing:
                            if (HasFlag(PlayerStatus.PlayerState.HangingR)) return;
                            // 止まっているということは移動していないので移動のステートを消す
                            RemoveFlag(PlayerStatus.PlayerState.Move);

                            PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.Move:
                            // ぶら下がり状態
                            if (HasFlag(PlayerStatus.PlayerState.HangingR)) return;
                            // 逆に移動しているということは静止していないので停止のステートを消す
                            RemoveFlag(PlayerStatus.PlayerState.Standing);

                            PlayerMotionController.Instance.SetFloat("IdleAndRun", 1f);
                            break;
                        case PlayerStatus.PlayerState.Jump:
                            // ぶら下がり状態
                            if (HasFlag(PlayerStatus.PlayerState.HangingR)) return;
                            break;
                        case PlayerStatus.PlayerState.Dodge:
                            // ぶら下がり状態じゃなければ地上回避モーション再生
                            if (!HasFlag(PlayerStatus.PlayerState.HangingL) && !HasFlag(PlayerStatus.PlayerState.HangingR))
                            {
                                PlayerMotionController.Instance.PlayTriggerMotion(PlayerMotionController.MotionType.DodgeInGround);
                            }
                            break;
                        case PlayerStatus.PlayerState.HangingR:
                            RemoveFlag(PlayerStatus.PlayerState.Move);
                            PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.HangingL:
                            RemoveFlag(PlayerStatus.PlayerState.Move);
                            PlayerMotionController.Instance.SetFloat("IdleAndRun", 0f);
                            break;
                        case PlayerStatus.PlayerState.Falling:
                            break;
                        // 共通アクション
                        case PlayerStatus.PlayerState.SwordPulling:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling)) return;
                            RemoveFlag(PlayerStatus.PlayerState.SwordDelivery);
                            SoundManager.Instance.PlaySe(SoundDef.SwitchSwordMode);
                            break;
                        case PlayerStatus.PlayerState.SwordDelivery:
                            if (HasFlag(PlayerStatus.PlayerState.SwordDelivery)) return;
                            RemoveFlag(PlayerStatus.PlayerState.SwordPulling);
                            SoundManager.Instance.PlaySe(SoundDef.SwitchSwordMode);
                            break;
                        // 攻撃系統 (右手)
                        case PlayerStatus.PlayerState.AttackR:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling)) return;
                            break;
                        case PlayerStatus.PlayerState.ThrowingR:
                            break;
                        // 攻撃系統 (左手)
                        case PlayerStatus.PlayerState.AttackL:
                            if (HasFlag(PlayerStatus.PlayerState.SwordPulling)) return;
                            break;
                        case PlayerStatus.PlayerState.ThrowingL:
                            break;
                    }

                    // ステートの追加 (変更を行う)
                    StateChange(x.State);
                }
                // falseで飛んできた場合remove
                else if (!x.IsAdd)
                {
                    RemoveFlag(x.State);
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
