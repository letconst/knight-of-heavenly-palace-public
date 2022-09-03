using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

// プレイヤーが武器を投擲したときの処理クラス
public partial class WeaponThrowing : SingletonMonoBehaviour<WeaponThrowing>
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

    private WeaponThrowingData _leftWeapon;
    private WeaponThrowingData _rightWeapon;

    private Camera _mainCamera;

    // イベント発行用のbroker
    private IMessageBroker _broker;
    private Rigidbody _playerRb;

    private void Start()
    {
        _mainCamera = Camera.main;
        _playerRb = _playerObject.GetComponent<Rigidbody>();

        // 初期化で各剣をプレイヤーの子にする
        SwordPositionReset(PlayerInputEvent.PlayerHand.Right);
        SwordPositionReset(PlayerInputEvent.PlayerHand.Left);

        _broker = PlayerInputEventEmitter.Instance.Broker;

        _broker.Receive<PlayerEvent.OnLandingSword>()
               .Subscribe(OnLandingSword)
               .AddTo(this);
    }

    private async void OnLandingSword(PlayerEvent.OnLandingSword data)
    {
        PlayerStatus.PlayerState targetState = data.ActionInfo.actHand switch
        {
            PlayerInputEvent.PlayerHand.Left  => PlayerStatus.PlayerState.TransferringL,
            PlayerInputEvent.PlayerHand.Right => PlayerStatus.PlayerState.TransferringR
        };

        // 転移状態に設定
        _broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(targetState, PlayerStateChangeOptions.Add, null, null));

        EffectType targetEffectIn = data.ActionInfo.actHand switch
        {
            PlayerInputEvent.PlayerHand.Left  => EffectType.SwordTransfer_In_Left,
            PlayerInputEvent.PlayerHand.Right => EffectType.SwordTransfer_In_Right
        };

        EffectType targetEffectOut = data.ActionInfo.actHand switch
        {
            PlayerInputEvent.PlayerHand.Left  => EffectType.SwordTransfer_Out_Left,
            PlayerInputEvent.PlayerHand.Right => EffectType.SwordTransfer_Out_Right
        };

        EffectManager.Instance.EffectPlay(targetEffectIn, _playerObject.transform.position, Quaternion.identity, async () =>
        {
            EffectManager.Instance.EffectPlay(targetEffectOut, data.LandingPosition, Quaternion.identity);

            if (data.LandingAngle < _wallAngle)
            {
                WeaponHanging(data.LandingPosition, data.ActionInfo);
            }
            else
            {
                // hitしたオブジェクトの座標をプレイヤー座標に代入
                _playerObject.transform.position = data.LandingPosition;

                // ポジションのリセット
                SwordPositionReset(data.ActionInfo.actHand);
            }

            // 転移終了エフェクトを少し待ってからプレイヤーアクティブ化
            // TODO: delayは避けたい
            await UniTask.Delay(500);

            _playerObject.SetActive(true);
            _broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(targetState, PlayerStateChangeOptions.Delete, null, null));
        });

        switch (data.ActionInfo.actHand)
        {
            // 着地処理を行って代入したのでfalseにする
            case PlayerInputEvent.PlayerHand.Right:
                // それにあわせてフラグを落とすためにイベントの発行
                _broker.Publish(PlayerEvent.OnStateChangeRequest
                                           .GetEvent(PlayerStatus.PlayerState.ThrowingR, PlayerStateChangeOptions.Delete,
                                                     null, null));
                break;
            case PlayerInputEvent.PlayerHand.Left:
                // それにあわせてフラグを落とすためにイベントの発行
                _broker.Publish(PlayerEvent.OnStateChangeRequest
                                           .GetEvent(PlayerStatus.PlayerState.ThrowingL, PlayerStateChangeOptions.Delete,
                                                     null, null));
                break;
        }

        // 転移開始エフェクトを少し待ってからプレイヤー非アクティブ化
        // TODO: delayは避けたい
        await UniTask.Delay(500);

        _playerObject.SetActive(false);
    }

    /// <summary>
    /// 判定対象とする左手の武器を設定する
    /// </summary>
    /// <param name="weapon"></param>
    public void SetLeftTargetWeapon(WeaponThrowingData weapon)
    {
        _leftWeapon = weapon;
    }

    /// <summary>
    /// 判定対象とする右手の武器を設定する
    /// </summary>
    /// <param name="weapon"></param>
    public void SetRightTargetWeapon(WeaponThrowingData weapon)
    {
        _rightWeapon = weapon;
    }

    /// <summary>
    /// 実際に武器を投げる処理
    /// </summary>
    public void Throwing(PlayerActionInfo actionInfo)
    {
        // 2022.07.08 条件式の変更
        // 投擲を行う (投てきしている最中だったらスルーする)
        if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Right && !_rightWeapon.IsThrowing)
        {
            /*var playerActionInfo = new PlayerActionInfo
            {
                // どちらの手で投げられたかを変数に代入
                actHand = PlayerInputEvent.PlayerHand.Right
            };*/

            JoyToRay(_joyR.transform.position, _swordObjR.transform, actionInfo);
        }
        else if (actionInfo.actHand == PlayerInputEvent.PlayerHand.Left && !_leftWeapon.IsThrowing)
        {
            /*var playerActionInfo = new PlayerActionInfo
            {
                // どちらの手で投げられたかを変数に代入
                actHand = PlayerInputEvent.PlayerHand.Left
            };*/

            JoyToRay(_joyL.transform.position, _swordObjL.transform, actionInfo);
        }
    }

    /// <summary>
    /// Joyconの座標を習得してボタンをおすことでその座標にRayを飛ばす処理
    /// </summary>
    /// <param name="joyVec"> 座標を習得する変数 </param>>
    private void JoyToRay(Vector3 joyVec, Transform swordObj, PlayerActionInfo actionInfo)
    {
        Ray ray = _mainCamera.ScreenPointToRay(joyVec);

        // Rayを実際に飛ばす (ray, hitしたときの情報, 無限に飛ばす)
        // 何かしらにあたった場合 true 当たらなかった場合 false
        // なにか下に当たった時 かつ 投擲中じゃない時 (Playerを例外)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~LayerConstants.Player, QueryTriggerInteraction.Ignore))
        {
            // 剣を飛ばしたらプレイヤーの座標に追従しない様にparentを移す
            swordObj.parent = _flySwordParent.transform;
            // 向きに合わせて回転処理を行う
            swordObj.rotation = Quaternion.FromToRotation(swordObj.up, ray.direction)
                                * swordObj.rotation;

            // 再生するエフェクト選択
            EffectType trailEffectType = actionInfo.actHand switch
            {
                PlayerInputEvent.PlayerHand.Left => EffectType.SwordTrailL,
                PlayerInputEvent.PlayerHand.Right => EffectType.SwordTrailR
            };

            // 総距離
            float totalThrowingTime = Vector3.Distance(swordObj.position, hit.point) / _swordSpeed;

            // トレイルエフェクト再生
            // 引数 : EffectType , LocalVector3 , Rotation , ParentTransform , LoopFlag , LoopTime
            EffectManager.Instance.EffectPlay(trailEffectType, Vector3.zero, Quaternion.identity,
                swordObj, true, totalThrowingTime);

            _broker.Publish(
                PlayerEvent.OnBeginThrowSword.GetEvent(actionInfo.actHand, hit, _flySwordParent.transform, _swordSpeed));
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
