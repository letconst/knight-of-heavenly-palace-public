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

    [SerializeField, Header("判定の直径")] private float _CapsuleCastRadius = 1;

    /// <summary>実際にSphereCastで使用する半径用の変数</summary>
    private float _setCapsuleCastRadius = 0;

    [SerializeField, Header("オブジェクトとの距離がこの値以下ならエイムアシストを切る")]
    private float _closeRangeDistance = 3;

    public bool isAimAssist;
    [SerializeField] private Transform Player;

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
    [SerializeField] private int _playerLayerMask;
    [SerializeField] private LayerMask _defoultAndPlayerLayerMask;
    private float _ScreenWidth;
    private float _ScreenHeight;

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
        _playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        //レイヤーの反転 プレイヤー以外のレイヤーをマスク
        _playerLayerMask = ~_playerLayerMask;

        _ScreenWidth = Screen.width;
        _ScreenHeight = Screen.height;

        /*レイヤーマスクの複数反転ができない
        _defoultAndPlayerLayerMask =  1 << LayerMask.NameToLayer ("Default Player");
        _defoultAndPlayerLayerMask = ~_defoultAndPlayerLayerMask;
        */
    }

    public void Draw()
    {
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            JoyConToScreenPointer.Instance.AngleReset();
        }

        if (isAimAssist)
        {
            _setCapsuleCastRadius = _CapsuleCastRadius;
            //右Joy-Conのエイムアシストの処理
            var ray = _camera.ScreenPointToRay(JoyConToScreenPointer.Instance.RightJoyConScreenVector2);
            //エイムアシストを入れるか判定
            if (Physics.SphereCast(ray.origin, _setCapsuleCastRadius, ray.direction, out RaycastHit hitR,
                    Mathf.Infinity,
                    _defoultAndPlayerLayerMask))
            {
                if (Vector3.Distance(hitR.point, Player.position) < _closeRangeDistance)
                {
                    _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
                }
                else
                {
                    var v3 = _camera.WorldToScreenPoint(hitR.collider.bounds.center);
                    //v3の座標が画面内ならそのまま表示する。そうでなければ元のカーソルの座標で表示する
                    if (InScreenVector3(v3))
                    {
                        _cursorObjR.position = v3;
                    }
                    else
                    {
                        _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
                    }
                }
            }
            else
            {
                _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
            }

            //左Joy-Conのエイムアシストの処理
            ray = _camera.ScreenPointToRay(JoyConToScreenPointer.Instance.LeftJoyConScreenVector2);
            //エイムアシストを入れるか判定
            if (Physics.SphereCast(ray.origin, _setCapsuleCastRadius, ray.direction, out RaycastHit hitL,
                    Mathf.Infinity,
                    _defoultAndPlayerLayerMask))
            {
                //transform.positionではなくPlayerのtransform.Position
                if (Vector3.Distance(hitL.point, Player.position) < _closeRangeDistance)
                {
                    _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
                }
                else
                {
                    var v3 = _camera.WorldToScreenPoint(hitL.collider.bounds.center);
                    //v3の座標が画面内ならそのまま表示する。そうでなければ元のカーソルの座標で表示する
                    if (InScreenVector3(v3))
                    {
                        _cursorObjL.position = v3;
                    }
                    else
                    {
                        _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
                    }
                }
            }
            else
            {
                _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
            }
        }
        else
        {
            //球の半径を小さくし、擬似的にRayCastを作成
            _setCapsuleCastRadius = 0.001f;
            _cursorObjR.position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
            _cursorObjL.position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
        }

        //カーソルのアニメーション
        DrawSpriteAnimationR(CursorSpriteChange(_drawObjectR, _cursorObjR));
        DrawSpriteAnimationL(CursorSpriteChange(_drawObjectL, _cursorObjL));
    }

    /// <summary>
    /// 引数のVector3が画面内の座標か判定する
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    private bool InScreenVector3(Vector3 v3)
    {
        return 0 <= v3.x && v3.x <= _ScreenWidth &&
               0 <= v3.y && v3.y <= _ScreenHeight;
    }

    /// <summary>
    /// スクリーン上の地点からrayを飛ばし、投擲可能な位置の場合trueが返る関数
    /// オブジェクトが非アクティブの場合nullを返す
    /// </summary>
    /// <param name="cursorGameObject"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    private bool CursorSpriteChange(GameObject cursorGameObject, RectTransform transform)
    {
        //オブジェクトがアクティブなときのみ実行
        if (cursorGameObject.activeSelf)
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(transform.position);

            //オブジェクトにあたったときにtrueが返ってくる
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _playerLayerMask,
                                QueryTriggerInteraction.Ignore))
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
    private async UniTaskVoid CursorLockOnAnimation(CancellationToken token, Image image,
        IReadOnlyList<Sprite> animationSprites)
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
    private async UniTaskVoid CursorUnlockOnAnimation(CancellationToken token, Image image,
        IReadOnlyList<Sprite> animationSprites)
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
        if (isLockOn && cursorStatusL != CursorStatus.LockOn)
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
