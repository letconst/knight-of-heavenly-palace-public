using Cysharp.Threading.Tasks;
using UnityEngine;

// プレイヤーが武器を投擲したときの処理クラス
public class WeaponThrowing : MonoBehaviour
{
    [SerializeField] private GameObject _joyR;
    [SerializeField] private GameObject _joyL;
    [SerializeField] private GameObject _playerObject; //仮のプレイヤー座標習得 プロパティとか使って習得するとよさそう
    [SerializeField] private GameObject _swordObjR;
    [SerializeField] private GameObject _swordObjL;
    [SerializeField] private Transform _swordTipR;
    [SerializeField] private Transform _swordTipL;

    [SerializeField, Header("飛ばした後の剣の親オブジェクト")]
    private GameObject _flySwordParent;

    [SerializeField] private float _swordSpeed;

    // 剣が投げられているかどうかの変数
    private bool _isThrow = false;
    private bool _isRotate = false;
    private Camera _mainCamera;

    // 例外処理を行うplayerLayerを保持する変数
    private LayerMask _playerLayerMask;

    private void Start()
    {
        _mainCamera = Camera.main;
        _playerLayerMask = LayerMask.GetMask("Player");

        // 初期化で各剣をプレイヤーの子にする
        _swordObjR.transform.parent = _playerObject.transform;
        _swordObjL.transform.parent = _playerObject.transform;
    }

    /// <summary>
    /// 実際に武器を投げる処理
    /// </summary>
    public void Throwing()
    {
        // 投擲を行う
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.ZR) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            JoyToRay(_joyR.transform.position);
        }
        else if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.ZL))
        {
            JoyToRay(_joyL.transform.position);
        }
    }

    /// <summary>
    /// Joyconの座標を習得してボタンをおすことでその座標にRayを飛ばす処理
    /// </summary>
    /// <param name="joyVec"> 座標を習得する変数 </param>>
    private async void JoyToRay(Vector3 joyVec)
    {
        Ray ray;
        ray = _mainCamera.ScreenPointToRay(joyVec);

        // Rayを実際に飛ばす (ray, hitしたときの情報, 無限に飛ばす)
        // 何かしらにあたった場合 true 当たらなかった場合 false
        // なにか下に当たった時 かつ 投擲中じゃない時 (Playerを例外)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_playerLayerMask) && !_isThrow)
        {
            _isThrow = true;

            // 剣を飛ばしたらプレイヤーの座標に追従しない様にparentを移す
            _swordObjR.transform.parent = _flySwordParent.transform;
            // 向きに合わせて回転処理を行う
            _swordObjR.transform.rotation = Quaternion.FromToRotation(_swordObjR.transform.up, ray.direction)
                                            * _swordObjR.transform.rotation;

            Vector3 playerVec;
            Vector3 lerpVec;
            Vector3 rayHitPoint;
            float oneFrameMoveTime = 0.0f;
            playerVec = _playerObject.transform.position;
            rayHitPoint = hit.point;
            float calcSpeed = 0;

            int hitColliderNum = 0;

            _swordObjR.transform.position = playerVec;
            // 剣の中心から剣先までの差分
            Vector3 swordCenterToTipDelta = _swordObjR.transform.position - _swordTipR.position;
            
            // ループを行う
            while (true)
            {
                oneFrameMoveTime = Vector3.Distance(playerVec, rayHitPoint) / _swordSpeed;
                calcSpeed += Time.deltaTime / oneFrameMoveTime;
                
                lerpVec = Vector3.Lerp(playerVec, rayHitPoint + swordCenterToTipDelta, calcSpeed);

                // 剣を飛ばしてLerpで求めた値を剣のオブジェクトに加算
                _swordObjR.transform.position = lerpVec; //現在右手しか対応していないのでイベント関係ができたら対応を行う

                if (calcSpeed >= 1)
                {
                    // hitしたオブジェクトの座標をプレイヤー座標に代入
                    _playerObject.transform.position = rayHitPoint;
                    // 着地処理を行って代入したのでfalseにする
                    _isThrow = false;

                    break;
                }
                
                // 次のアップデートが呼ばれるタイミングを非同期でまつ
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}