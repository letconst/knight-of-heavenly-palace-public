using UniRx;
using UnityEngine;

public sealed class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float damageInterval;

    private Subject<AttackPower> _onDamage = new();

    public HP Hp { get; private set; }

    public SP Sp { get; private set; }

    private async void Awake()
    {
        MasterPlayer masterData = await MasterDataManager.Instance.GetPlayerMasterDataAsync();

        Hp = new HP(masterData.MaxHitPoint, masterData.MaxHitPoint);
        Sp = new SP(masterData.MaxStaminaPoint, masterData.MaxStaminaPoint);

        _onDamage.ThrottleFirst(System.TimeSpan.FromSeconds(damageInterval))
                 .Subscribe(attackPower =>
                 {
                     Hp.Damage(attackPower);

                     float hpRatio = 1f * Hp.Value / Hp.MaxValue;

                     StatusBarReceiver.Instance.HpBarRegister.OnNext(hpRatio);
                 })
                 .AddTo(this);
    }

    public void OnDamage(AttackPower attackPower)
    {
        _onDamage.OnNext(attackPower);
    }
}
