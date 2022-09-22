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
                 .Where(other => other.GetComponentInParent<EnemyBase>() != null)
                 .Subscribe(OnHit)
                 .AddTo(this);
    }

    private void OnHit(Collider other)
    {
        var target = other.GetComponentInParent<EnemyBase>();

        EnemyManeger.Instance.Broker.Publish(OnAttackEnemy.GetEvent(target.GetInstanceID(), new AttackPower(10)));
    }
}
