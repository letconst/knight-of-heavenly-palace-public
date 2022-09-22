public class Monster_02_StateChild_Null : StateChildBase
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        return (int)Monster_02_StateController.StateType.Wait;
    }
}
