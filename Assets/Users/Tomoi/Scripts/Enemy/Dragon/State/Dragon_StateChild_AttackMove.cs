using UnityEngine;

public class Dragon_StateChild_AttackMove : StateChildBase
{
    private float moveRange, currentTimeCount = 0f;
    [SerializeField, Header("移動スピード")] private float _moveSpeed = 10f;

    [SerializeField, Header("口のトランスフォーム")] private Transform mouthPosition;

    [SerializeField, Header("Y軸の誤差範囲")] private float YAxitsErrorRange = 1;

    [SerializeField, Header("地面に降りるときのY軸の距離")]
    private float groundDistance = 3f;

    [SerializeField, Header("前に動くときの地面を判定するときの許容距離")]
    private float forwardTolerance = 3f;

    [SerializeField, Header("下方向に向けるスフィアの判定の半径")]
    private float directionDownSphereRadius = 1f;

    [SerializeField, Header("ドラゴンを中心とした地面までの半径")]
    private float dragonSphereRadius = 10f;

    [SerializeField, Header("振り向き速度")] private float rotationSpeed = 0.2f;

    [Header("- - 判定用 - -")] [Header("- どの距離まで近づいてくるか -")] [SerializeField, Header("プレイヤーの真上からどれだけずれた位置に移動するか(XZ軸)")]
    private float approachXZDistancePlayer = 5f;

    [SerializeField, Header("どのくらいプレイヤーの近くまで降りてくるか(Y軸)")]
    private float approachYDistancePlayer = 5f;

    [Header("- ドラゴンの前方にプレイヤーが来たときに視認できる角度と距離 -")] [SerializeField, Header("ドラゴン前方に作る円錐状の当たり判定の角度")]
    private float CheckConeAngle = 30f;

    [SerializeField, Header("ドラゴン前方に作る円錐状の当たり判定の距離")]
    private float CheckDistance = 10f;

    /// <summary>
    /// 地面のレイヤーマスク
    /// </summary>
    //private int groundLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    /*計算用の変数たち*/
    Vector3 aim;
    Quaternion look;
    Vector2 tmpPlayerV2, tmpDragonV2;
    private float ySign;

    private void Start()
    {
    }

    public override void OnEnter()
    {
        currentTimeCount = 0f;
    }

    public override void OnExit()
    {
        controller.rigidbody.velocity = Vector3.zero;
    }

    public override int StateUpdate()
    {
        //　ドラゴンの前方にプレイヤーが存在するか判定する計算
        //　自機前方ベクトルと対象への位置ベクトルで内積を求める
        float dot = Vector3.Dot(mouthPosition.forward, controller.playerTransform.position.normalized);
        //　コサインθを逆三角関数で角度に変換
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //　長さのチェック
        float distance = Vector3.Distance(controller.playerTransform.position, mouthPosition.position);
        //　斜辺と角度から底辺を求めて長さを定量化
        float dis = distance * dot;
        //　チェック
        bool angleCheck = angle <= CheckConeAngle;
        bool distanceCheck = dis <= CheckDistance;
        //  範囲内に入っていたら
        if (angleCheck && distanceCheck)
        {
            //ステート変更
            return (int)Dragon_StateController.StateType.AttackBreath;
        }

        //移動の処理
        {
            tmpPlayerV2.x = controller.playerTransform.position.x;
            tmpPlayerV2.y = controller.playerTransform.position.z;
            tmpDragonV2.x = transform.position.x;
            tmpDragonV2.y = transform.position.z;

            aim = controller.playerTransform.position - mouthPosition.position;
            ySign = Mathf.Sign(aim.y);
            //プレイヤーの真上以外ならプレイヤーの方向を向く
            if (!(Mathf.Abs(Vector2.Distance(tmpPlayerV2, tmpDragonV2)) <= approachXZDistancePlayer))
            {
                //プレイヤーの方向を見る
                aim.y = 0f;
                look = Quaternion.LookRotation(aim);
                var q = Quaternion.Slerp(transform.rotation, look, rotationSpeed);
                transform.localRotation = q;
            }

            Vector3 v;
            //XZ軸
            //前方向に進む
            //ただし、前方向に地面がある場合は動かない
            //また、プレイヤーのxz軸が一定値を越したら動かない
            if (Physics.Raycast(transform.position, transform.forward, forwardTolerance, groundLayerMask))
            {
                v = Vector3.zero;
            }
            else if (Mathf.Abs(Vector2.Distance(tmpPlayerV2, tmpDragonV2)) <= approachXZDistancePlayer)
            {
                v = Vector3.zero;
            }
            else
            {
                v = transform.forward;
            }

            //Y軸
            //プレイヤーのY座標が自分より下で尚且つ地面に近づいていい距離なら上方向のベクトルを追加する
            if (0 < ySign
                && transform.position.y <= controller.WorldYPosition(0) - YAxitsErrorRange)
            {
                v += Vector3.up;
            }
            //プレイヤーのY座標が自分より下で尚且つ地面に近づいていい距離なら下方向のベクトルを追加する
            else if (0 > ySign
                     && controller.WorldYPosition(0) + YAxitsErrorRange <= transform.position.y)
            {
                //常にワールド座標上の下方向に向けてRayを生成する
                if (Physics.SphereCast(transform.position, directionDownSphereRadius, -Vector3.up, out RaycastHit raycastHit
                        , groundDistance, groundLayerMask))
                {
                    v += Vector3.up;
                }
                else if (groundDistance <= Mathf.Abs(transform.position.y - raycastHit.point.y)
                         && 0 <= Mathf.Sign(transform.position.y - raycastHit.point.y))
                {
                    v -= Vector3.up;
                }
            }
            //判定内に地面が存在するなら配列内の0番の地点から方向ベクトルを生成し、反転してvに加算する
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, dragonSphereRadius, groundLayerMask);
            if (0 < hitColliders.Length)
            {
                foreach (Collider col in hitColliders)
                {
                    Vector3 VectorAwayGround = -(col.transform.position - transform.position).normalized;
                    v += VectorAwayGround * 2;
                }
            }
            //XZ軸の移動
            controller.rigidbody.AddForce(v * _moveSpeed, ForceMode.Acceleration);
        }

        return StateType;
    }
}
