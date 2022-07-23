using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public sealed class LobbyController : SingletonMonoBehaviour<LobbyController>
{
    [SerializeField]
    private LobbyStateManager stateManager;

    [SerializeField]
    private CanvasGroup confirmCanvasGroup;

    [SerializeField, Header("依頼選択の確認画面表示のフェード時間 (秒)")]
    private float confirmViewFadeDuration;

    private readonly MessageBroker _broker = new();

    public IMessageBroker Broker => _broker;

    public LobbyStateManager StateManager => stateManager;

    private void Start()
    {
        _broker.AddTo(this);
        EventReceiver();

        confirmCanvasGroup.alpha = 0f;

        // B押下でのタイトルへ戻る動作
        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.B))
            .Where(_ => stateManager.State != LobbyStateManager.LobbyState.MissionSelected) // 依頼選択中ではない
            .Take(1)
            .Subscribe(_ => OnBPressed())
            .AddTo(this);

        SoundManager.Instance.StopBgm();
    }

    private void EventReceiver()
    {
        _broker.Receive<LobbyEvent.OnMissionSelected>()
               .Subscribe(OnMissionSelected)
               .AddTo(this);
    }

    private void OnBPressed()
    {
        FadeContllor2.Instance.LoadScene(1f, GameScene.Title);
    }

    private async void OnMissionSelected(LobbyEvent.OnMissionSelected data)
    {
        CancellationTokenSource cts = new();
        stateManager.State = LobbyStateManager.LobbyState.MissionSelected;

        await ToggleConfirmView(true, confirmCanvasGroup, confirmViewFadeDuration);

        // 選択依頼確定処理
        UniTask confirmTask = UniTask.Defer(async () =>
        {
            // A押下待機
            await UniTask.WaitUntil(() => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A),
                                    cancellationToken: cts.Token);

            var dataPack = new ToMainGameSceneDataPack(GameScene.Lobby, data.SelectedMission);

            cts.Cancel();
            FadeContllor2.Instance.LoadScene(1f, GameScene.MainGame, dataPack);
        });

        // 選択依頼キャンセル処理
        UniTask cancelTask = UniTask.Defer(async () =>
        {
            // B押下待機
            await UniTask.WaitUntil(() => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.B),
                                    cancellationToken: cts.Token);

            cts.Cancel();

            await ToggleConfirmView(false, confirmCanvasGroup, confirmViewFadeDuration);

            _broker.Publish(LobbyEvent.OnMissionSelectCancelled.GetEvent());
        });

        await UniTask.WhenAny(confirmTask, cancelTask);
    }

    private async UniTask ToggleConfirmView(bool isShow, CanvasGroup target, float fadeTime)
    {
        System.Func<bool> breakCond = isShow
                                          ? () => target.alpha < 1
                                          : () => target.alpha > 0;

        float fadingTime = 0f;

        while (breakCond())
        {
            await UniTask.Yield();

            fadingTime += Time.deltaTime;
            target.alpha = isShow
                               ? fadingTime / fadeTime
                               : 1f - fadingTime / fadeTime;
        }
    }
}
