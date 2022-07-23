public interface IDamageable
{
    public HP Hp { get; }

    /// <summary>
    /// ダメージを受けた際の処理
    /// <remarks>MessageBrokerでのReceive()のコールバックとして使用することを想定</remarks>
    /// </summary>
    public void OnDamage(AttackPower attackPower);
}
