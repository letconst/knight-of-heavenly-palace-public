using Unity.VisualScripting;using UnityEngine;

public sealed class OnStatusDestroy : EventMessage<OnStatusDestroy,int>
{
    public int InstanceID => param1;
}
public sealed class OnEnemyDamage : EventMessage<OnEnemyDamage,int,AttackPower>
{
    public int InstanceID => param1;
    public AttackPower AttackPower => param2;
}

public sealed class SetEnemyInstanceID : EventMessage<SetEnemyInstanceID, int>
{
    public int InstanceID => param1;
}
public sealed class OnAttackEnemy : EventMessage<OnAttackEnemy, int,AttackPower>
{
    public int InstanceID => param1;
    public AttackPower AttackPower => param2;
}