using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody))]
public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("先端位置 (投擲後の刺さる深さの位置となる)")]
    protected Collider tip;
    
    [SerializeField, Header("ParentConstraintのオブジェクト")]
    protected ParentConstraint _parentConstraint;
    
    [SerializeField, Header("どちらの手で持つ剣か")]
    protected PlayerInputEvent.PlayerHand hand;

    // 剣がホーミングしているかどうかの変数
    private bool _isHoming = false;

    protected Rigidbody selfRig;

    public readonly WeaponThrowingData throwingData = new();

    protected readonly Dictionary<int, System.Action> onBeginThrowHandAction = new()
    {
        {
            (int) PlayerInputEvent.PlayerHand.Left, () =>
            {
                PlayerAnimationManager.Instance.ThrowSwordL();
                // 武器投擲時にステート変更のイベントを発行
                PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(
                                                                    PlayerStatus.PlayerState.ThrowingL,
                                                                    PlayerStateChangeOptions.Add,
                                                                    null, null));
            }
        },
        {
            (int) PlayerInputEvent.PlayerHand.Right, () =>
            {
                PlayerAnimationManager.Instance.ThrowSwordR();
                // 武器投擲時にステート変更のイベントを発行
                PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(
                                                                    PlayerStatus.PlayerState.ThrowingR,
                                                                    PlayerStateChangeOptions.Add,
                                                                    null, null));
            }
        }
    };

    protected virtual void Start()
    {
        selfRig = GetComponent<Rigidbody>();

        if (hand == PlayerInputEvent.PlayerHand.Left)
        {
            WeaponThrowing.Instance.SetLeftTargetWeapon(throwingData);
        }
        else if (hand == PlayerInputEvent.PlayerHand.Right)
        {
            WeaponThrowing.Instance.SetRightTargetWeapon(throwingData);
        }

        // 投擲開始時イベントの受付
        PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.OnBeginThrowSword>()
                               .Select(data => data.Params)
                               .Where(data => data.Hand == hand) // 同じ手の通知のみ処理
                               .Subscribe(OnBeginThrow)
                               .AddTo(this);

        // 剣先の着弾イベントの受付
        tip.OnTriggerEnterAsObservable()
           .Where(_ => throwingData.IsThrowing)
           .Subscribe(OnLanding)
           .AddTo(this);

        // 追従先変更イベントの受付
        PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.OnParentChangeToObject>()
                                                        .Where(x => x.ActionInfo.actHand == hand) //　同じ手なら処理
                                                        .Subscribe(x =>
                                                        { 
                                                            ParentChangeToSword(x.HitObject, x.ActionInfo);
                                                        })
                                                        .AddTo(this);

        // 剣の情報を渡す
        PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.GetSwordPosition>()
                                                        .Where(x => x.ActionInfo.actHand == hand)
                                                        .Subscribe(x =>
                                                        {
                                                            GetSwordPositionParams swordParams =
                                                            new GetSwordPositionParams()
                                                            {
                                                                swordPosition = this.transform.position,
                                                                actionInfo = x.ActionInfo
                                                            };
                                                            
                                                            x.Response.OnNext(swordParams);
                                                        })
                                                        .AddTo(this);
    }

    /// <summary>
    /// 投擲開始時の処理
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual void OnBeginThrow(WeaponThrowParams parameters)
    {
        // 当たった対象が魔物ならホーミングで飛ばす
        if (parameters.Target)
        {
            HomingMove(transform.position, parameters.Target, parameters.Speed).Forget();
        }
        // 魔物でなければ通常投擲
        else
        {
            NormalMove(transform.position, parameters.Hit.point, parameters.Speed).Forget();
        }

        // 手に応じた処理を実行
        onBeginThrowHandAction[(int) parameters.Hand]();
    }

    /// <summary>
    /// 投擲時に着弾した際の処理
    /// </summary>
    protected virtual void OnLanding(Collider other)
    {
        // プレイヤーには刺さらせない
        if (1 << other.gameObject.layer == LayerConstants.Player)
            return;

        // triggerには反応させない
        if (other.isTrigger)
            return;

        // 投擲状態解除
        throwingData.IsThrowing = false;
        
        var actionInfo = new PlayerActionInfo
        {
            actHand = hand
        };

        // 刺さった角度などを計算するため、対象にもう一度軌道上にrayを放つ
        RaycastHit? hit = GetForwardHit();
        
        if (hit == null) return;

        // ホーミング(追従中のみ差分処理を行う)
        if (_isHoming)
        {   
            // 初期化
            _isHoming = false;
            
            // 差分を求める
            Vector3 swordCenterToTipDelta = transform.position - tip.transform.position;
            // 剣のズレを修正するために代入をおこなう
            transform.position = hit.Value.point;
            transform.position += swordCenterToTipDelta;
        }
        
        // ray y軸の値を *180して、値が - になってしまうので +180をおこない値を補正
        // hit.normal 1 ~ -1;
        float angle = hit.Value.normal.y * 180f + 180f;

        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnLandingSword.GetEvent(hit.Value.point, angle, actionInfo));
        
        // 親を変えるためにイベントの発行
        PlayerInputEventEmitter.Instance.Broker.
            Publish(PlayerEvent.OnParentChangeToObject.GetEvent(hit.Value.transform, this.transform, actionInfo));
    }

    /// <summary>
    /// 武器の投擲進行方向に対し、現在地より少し手前から飛ばしたrayのhit情報を取得する
    /// </summary>
    /// <returns></returns>
    protected RaycastHit? GetForwardHit()
    {
        Vector3 dir       = tip.transform.position - transform.position;
        Vector3 rayOrigin = transform.position     - dir * 5f;
        var     ray       = new Ray(rayOrigin, dir);

        // ray確認用
        // Debug.DrawLine(transform.position, rayOrigin, Color.red, 5f);
        // Debug.DrawLine(rayOrigin, transform.position - dir * 5.1f, Color.green, 5f);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~LayerConstants.Player,
                            QueryTriggerInteraction.Ignore))
        {
            // Debug.DrawLine(rayOrigin, hit.point, Color.cyan, 5f);

            return hit;
        }

        return null;
    }

    /// <summary>
    /// 通常投擲時の武器の座標更新処理
    /// </summary>
    /// <param name="startPos">開始地点</param>
    /// <param name="endPos">着弾地点</param>
    /// <param name="throwSpeed">投擲速度</param>
    protected async UniTaskVoid NormalMove(Vector3 startPos, Vector3 endPos, float throwSpeed)
    {
        throwingData.IsThrowing = true;
        throwingData.StartPos   = startPos;
        throwingData.EndPos     = endPos;

        // 剣の中心から剣先までの差分
        Vector3 swordCenterToTipDelta = transform.position - tip.transform.position;
        Vector3 lerpVec;
        float   moveRatio = 0;

        // 総距離
        float totalThrowingTime = Vector3.Distance(startPos, endPos) / throwSpeed;

        while (throwingData.IsThrowing)
        {
            moveRatio += Time.fixedDeltaTime / totalThrowingTime;
            lerpVec   =  Vector3.Lerp(startPos, endPos + swordCenterToTipDelta, moveRatio);
            selfRig.MovePosition(lerpVec);

            // 次のFixedUpdateが呼ばれるタイミングを非同期でまつ
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }

    /// <summary>
    /// ホーミング時の武器の座標更新処理
    /// </summary>
    /// <param name="startPos">開始地点</param>
    /// <param name="target">ホーミング対象</param>
    /// <param name="throwSpeed">投擲速度</param>
    protected async UniTaskVoid HomingMove(Vector3 startPos, Transform target, float throwSpeed)
    {
        throwingData.IsThrowing = true;
        throwingData.StartPos   = startPos;

        _isHoming = true;
        
        while (throwingData.IsThrowing)
        {
            float   speed = throwSpeed * Time.fixedDeltaTime;
            Vector3 dir   = target.position - transform.position;

            // 座標更新
            selfRig.MovePosition(Vector3.MoveTowards(transform.position, target.position, speed));

            // 向きに合わせて回転
            selfRig.MoveRotation(Quaternion.FromToRotation(transform.up, dir) * transform.rotation);

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }
    
        
    /// <summary>
    /// 追従する親オブジェクトを変更する処理
    /// </summary>
    private void ParentChangeToSword(Transform hitObject, PlayerActionInfo actionInfo)
    {
        if (hitObject == null)
        {
            WeaponThrowing.Instance.SwordPositionReset(actionInfo.actHand);
        }
        else
        {
            transform.parent = hitObject;   
        }
    }
}