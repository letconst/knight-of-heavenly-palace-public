using UnityEngine;
using UnityEngine.UI;

public sealed class MissionBoardView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup boardCanvasGroup;

    /// <summary>依頼掲示板のCanvasGroup</summary>
    public CanvasGroup BoardCanvasGroup => boardCanvasGroup;

    [SerializeField]
    private CanvasGroup confirmCanvasGroup;

    /// <summary>確認表示のCanvasGroup</summary>
    public CanvasGroup ConfirmCanvasGroup => confirmCanvasGroup;

    [SerializeField]
    private Image contentImage;

    /// <summary>確認表示時に表示される依頼内容のImage</summary>
    public Image ContentImage => contentImage;
}
