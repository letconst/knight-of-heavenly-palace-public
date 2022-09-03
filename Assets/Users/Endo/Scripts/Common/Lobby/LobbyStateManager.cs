using UniRx;
using UnityEngine;

public sealed class LobbyStateManager : MonoBehaviour
{
    [System.Flags]
    public enum LobbyState
    {
        Normal             = 1 << 0,
        MissionBoardOpened = 1 << 1,
        MissionSelected    = 1 << 2,
    }

    public enum StateChangeType
    {
        Add,
        Remove,
    }

    public LobbyState State { get; private set; } = LobbyState.Normal;

    private void Start()
    {
        EventReceiver();
    }

    private void EventReceiver()
    {
        IMessageReceiver broker = LobbyController.Instance.Broker;

        broker.Receive<LobbyEvent.OnMissionSelected>()
              .Subscribe(_ => AddState(LobbyState.MissionSelected))
              .AddTo(this);

        broker.Receive<LobbyEvent.OnMissionSelectCancelled>()
              .Subscribe(_ => RemoveState(LobbyState.MissionSelected))
              .AddTo(this);
    }

    /// <summary>
    /// ステートを追加する
    /// </summary>
    /// <param name="state"></param>
    public void AddState(LobbyState state)
    {
        // 指定のステートがまだ追加されていなければ、ステート変更イベント発行
        if (!HasState(state))
        {
            LobbyController.Instance.Broker.Publish(LobbyEvent.OnStateChanged.GetEvent(state, StateChangeType.Add));
        }

        State |= state;
    }

    /// <summary>
    /// ステートを削除する
    /// </summary>
    /// <param name="state"></param>
    public void RemoveState(LobbyState state)
    {
        // 指定のステートがあれば、ステート変更イベント発行
        if (HasState(state))
        {
            LobbyController.Instance.Broker.Publish(LobbyEvent.OnStateChanged.GetEvent(state, StateChangeType.Remove));
        }

        State &= ~state;
    }

    /// <summary>
    /// 指定のステートが追加されているかを取得する
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool HasState(LobbyState state)
    {
        return State.HasFlag(state);
    }
}
