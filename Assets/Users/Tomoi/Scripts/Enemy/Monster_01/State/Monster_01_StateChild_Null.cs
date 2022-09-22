public class Monster_01_StateChild_Null : StateChildBase
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        return (int)Monster_01_StateController.StateType.Wait;
    }
}
