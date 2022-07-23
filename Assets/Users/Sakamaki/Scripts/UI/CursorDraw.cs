using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Joyconの座標を習得して描画処理を行うクラス
public class CursorDraw : MonoBehaviour
{
    enum CursorStatus
    {
        UnlockOn = 0,
        LockOn = 1
    }

    [SerializeField] private CursorStatus cursorStatusR = CursorStatus.UnlockOn;
    private CursorStatus cursorStatusL = CursorStatus.UnlockOn;
    [SerializeField, Header("描画するオブジェクト")] private GameObject _drawObjectR;
    [SerializeField, Header("描画するオブジェクト")] private GameObject _drawObjectL;

    [SerializeField, Header("描画する右のカーソルのアニメーション連番")]
    private List<Sprite> _drawSpritesR = new List<Sprite>();

    [SerializeField, Header("描画する左のカーソルのアニメーション連番")]
    private List<Sprite> _drawSpritesL = new List<Sprite>();

    [SerializeField, Header("描画する右のImage")]
    private Image _drawImageR;

    [SerializeField, Header("描画する左のImage")]
    private Image _drawImageL;

    /*UniTaskのキャンセルトークンを保持する変数*/
    private CancellationTokenSource _ctsR;
    private CancellationToken _ctR;
    private CancellationTokenSource _ctsL;
    private CancellationToken _ctL;


    [SerializeField] private float animationTime = 0.3f;
    private RectTransform _cursorObjR;
    private RectTransform _cursorObjL;

    //RayCast周りに使用するの定数
    private Camera _camera;
    private int _layerMask;

    private void Start()
    {
        //キャンセルトークンの初期化
        _ctsR = new CancellationTokenSource();
        _ctR = _ctsR.Token;
        _ctsL = new CancellationTokenSource();
        _ctL = _ctsL.Token;

        _cursorObjR = _drawObjectR.GetComponent<RectTransform>();
        _cursorObjL = _drawObjectL.GetComponent<RectTransform>();

        _camera = Camera.main;

        //プレイヤーのレイヤーのみマスク
        _layerMask = 1 << LayerMask.NameToLayer("Player");
        //レイヤーの反転 プレイヤー以外のレイヤーをマスク
        _layerMask = ~_layerMask;
    }

    public void Draw()
    {
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            JoyConToScreenPointer.Instance.AngleReset();
        }

        //カーソルの移動
        _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
        _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;

        //カーソルのアニメーション
        DrawSpriteAnimationR(CursorSpriteChange(_drawObjectR, _cursorObjR, cursorStatusR));
        DrawSpriteAnimationL(CursorSpriteChange(_drawObjectL, _cursorObjL, cursorStatusL));
    }

    /// <summary>
    /// スクリーン上の地点からrayを飛ばし、投擲可能な位置の場合trueが返る関数
    /// オブジェクトが非アクティブの場合nullを返す
    /// </summary>
    /// <param name="cursorGameObject"></param>
    /// <param name="transform"></param>
    /// <param name="cursorStatus"></param>
    /// <returns></returns>
    private bool CursorSpriteChange(GameObject cursorGameObject, RectTransform transform, CursorStatus cursorStatus)
    {
        //オブジェクトがアクティブなときのみ実行
        if (cursorGameObject.activeSelf)
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(transform.position);

            //オブジェクトにあたったときにtrueが返ってくる
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,_layerMask))
            {
                    return true;
            }
        }
        return false;
    }

    #region アニメーション関数

    /// <summary>
    /// 引数のカーソルを0 ~ 1*でアニメーションさせる関数
    /// </summary>
    /// <param name="token">UniTaskのキャンセルトークン</param>
    /// <param name="image"></param>
    /// <param name="animationSprites"></param>
    private async UniTaskVoid CursorLockOnAnimation(CancellationToken token, Image image, IReadOnlyList<Sprite> animationSprites)
    {
        SoundManager.Instance.PlaySe(SoundDef.CursorLocked, .8f);

        //スプライトを初期化
        image.sprite = animationSprites[0];
        var time = 0f;
        var oneFrameTime = animationTime / (animationSprites.Count + 1);
        var i = 0;
        while (i < animationSprites.Count)
        {
            if (oneFrameTime * i < time)
            {
                token.ThrowIfCancellationRequested();
                image.sprite = animationSprites[i];
                i++;
            }
            time += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 引数のカーソルを1* ~ 0でアニメーションさせる関数
    /// </summary>
    /// <param name="token">UniTaskのキャンセルトークン</param>
    /// <param name="image"></param>
    /// <param name="animationSprites"></param>
    private async UniTaskVoid CursorUnlockOnAnimation(CancellationToken token, Image image, IReadOnlyList<Sprite> animationSprites)
    {
        //スプライトを初期化
        image.sprite = animationSprites[animationSprites.Count - 1];
        var time = 0f;
        var oneFrameTime = animationTime / (animationSprites.Count + 1);
        var i = animationSprites.Count - 1;
        while (0 <= i)
        {
            if (oneFrameTime * (animationSprites.Count - i) < time)
            {
                token.ThrowIfCancellationRequested();
                image.sprite = animationSprites[i];
                i--;
            }
            time += Time.deltaTime;
            await UniTask.Yield();
        }
    }
    #endregion


    #region アニメーション発火関数

    /// <summary>
    /// 右のSpriteアニメーションを発火させる関数
    /// </summary>
    /// <param name="isLockOn"></param>
    private void DrawSpriteAnimationR(bool isLockOn)
    {
        if (isLockOn && cursorStatusR != CursorStatus.LockOn)
        {
            _ctsR.Cancel();
            _ctsR = new CancellationTokenSource();
            _ctR = _ctsR.Token;
            cursorStatusR = CursorStatus.LockOn;
            CursorLockOnAnimation(_ctR, _drawImageR, _drawSpritesR).Forget();
        }
        else if (!isLockOn && cursorStatusR != CursorStatus.UnlockOn)
        {
            _ctsR.Cancel();
            _ctsR = new CancellationTokenSource();
            _ctR = _ctsR.Token;
            cursorStatusR = CursorStatus.UnlockOn;
            CursorUnlockOnAnimation(_ctR, _drawImageR, _drawSpritesR).Forget();
        }
    }

    /// <summary>
    /// 左のSpriteアニメーションを発火させる関数
    /// </summary>
    /// <param name="isLockOn"></param>
    private void DrawSpriteAnimationL(bool isLockOn)
    {
        //nullチェック
        if (isLockOn  && cursorStatusL != CursorStatus.LockOn)
        {
            _ctsL.Cancel();
            _ctsL = new CancellationTokenSource();
            _ctL = _ctsL.Token;
            cursorStatusL = CursorStatus.LockOn;
            CursorLockOnAnimation(_ctL, _drawImageL, _drawSpritesL).Forget();
        }
        else if (!isLockOn && cursorStatusL != CursorStatus.UnlockOn)
        {
            _ctsL.Cancel();
            _ctsL = new CancellationTokenSource();
            _ctL = _ctsL.Token;
            cursorStatusL = CursorStatus.UnlockOn;
            CursorUnlockOnAnimation(_ctL, _drawImageL, _drawSpritesL).Forget();
        }
    }

    #endregion


    /// <summary>
    /// カーソル表示を表示(アクティブ)と非表示(非アクティブ)にする関数
    /// </summary>
    /// <param name="isActive"> 表示するかしないか </param>>
    public void ActiveDraw(bool isActive)
    {
        _drawObjectR.SetActive(isActive);
        _drawObjectL.SetActive(isActive);
    }
}