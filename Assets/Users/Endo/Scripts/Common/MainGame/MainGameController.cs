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

        await UniTask.WaitUntil(() => SoundManager.Instance.CanPlayable);

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(MusicDef.First);
    }
}
