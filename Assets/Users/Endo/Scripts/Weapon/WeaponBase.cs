using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("先端位置 (投擲後の刺さる深さの位置となる)")]
    protected Collider tip;

    [SerializeField, Header("どちらの手で持つ剣か")]
    protected PlayerInputEvent.PlayerHand hand;

    protected Rigidbody selfRig;

    public readonly WeaponThrowingData throwingData = new();

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

        PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.OnBeginThrowSword>()
                               .Where(data => data.Hand == hand)
                               .Subscribe(OnBeginThrow)
                               .AddTo(this);

        tip.OnTriggerEnterAsObservable()
           .Where(_ => throwingData.IsThrowing)
           .Subscribe(OnLanding)
           .AddTo(this);
    }

    protected virtual void OnBeginThrow(PlayerEvent.OnBeginThrowSword data)
    {
        Move(transform.position, data.Hit.point, data.Speed).Forget();

        switch (data.Hand)
        {
            // 投げたのを確認したのでステートとフラグをつける
            case PlayerInputEvent.PlayerHand.Right:
                PlayerAnimationManager.Instance.ThrowSwordR();
                // 武器投擲時にステート変更のイベントを発行
                PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(
                                                                    PlayerStatus.PlayerState.ThrowingR,
                                                                    PlayerStateChangeOptions.Add,
                                                                    null, null));

                break;

            case PlayerInputEvent.PlayerHand.Left:
                PlayerAnimationManager.Instance.ThrowSwordL();
                PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnStateChangeRequest.GetEvent(
                                                                    PlayerStatus.PlayerState.ThrowingL,
                                                                    PlayerStateChangeOptions.Add,
                                                                    null, null));

                break;
        }
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

        float angle = GetForwardSurfaceAngle();

        PlayerInputEventEmitter.Instance.Broker.Publish(
            PlayerEvent.OnLandingSword.GetEvent(throwingData.EndPos, angle, actionInfo));
    }

    protected float GetForwardSurfaceAngle()
    {
        float   angle = 0f;
        Vector3 dir   = tip.transform.position - transform.position;
        var     ray   = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~LayerConstants.Player,
                            QueryTriggerInteraction.Ignore))
        {
            // ray y軸の値を *180して、値が - になってしまうので +180をおこない値を補正
            // hit.normal 1 ~ -1;
            angle = hit.normal.y * 180f + 180f;
        }

        return angle;
    }

    public async UniTaskVoid Move(Vector3 startPos, Vector3 endPos, float throwSpeed)
    {
        throwingData.IsThrowing = true;
        throwingData.StartPos   = startPos;
        throwingData.EndPos     = endPos;

        Vector3 dir = startPos - endPos;

        // 剣の中心から剣先までの差分
        Vector3 swordCenterToTipDelta = transform.position - tip.transform.position;
        Vector3 lerpVec;
        float   moveRatio = 0;

        // 総距離
        float totalThrowingTime = Vector3.Distance(startPos, endPos) / throwSpeed;

        while (throwingData.IsThrowing)
        {
            moveRatio += Time.deltaTime / totalThrowingTime;
            lerpVec   =  Vector3.Lerp(startPos, endPos + swordCenterToTipDelta, moveRatio);
            selfRig.MovePosition(lerpVec);

            // 次のアップデートが呼ばれるタイミングを非同期でまつ
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
