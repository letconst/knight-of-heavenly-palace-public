using UniRx;
using UnityEngine;

public sealed class LobbyStateManager : MonoBehaviour
{
    public enum LobbyState
    {
        None,
        MissionSelected,
    }

    public LobbyState State { get; set; }

    private void Start()
    {
        // ステートが変更されたらイベント発行
        this.ObserveEveryValueChanged(x => x.State)
            .Skip(1)
            .Subscribe(s => LobbyController.Instance.Broker.Publish(LobbyEvent.OnStateChanged.GetEvent(s)))
            .AddTo(this);

        EventReceiver();
    }

    private void EventReceiver()
    {
        IMessageReceiver broker = LobbyController.Instance.Broker;

        broker.Receive<LobbyEvent.OnMissionSelected>()
              .Subscribe(_ => State = LobbyState.MissionSelected)
              .AddTo(this);

        broker.Receive<LobbyEvent.OnMissionSelectCancelled>()
              .Subscribe(_ => State = LobbyState.None)
              .AddTo(this);
    }
}
