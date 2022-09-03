using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyDragon : EnemyBase, IDamageable
{
    enum State
    {
        Idle, // Idle
        Move, // 移動
        Attack, // 攻撃
        Interval, // 少し待つとき用のステート
        Discover, // プレイヤーを発見
        Lost, // プレイヤーを見失う
        Null // Null
    }

    public HP Hp { get; private set; }
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Transform _player;

    [SerializeField] private State _state = State.Idle;
    [SerializeField] private State _oldState = State.Null;

    [FormerlySerializedAs("Interval")] [SerializeField]
    private float _interval = 5f;

    [SerializeField, Header("移動スピード")] private float _moveSpeed = 10f;

    /*攻撃系の情報*/
    [SerializeField, Header("攻撃時にプレイヤーに近づく距離")]
    private float _attackPlayerDistance = 10f;

    [SerializeField, Header("攻撃時の移動スピード")] private float _attackMoveSpeed = 10f;

    [SerializeField, Header("プレイヤーの真上からどれだけずれた位置に移動するか(XZ軸)")]
    private float _approachXZDistancePlayer = 5f;
    [SerializeField, Header("どのくらいプレイヤーの近くまで降りてくるか(Y軸)")]
    private float _approachYDistancePlayer = 5f;

    [SerializeField, Header("口のトランスフォーム")] private Transform _mouthPosition;

    /*キャンセルトークン*/
    private CancellationTokenSource _cts;
    private CancellationToken _ct;

    private bool playerIsInRange = false;

    /*上下に揺れる処理系の変数*/
    [SerializeField, Header("地面からの距離")] private float _groundToDistance = 20f;
    [SerializeField, Header("上下に揺れるスピード")] private float _fluffySpeed = 2f;
    [SerializeField, Header("ふわふわする可動域")] private float _fluffyRange = 1f;

    [SerializeField, Header("地面のレイヤーマスク")] private LayerMask _layerMask;

    /*ふわふわ揺れるタイミングのオフセット*/
    private float _fluffyTimeOffset = 0f;

    protected override void Start()
    {
        base.Start();

        _cts = new CancellationTokenSource();
        _ct = _cts.Token;
        _rigidbody = GetComponent<Rigidbody>();
        _state = State.Move;
        _fluffyTimeOffset = Random.Range(0f, 4f);
        //そのうち別の取得処理に置き換える
        _player = GameObject.Find("PlayerModel").transform;
    }

    public override void Attack()
    {
        _state = State.Attack;
    }

    protected override void OnDead()
    {
        // プレイヤーの張り付き解除
        // TODO: 別の場所に移動
        PlayerActionManager.Instance.WeaponThrowing.ResetSword();

        Destroy(gameObject);
    }

    void Update()
    {
        if (!playerIsInRange)
        {
            YPosition();
        }

        if (_oldState == _state) return;
        _rigidbody.velocity = Vector3.zero;
        TaskCancel();
        switch (_state)
        {
            case State.Idle:
            {
            }
                break;
            case State.Move:
            {
                c_Move(_ct).Forget();
            }
                break;

            case State.Attack:
            {
                c_Attack(_ct).Forget();
            }
                break;
            case State.Interval:
            {
                c_Interval(_ct).Forget();
            }
                break;

            case State.Discover:
            {
                c_Discover(_ct).Forget();
            }
                break;
            case State.Lost:
            {
                c_Lost(_ct).Forget();
            }
                break;
        }

        _oldState = _state;
    }

    /// <summary>
    /// キャンセルトークンの再生成
    /// </summary>
    private void TaskCancel()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        _ct = _cts.Token;
    }

    private void YPosition()
    {
        var _position = transform.position;
        //常にワールド座標上の下方向に向けてRayを生成する
        Ray ray = new Ray(_position, -Vector3.up);
        RaycastHit raycastHit;
        //上に移動
        if (Physics.Raycast(ray, out raycastHit, int.MaxValue, _layerMask))
        {
            if (!(raycastHit.point.y + _groundToDistance <= transform.position.y - fluffy()))
            {
                //TODO:瞬間的に上に移動するので徐々に上に持ち上げる処理を追加
                _position.y = raycastHit.point.y + _groundToDistance + 0.5f;
            }
        }

        _position.y += fluffy();
        transform.position = _position;
    }

    /// <summary>
    /// ふわふわ上下に揺れる計算用の関数
    /// </summary>
    /// <returns></returns>
    private float fluffy()
    {
        return Mathf.Sin((Time.time * _fluffySpeed) + _fluffyTimeOffset) / _fluffyRange;
    }

    /// <summary>
    /// ふわふわ上下に揺れる計算用の関数
    /// </summary>
    /// <returns></returns>
    private float fluffyHarf()
    {
        return Mathf.Sin(0.5f) / _fluffyRange;
    }

    #region コルーチン

    /* c_ == コルーチンの処理 */
    private async UniTaskVoid c_Move(CancellationToken _token = default)
    {
        //変数定義
        var time = 0f;
        var f = Random.Range(3f, 8f);

        //ランダムな方向を向く
        transform.rotation = Quaternion.Euler(transform.rotation.x, Random.Range(0f, 360f), transform.rotation.z);

        while (time <= f)
        {
            //キャンセル処理
            _token.ThrowIfCancellationRequested();
            time += Time.deltaTime;

            //前方に移動
            _rigidbody.AddForce(transform.forward * _moveSpeed, ForceMode.Acceleration);
            await UniTask.Yield();
        }

        _state = State.Interval;
    }


    /// <summary>
    /// 攻撃モーション
    /// </summary>
    /// <param name="_token">キャンセルトークン</param>
    private async UniTaskVoid c_Attack(CancellationToken _token = default)
    {
        Vector3 aim;
        Quaternion look;
        Vector2 tmpPlayerV2, tmpDragonV2;
        //プレイヤーに近づく
        while (_attackPlayerDistance <= Vector3.Distance(transform.position, _player.position))
        {
            _rigidbody.velocity = Vector3.zero;

            //キャンセル処理
            _token.ThrowIfCancellationRequested();

            //プレイヤーの方向を見る
            aim = _player.position - transform.position;
            aim.y = 0f;
            look = Quaternion.LookRotation(aim);
            transform.localRotation = look;

            tmpPlayerV2.x = transform.position.x;
            tmpPlayerV2.y = transform.position.z;
            tmpDragonV2.x = transform.position.x;
            tmpDragonV2.y = transform.position.z;
            //XZ軸の移動
            if (_approachXZDistancePlayer <= Mathf.Abs(Vector2.Distance(tmpPlayerV2,tmpDragonV2)))
            {
                _rigidbody.AddForce(transform.forward * _attackMoveSpeed, ForceMode.Acceleration);
            }
            //プレイヤーの真上にいるときの処理
            else if(Mathf.Abs(Vector2.Distance(tmpPlayerV2,tmpDragonV2)) <= 3)
            {
                //プレイヤーと反対の方向を見る
                aim = transform.position - _player.position;
                aim.y = 0f;
                look = Quaternion.LookRotation(aim);
                transform.localRotation = look;

                _rigidbody.AddForce(transform.forward * _attackMoveSpeed, ForceMode.Acceleration);

            }

            //Y軸の移動
            if (_approachYDistancePlayer <= Mathf.Abs(transform.position.y - _player.position.y))
            {
                _rigidbody.AddForce(
                    -Mathf.Sign(transform.position.y - _player.position.y) * Vector3.up * _attackMoveSpeed * 10,
                    ForceMode.Acceleration);
            }
            //プレイヤーに近づきすぎたときの処理
            else if(Mathf.Abs(transform.position.y - _player.position.y) <= _approachYDistancePlayer -3 )
            {
                _rigidbody.AddForce(
                    Mathf.Sign(transform.position.y - _player.position.y) * Vector3.up * _attackMoveSpeed * 10,
                    ForceMode.Acceleration);
            }

            await UniTask.Yield();
        }

        //プレイヤーの方向を見る
        aim = _player.position - transform.position;
        aim.y = 0f;
        look = Quaternion.LookRotation(aim);
        transform.localRotation = look;

        _rigidbody.velocity = Vector3.zero;
        aim = _player.position - _mouthPosition.position;
        look = Quaternion.LookRotation(aim);

        //攻撃モーション
        //1秒待機
        await UniTask.Delay(1000, cancellationToken: _token);

        //ブレスを吐く

        EffectManager.Instance.EffectPlay(EffectType.FireBreath, _mouthPosition.position, look);
        await UniTask.Delay(4000, cancellationToken: _token);

        //キャンセル処理
        _token.ThrowIfCancellationRequested();
        //あったっく
        await UniTask.Yield();

        _state = State.Interval;
    }

    /// <summary>
    /// 少し待つ処理
    /// </summary>
    /// <param name="_token">キャンセルトークン</param>
    private async UniTaskVoid c_Interval(CancellationToken _token = default)
    {
        //キャンセル処理
        //_token.ThrowIfCancellationRequested();
        await UniTask.Delay((int)(_interval * 1000), cancellationToken: _token);
        _state = playerIsInRange ? State.Attack : State.Move;
    }

    /// <summary>
    /// プレイヤーを発見したときの処理
    /// </summary>
    /// <param name="_token"></param>
    private async UniTaskVoid c_Discover(CancellationToken _token = default)
    {
        //キャンセル処理
        _token.ThrowIfCancellationRequested();
        await UniTask.Yield();
        _state = State.Attack;
    }

    /// <summary>
    /// プレイヤーを見失ったときの処理
    /// </summary>
    /// <param name="_token"></param>
    private async UniTaskVoid c_Lost(CancellationToken _token = default)
    {
        //キャンセル処理
        _token.ThrowIfCancellationRequested();
        await UniTask.Yield();
        _state = State.Interval;
    }

    #endregion

    public void OnDamage(AttackPower attackPower)
    {
        HP.Damage(attackPower);
    }

    #region コライダーに入ってきたときの処理

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TaskCancel();
            _state = State.Discover;
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TaskCancel();
            _state = State.Lost;
            playerIsInRange = false;
        }
    }

    #endregion
}
