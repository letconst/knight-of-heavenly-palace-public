using UnityEngine;

public class Dragon_StateChild_AttackBreath : StateChildBase
{
    private float currentTimeCount= 0f;

    [SerializeField, Header("口のトランスフォーム")] 
    private Transform mouthPosition;
    
    /*計算用の変数たち*/
    Vector3 aim;
    Quaternion look;
    Vector2 tmpPlayerV2, tmpDragonV2;
    bool isBreathed;
    
    // 視野角（度数法）
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    private bool onConeJudgment = false;
    public override void OnEnter()
    {
        currentTimeCount = 0f;

        isBreathed = false;
        onConeJudgment = false;
        //一度だけプレイヤーの方向を見る
        aim = controller.playerTransform.position - transform.position;
        aim.y = 0f;
        look = Quaternion.LookRotation(aim);
        transform.localRotation = look;
        
        //円錐の内側か判定
        
        // 自身の位置
        var selfPos = mouthPosition.position;
        // ターゲットの位置
        var targetPos =  controller.playerTransform.position;

        // 自身の向き（正規化されたベクトル）
        var selfDir = mouthPosition.forward;
        
        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // 視界判定
        onConeJudgment = innerProduct > cosHalf && targetDistance < _maxDistance;
        Debug.Log(onConeJudgment);
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        currentTimeCount += Time.deltaTime;

        /*プレイヤーの方向を向く
         * 1秒待機
         * ブレスを吐く
         * 4秒待機
         * ステートを変更
        */
        if (1 <= currentTimeCount && !isBreathed)
        {
            isBreathed = true;

            if (onConeJudgment)
            {
                aim = controller.playerTransform.position - mouthPosition.position;
                look = Quaternion.LookRotation(aim);
                //ブレスを吐く
                EffectManager.Instance.EffectPlay(EffectType.FireBreath, mouthPosition.position + mouthPosition.forward,look);
            }
        }

        if (5 <= currentTimeCount)
        {
            return controller.playerIsInRange
                ? (int)Dragon_StateController.StateType.AttackMove
                : (int)Dragon_StateController.StateType.Interval;
        }
        return StateType;
    }
}
