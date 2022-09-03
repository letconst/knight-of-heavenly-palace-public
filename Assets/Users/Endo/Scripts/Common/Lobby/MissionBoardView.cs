using UnityEngine;

public sealed class MissionBoardView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup boardCanvasGroup;

    public CanvasGroup BoardCanvasGroup => boardCanvasGroup;

    [SerializeField]
    private CanvasGroup confirmCanvasGroup;

    public CanvasGroup ConfirmCanvasGroup => confirmCanvasGroup;
}
