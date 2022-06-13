using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DebugInputController : MonoBehaviour
{
    [SerializeField]
    private CinemachineCollider _cinemachineCollider;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Plus))
            .Subscribe(_ =>
            {
                _cinemachineCollider.enabled = !_cinemachineCollider.enabled;
            })
            .AddTo(this);
    }
}
