using UniRx;
using UnityEngine;

public sealed class MainGameController : SingletonMonoBehaviour<MainGameController>
{
    [SerializeField]
    private MissionTimer missionTimer;

    private readonly MessageBroker _broker = new();

    public IMessageBroker Broker => _broker;

    protected override void Awake()
    {
        base.Awake();
        InstantiateMissionPrefab();
    }

    private async void Start()
    {
        _broker.AddTo(this);
        
        missionTimer.IsTimerActive = true;

        // ロビーのUI操作ステートが残るため削除
        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.UIHandling, PlayerStateChangeOptions.Delete, null,
                                                      null));

        await SoundManager.Instance.WaitForInitialize();

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(MusicDef.First);
    }

    /// <summary>
    /// 依頼に応じたプレハブを生成する
    /// </summary>
    private void InstantiateMissionPrefab()
    {
        var dataPack = (ToMainGameSceneDataPack) FadeContllor2.PreviousSceneData;

        if (dataPack == null)
            return;

        if (!dataPack.MissionData.InitGeneratePrefab)
            return;

        Instantiate(dataPack.MissionData.InitGeneratePrefab);
    }
}
