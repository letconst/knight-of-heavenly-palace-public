using UniRx;
using UniRx.Triggers;
using UnityEngine;

public partial class PlayerAttackController
{
    [SerializeField]
    private Collider attackCol;

    private void StartDebug()
    {
        attackCol.enabled = false;

        attackCol.OnTriggerEnterAsObservable()
                 .Where(other => !other.isTrigger)
                 .Where(other => other.GetComponent<IDamageable>() != null)
                 .Subscribe(OnHit)
                 .AddTo(this);
    }

    private void OnHit(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();

        damageable.OnDamage(new AttackPower(1));
    }
}
