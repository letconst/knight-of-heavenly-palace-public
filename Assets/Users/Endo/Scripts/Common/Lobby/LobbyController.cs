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
    private MissionBoardView missionBoardView;

    [SerializeField, Header("依頼選択の確認画面表示のフェード時間 (秒)")]
    private float confirmViewFadeDuration;

    private readonly MessageBroker _broker = new();

    public IMessageBroker Broker => _broker;

    public LobbyStateManager StateManager => stateManager;

    public MissionBoardView MissionBoardView => missionBoardView;

    private void Start()
    {
        _broker.AddTo(this);
        EventReceiver();

        missionBoardView.BoardCanvasGroup.alpha          = 0f;
        missionBoardView.BoardCanvasGroup.interactable   = false;
        missionBoardView.ConfirmCanvasGroup.alpha        = 0f;
        missionBoardView.ConfirmCanvasGroup.interactable = false;

        // B押下でのタイトルへ戻る動作
        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.B))
            .Where(_ => stateManager.HasState(LobbyStateManager.LobbyState.MissionBoardOpened)) // 掲示板表示中
            .Where(_ => !stateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))   // 依頼選択中ではない
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

    /// <summary>
    /// Bボタン押下時の処理
    /// </summary>
    private void OnBPressed()
    {
        stateManager.RemoveState(LobbyStateManager.LobbyState.MissionBoardOpened);
        stateManager.AddState(LobbyStateManager.LobbyState.Normal);
        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnStateChangeRequest.GetEvent(
                PlayerStatus.PlayerState.UIHandling, PlayerStateChangeOptions.Delete, null, null));

        missionBoardView.BoardCanvasGroup.interactable = false;
        missionBoardView.BoardCanvasGroup.ToggleFade(false, .5f).Forget();
    }

    /// <summary>
    /// 依頼が選択されたときの処理
    /// </summary>
    /// <param name="data"></param>
    private async void OnMissionSelected(LobbyEvent.OnMissionSelected data)
    {
        CancellationTokenSource cts = new();

        // 表示内容設定
        missionBoardView.ContentImage.sprite = data.SelectedMission.ContentImage;

        await missionBoardView.ConfirmCanvasGroup.ToggleFade(true, confirmViewFadeDuration);

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

            await missionBoardView.ConfirmCanvasGroup.ToggleFade(false, confirmViewFadeDuration);

            _broker.Publish(LobbyEvent.OnMissionSelectCancelled.GetEvent());
        });

        await UniTask.WhenAny(confirmTask, cancelTask);
    }
}
