using UnityEngine;

public class Monster_02_StateChild_Interval : StateChildBase
{
    // タイムカウント
    float currentTimeCount;
    [SerializeField,Header("インターバルの待ち時間")]
    private float _interval = 5f;
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

        if (_interval <= currentTimeCount)
        {
            //ステートの更新
            return (int)Monster_02_StateController.StateType.Move;
        }
        return (int)StateType;
    }
}
