using UnityEngine;

public class Dragon_StateChild_Discover : StateChildBase
{
    // タイムカウント
    float currentTimeCount;
    [SerializeField, Header("振り向き速度")] private float rotationSpeed = 0.2f;
    Vector3 aim;
    Quaternion look;
    // 待機時間
    [SerializeField,Header("待機時間")]
    private float waitDuration = 2f;
    public override void OnEnter()
    {
        currentTimeCount = 0f;
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        currentTimeCount += Time.deltaTime;

        //ゆっくりプレイヤーの方向を向く
        aim = controller.playerTransform.position - transform.position;
        aim.y = 0f;
        look = Quaternion.LookRotation(aim);
        var q = Quaternion.Slerp(transform.rotation, look, rotationSpeed);
        transform.localRotation = q;
        
        if (currentTimeCount >= waitDuration)
        {
            return (int)Dragon_StateController.StateType.AttackMove;
        }

        return (int)StateType;
    }
}
