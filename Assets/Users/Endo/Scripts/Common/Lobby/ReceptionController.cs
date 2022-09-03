using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public sealed class ReceptionController : MonoBehaviour
{
    private bool _isPlayerInRange;

    private LobbyStateManager _stateManager;

    private void Start()
    {
        _stateManager = LobbyController.Instance.StateManager;

        this.OnTriggerEnterAsObservable()
            .Where(other => 1 << other.gameObject.layer == LayerConstants.Player)
            .Subscribe(OnEnter)
            .AddTo(this);

        this.OnTriggerExitAsObservable()
            .Where(other => 1 << other.gameObject.layer == LayerConstants.Player)
            .Subscribe(OnExit)
            .AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
            .Where(_ => _isPlayerInRange)
            .Where(_ => _stateManager.HasState(LobbyStateManager.LobbyState.Normal))
            .Subscribe(_ => OnAPressed())
            .AddTo(this);
    }

    private void OnEnter(Collider other)
    {
        _isPlayerInRange = true;
    }

    private void OnExit(Collider other)
    {
        _isPlayerInRange = false;
    }

    private void OnAPressed()
    {
        _stateManager.RemoveState(LobbyStateManager.LobbyState.Normal);
        _stateManager.AddState(LobbyStateManager.LobbyState.MissionBoardOpened);
        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnStateChangeRequest.GetEvent(
                PlayerStatus.PlayerState.UIHandling, PlayerStateChangeOptions.Add, null, null));

        LobbyController.Instance.MissionBoardView.BoardCanvasGroup.interactable = true;
        LobbyController.Instance.MissionBoardView.BoardCanvasGroup.ToggleFade(true, .5f).Forget();
    }
}
