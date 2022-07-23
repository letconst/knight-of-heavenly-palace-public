using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

// プレイヤーが武器を投擲したときの処理クラス
public partial class WeaponThrowing : MonoBehaviour
{
    [SerializeField] private GameObject _joyR;
    [SerializeField] private GameObject _joyL;
    [SerializeField] private GameObject _playerObject; //仮のプレイヤー座標習得 プロパティとか使って習得するとよさそう
    [SerializeField] private Transform _swordParentR;
    [SerializeField] private Transform _swordParentL;
    [SerializeField] private GameObject _swordObjR;
    [SerializeField] private GameObject _swordObjL;
    [SerializeField] private Transform _swordTipR;
    [SerializeField] private Transform _swordTipL;

    [SerializeField, Header("飛ばした後の剣の親オブジェクト")]
    private GameObject _flySwordParent;

    [SerializeField] private float _swordSpeed;

    [SerializeField, Tooltip("どれぐらいの角度でぶら下がりをおこなうか")]
    private float _wallAngle;

    // 剣が投げられているかどうかの変数
    private bool _isThrowR = false;
    private bool _isThrowL = false;
    private Camera _mainCamera;

    // 例外処理を行うplayerLayerを保持する変数
    private LayerMask _playerLayerMask;

    // イベント発行用のbroker
    private IMessageBroker _broker;
    private Rigidbody _playerRb;

    private void Start()
    {
        _mainCamera = Camera.main;
        _playerLayerMask = LayerMask.GetMask("Player");
        _playerRb = _playerObject.GetComponent<Rigidbody>();

        // 初期化で各剣をプレイヤーの子にする
        SwordPositionReset(PlayerInputEvent.PlayerHand.Right);
        SwordPositionReset(PlayerInputEvent.PlayerHand.Left);
        
        _broker = PlayerInputEventEmitter.Instance.Broker;
    }

    /// <summary>
    /// 実際に武器を投げる処理
    /// </summary>
    public void Throwing(PlayerActionInfo actionInfo)
    {
        // 2022.07.08 条件式の変更
        // 投擲を行う (投てきしている最中だったらスルーする)
        if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Right && !_isThrowR)
        {
            /*var playerActionInfo = new PlayerActionInfo
            {
                // どちらの手で投げられたかを変数に代入
                actHand = PlayerInputEvent.PlayerHand.Right
            };*/

            JoyToRay(_joyR.transform.position, _swordObjR.transform, _swordTipR, actionInfo);
        }
        else if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Left && !_isThrowL)
        {
            /*var playerActionInfo = new PlayerActionInfo
            {
                // どちらの手で投げられたかを変数に代入
                actHand = PlayerInputEvent.PlayerHand.Left
            };*/

            JoyToRay(_joyL.transform.position, _swordObjL.transform, _swordTipL, actionInfo);
        }
    }

    /// <summary>
    /// Joyconの座標を習得してボタンをおすことでその座標にRayを飛ばす処理
    /// </summary>
    /// <param name="joyVec"> 座標を習得する変数 </param>>
    private async void JoyToRay(Vector3 joyVec, Transform swordObj, Transform swordTip, PlayerActionInfo actionInfo)
    {
        Ray ray;
        ray = _mainCamera.ScreenPointToRay(joyVec);

        // Rayを実際に飛ばす (ray, hitしたときの情報, 無限に飛ばす)
        // 何かしらにあたった場合 true 当たらなかった場合 false
        // なにか下に当たった時 かつ 投擲中じゃない時 (Playerを例外)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_playerLayerMask))
        {
            // ray y軸の値を *180して、値が - になってしまうので +180をおこない値を補正
            // hit.normal 1 ~ -1;
            float angleY = (hit.normal.y * 180) + 180;

            switch (actionInfo.actHand)
            {
                // 投げたのを確認したのでステートとフラグをつける
                case PlayerInputEvent.PlayerHand.Right:
                    _isThrowR = true;

                    // 武器投擲時にステート変更のイベントを発行
                    _broker.Publish(PlayerEvent.OnStateChangeRequest
                        .GetEvent(PlayerStatus.PlayerState.ThrowingR, true));
                    break;
                case PlayerInputEvent.PlayerHand.Left:
                    _isThrowL = true;

                    _broker.Publish(PlayerEvent.OnStateChangeRequest
                        .GetEvent(PlayerStatus.PlayerState.ThrowingL, true));
                    break;
            }

            // 剣を飛ばしたらプレイヤーの座標に追従しない様にparentを移す
            swordObj.parent = _flySwordParent.transform;
            // 向きに合わせて回転処理を行う
            swordObj.rotation = Quaternion.FromToRotation(swordObj.up, ray.direction)
                                * swordObj.rotation;

            Vector3 playerVec;
            Vector3 lerpVec;
            Vector3 rayHitPoint;
            playerVec = swordObj.position;
            rayHitPoint = hit.point;
            float calcSpeed = 0;

            swordObj.position = playerVec;
            // 剣の中心から剣先までの差分
            Vector3 swordCenterToTipDelta = swordObj.position - swordTip.position;

            // 再生するエフェクト選択
            EffectType trailEffectType = actionInfo.actHand switch
            {
                PlayerInputEvent.PlayerHand.Left => EffectType.SwordTrailL,
                PlayerInputEvent.PlayerHand.Right => EffectType.SwordTrailR
            };

            // 総距離
            float totalThrowingTime = Vector3.Distance(playerVec, rayHitPoint) / _swordSpeed;

            // トレイルエフェクト再生
            // 引数 : EffectType , LocalVector3 , Rotation , ParentTransform , LoopFlag , LoopTime
            EffectManager.Instance.EffectPlay(trailEffectType, Vector3.zero, Quaternion.identity,
                swordObj, true, totalThrowingTime);

            // ループを行う
            while (true)
            {
                calcSpeed += Time.deltaTime / totalThrowingTime;

                lerpVec = Vector3.Lerp(playerVec, rayHitPoint + swordCenterToTipDelta, calcSpeed);

                // 剣を飛ばしてLerpで求めた値を剣のオブジェクトに加算
                swordObj.position = lerpVec;

                if (calcSpeed >= 1)
                {
                    // 剣があたった先のy軸が一定以下
                    if (angleY < _wallAngle)
                    {
                        // ぶら下がり状態処理の関数
                        WeaponHanging(rayHitPoint, actionInfo);
                    }
                    else
                    {
                        // hitしたオブジェクトの座標をプレイヤー座標に代入
                        _playerObject.transform.position = rayHitPoint;

                        // 角度が一定以下ではなかった場合すぐ投げれるようにフラグを下ろす
                        if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Right)
                        {
                            _isThrowR = false;
                        }
                        else if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Left)
                        {
                            _isThrowL = false;
                        }
                        
                        // ポジションのリセット
                        SwordPositionReset(actionInfo.actHand);
                    }

                    switch (actionInfo.actHand)
                    {
                        // 着地処理を行って代入したのでfalseにする
                        case PlayerInputEvent.PlayerHand.Right:
                            // それにあわせてフラグを落とすためにイベントの発行
                            _broker.Publish(PlayerEvent.OnStateChangeRequest
                                .GetEvent(PlayerStatus.PlayerState.ThrowingR, false));
                            break;
                        case PlayerInputEvent.PlayerHand.Left:
                            // それにあわせてフラグを落とすためにイベントの発行
                            _broker.Publish(PlayerEvent.OnStateChangeRequest
                                .GetEvent(PlayerStatus.PlayerState.ThrowingL, false));
                            break;
                    }

                    break;
                }

                // 次のアップデートが呼ばれるタイミングを非同期でまつ
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }

    /// <summary>
    /// 見た目上の剣の場所を戻す関数
    /// </summary>
    private void SwordPositionReset(PlayerInputEvent.PlayerHand actionInfo)
    {
        if (actionInfo == PlayerInputEvent.PlayerHand.Right)
        {
            _swordObjR.transform.parent = _swordParentR.transform;
            _swordObjR.transform.position = _swordParentR.position;
            _swordObjR.transform.rotation = _swordParentR.rotation;
        }
        else if (actionInfo == PlayerInputEvent.PlayerHand.Left)
        {
            _swordObjL.transform.parent = _swordParentL.transform;
            _swordObjL.transform.position = _swordParentL.position;
            _swordObjL.transform.rotation = _swordParentL.rotation;
        }
    }
}