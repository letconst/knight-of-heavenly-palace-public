using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dragon_StateChild_Move : StateChildBase
{
    private float moveTime ,currentTimeCount= 0f;
    [SerializeField, Header("移動スピード")]
    private float _moveSpeed = 10f;

    [SerializeField, Header("Y軸の誤差範囲")] private float YAxitsErrorRange = 1;

    [SerializeField,Header("移動範囲のコライダー")] private SphereCollider ColliderMoveRange;

    public override void OnEnter()
    {
        //変数定義
        moveTime = Random.Range(3f, 8f);
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
    }

    public override int StateUpdate()
    { 
        //時間がないのでとりあえず疑似非同期処理
        currentTimeCount += Time.deltaTime;

        if(moveTime <= currentTimeCount)
        {
            //次のステートに移動
            return (int)Dragon_StateController.StateType.Interval;
        }
        //前方向に進ませつつ、Y座標が一定値を越していないなら上方向のベクトルも追加する
        Vector3 v = transform.forward;
        if (transform.position.y <= controller.WorldYPosition(1) - YAxitsErrorRange)
        {
            v += Vector3.up;
        }else if (transform.position.y <= controller.WorldYPosition(1) + YAxitsErrorRange)
        {
            v -= Vector3.up;
        }
        
        controller.rigidbody.AddForce(v * _moveSpeed, ForceMode.Acceleration);
        return (int)StateType;
    }
}
