using UnityEngine;

public class Dragon_StateChild_Null : StateChildBase
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        //とりあえずwaitに返す
        return (int)Dragon_StateController.StateType.Wait;
    }
}
