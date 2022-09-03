using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public sealed class MainGameController : SingletonMonoBehaviour<MainGameController>
{
    [SerializeField]
    private MissionTimer missionTimer;

    private readonly MessageBroker _broker = new();

    public IMessageBroker Broker => _broker;

    private async void Start()
    {
        _broker.AddTo(this);

        missionTimer.IsTimerActive = true;

        // ロビーのUI操作ステートが残るため削除
        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnStateChangeRequest.GetEvent(
                PlayerStatus.PlayerState.UIHandling, PlayerStateChangeOptions.Delete, null, null));

        await SoundManager.Instance.WaitForInitialize();

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(MusicDef.First);
    }
}
