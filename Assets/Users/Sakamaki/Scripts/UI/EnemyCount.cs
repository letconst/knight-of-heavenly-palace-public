using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーの討伐数を表示するクラス
/// </summary>
public class EnemyCount : MonoBehaviour
{
    [SerializeField, Tooltip("表示する画像")]
    private Sprite[] _countNumImages = new Sprite[11];
    [SerializeField, Tooltip("表示する分母画像オブジェクト")]
    private Image _denominatorCountImage;
    [SerializeField, Tooltip("表示する画像オブジェクト")]
    private Image _moleculeCountImage;
    [SerializeField, Tooltip("10を調整する用の変数")] private float _numResize;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;

    private Vector2 _denominatorInitSize;
    private Vector2 _moleculeInitSize;

    void Start()
    {
        _denominatorInitSize = _denominatorCountImage.rectTransform.sizeDelta;
        _moleculeInitSize = _moleculeCountImage.rectTransform.sizeDelta;

        // 分母か分子のサイズが最大値だった場合
        if (MainGameMissionManager.Instance._RequireCount == 10)
        {
            //サイズの変更
            _denominatorCountImage.rectTransform.sizeDelta += new Vector2(_numResize, 0);
        }
        else if (MainGameMissionManager.Instance._CurrentCount == 10)
        {
            _moleculeCountImage.rectTransform.sizeDelta += new Vector2(_numResize, 0);
        }
        else
        {
            _denominatorCountImage.rectTransform.sizeDelta = _denominatorInitSize;
            _moleculeCountImage.rectTransform.sizeDelta = _moleculeInitSize;
        }

        // 初期化処理
        _denominatorCountImage.sprite = _countNumImages[MainGameMissionManager.Instance._RequireCount];
        _moleculeCountImage.sprite = _countNumImages[MainGameMissionManager.Instance._CurrentCount];

        // 値を監視して値が変更された時、UIの変更を行う
        this.ObserveEveryValueChanged(_ => MainGameMissionManager.Instance._CurrentCount)
            .Skip(1) // 1回だけ無視
            .DistinctUntilChanged() // 同じ値だったら無視
            .Subscribe(count =>
            {
                if (MainGameMissionManager.Instance._CurrentCount == 10)
                {
                    _moleculeCountImage.rectTransform.sizeDelta += new Vector2(_numResize, 0);
                }
                else
                {
                    _moleculeCountImage.rectTransform.sizeDelta = _moleculeInitSize;
                }

                // 値の変更
                _moleculeCountImage.sprite = _countNumImages[count];
            })
            .AddTo(this);
    }
}
