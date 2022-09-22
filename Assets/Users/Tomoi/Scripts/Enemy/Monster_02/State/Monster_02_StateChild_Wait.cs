using UnityEngine;

public class Monster_02_StateChild_Wait : StateChildBase
{
    // タイムカウント
    float currentTimeCount;

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

        if (currentTimeCount >= waitDuration)
        {
            return (int)Monster_02_StateController.StateType.Move;
        }

        return (int)StateType;
    }
}
