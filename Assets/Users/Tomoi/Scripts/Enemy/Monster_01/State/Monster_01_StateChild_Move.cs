using System.Linq;
using UnityEngine;

public class Monster_01_StateChild_Move : StateChildBase
{
    private float moveTime ,currentTimeCount= 0f;
    [SerializeField, Header("移動スピード")]
    private float _moveSpeed = 10f;

    [SerializeField, Header("Y軸の誤差範囲")] private float YAxitsErrorRange = 1;

    [SerializeField,Header("移動範囲のコライダー")] private SphereCollider ColliderMoveRange;

    [SerializeField, Header("判定用のスフィアの半径")]
    private float directionSphereRadius = 1;
    [SerializeField, Header("地面に降りるときのY軸の距離")]
    private float groundDistance = 3f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField, Header("前に動くときの地面を判定するときの許容距離")] 
    private float forwardTolerance = 6f; 
    
    [SerializeField,Header("Y軸の最小距離")] private float yMinDistance = 2;
    [SerializeField,Header("Y軸の最大距離")] private float yMaxDistance = 4;

    

    public override void OnEnter()
    {
        controller.playerIsInRange = true;
        //変数定義
        moveTime = Random.Range(3f, 6f);
        currentTimeCount = 0f;
        //ランダムな方向を向く
        transform.rotation = Quaternion.Euler(transform.rotation.x, Random.Range(0f, 360f), transform.rotation.z);
        //到達予定地点がコライダーの内側にあるか
        //上下の判定を取らないので正確ではないが、次に移動するときにコライダー側に戻ってくる設計
        
        //前ベクトルの方向にmoveTimeの時間*_moveSpeedをかけた座標を取得
        Vector3 nomal = transform.forward.normalized;
        Vector3 scheduleTargetPosition = transform.forward + (nomal * (_moveSpeed * moveTime));
        //対象の座標がコライダーの外にある場合
        if(!Physics.OverlapSphere(scheduleTargetPosition, 0).Any(col => col == ColliderMoveRange))
        {
            Vector3 closestPosition = ColliderMoveRange.ClosestPoint(transform.position);
            var aim = closestPosition - transform.position;
            aim.y = 0;
            var look = Quaternion.LookRotation(aim);
            transform.rotation = look;
        }
    }

    public override void OnExit()
    {
        controller.rigidbody.velocity = Vector3.zero;
        controller.playerIsInRange = false;
    }

    public override int StateUpdate()
    { 
        //時間がないのでとりあえず疑似非同期処理
        currentTimeCount += Time.deltaTime;

        if(moveTime <= currentTimeCount)
        {
            //次のステートに移動
            return (int)Monster_01_StateController.StateType.Interval;
        }

        Vector3 v = Vector3.zero;
        //前方向ベクトルの追加
        if (Physics.Raycast(transform.position, transform.forward, forwardTolerance, groundLayerMask)) 
        { 
            controller.rigidbody.velocity = Vector3.zero; 
            v = Vector3.zero; 
        } 
        else 
        { 
            v = transform.forward; 
        } 

        //地面の方向に判定を出し、ヒットした地点との距離で上下のベクトルを追加する
        if (Physics.SphereCast(transform.position, directionSphereRadius, -Vector3.up, out RaycastHit raycastHit
                , Mathf.Infinity, groundLayerMask))
        {
            if (Mathf.Abs(Mathf.Abs(raycastHit.point.y) - Mathf.Abs(transform.position.y)) <= yMinDistance)
            {
                v += Vector3.up;
            }else if (yMaxDistance <= Mathf.Abs(Mathf.Abs(raycastHit.point.y) - Mathf.Abs(transform.position.y)))
            {
                v += -Vector3.up;
            }
        }
        controller.rigidbody.AddForce(v * _moveSpeed, ForceMode.Acceleration);
        return (int)StateType;
    }
}
