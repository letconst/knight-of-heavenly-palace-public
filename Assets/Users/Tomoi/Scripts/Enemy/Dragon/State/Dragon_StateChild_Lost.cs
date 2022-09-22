using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_StateChild_Lost : StateChildBase
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override int StateUpdate()
    {
        return (int)Dragon_StateController.StateType.Interval;
    }
}
