using UniRx;
using UnityEngine;

public sealed class PlayerHealth : SingletonMonoBehaviour<PlayerHealth>, IDamageable
{
    [SerializeField]
    private float damageInterval;

    private Subject<AttackPower> _onDamage = new();

    public HP Hp { get; private set; }

    public SP Sp { get; private set; }

    protected override async void Awake()
    {
        base.Awake();

        MasterPlayer masterData = await MasterDataManager.Instance.GetPlayerMasterDataAsync();

        Hp = new HP(masterData.MaxHitPoint, masterData.MaxHitPoint, PlayerInputEventEmitter.Instance.Broker);
        Sp = new SP(masterData.MaxStaminaPoint, masterData.MaxStaminaPoint);

        _onDamage.ThrottleFirst(System.TimeSpan.FromSeconds(damageInterval))
                 .Subscribe(attackPower =>
                 {
                     Hp.Damage(attackPower);
                     UpdateHpBar();
                 })
                 .AddTo(this);

        // スタミナ減少処理
        PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.ReduceStamina>()
            .Subscribe(x =>
            {
                Sp.Consumption(new SP(x.ReduceNum));
                UpdateSpBar();
            }).AddTo(this);
    }

    private void Update()
    {
         RecoveryStamina();
    }

    /// <summary>
    /// スタミナ自動回復処理
    /// </summary>
    private void RecoveryStamina()
    {
        if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL) ||
            PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR) ||
            PlayerStateManager.HasFlag(PlayerStatus.PlayerState.Dodge))
            return;

        if (PlayerStatus.playerMasterData == null)
            return;

        float recoveryNum = 0f;
        // プレイヤーが地面にいるときに回復できる倍率
        int groundedRatio = 2;

        // 毎秒の回復処理
        if (PlayerGrounded.isGrounded)
        {
            recoveryNum = Time.deltaTime * (PlayerStatus.playerMasterData.BaseAutoStaminaRecoveryQuantity) * groundedRatio;
        }
        else
        {
            recoveryNum = (Time.deltaTime * PlayerStatus.playerMasterData.BaseAutoStaminaRecoveryQuantity);
        }

        Sp.Recovery(new SP(recoveryNum));
        UpdateSpBar();
    }

    public void OnDamage(AttackPower attackPower)
    {
        _onDamage.OnNext(attackPower);
    }

    public void UpdateHpBar()
    {
        if (!StatusBarReceiver.Instance) return;

        float hpRatio = 1f * Hp.Value / Hp.MaxValue;
        StatusBarReceiver.Instance.HpBarRegister.OnNext(hpRatio);
    }

    public void UpdateSpBar()
    {
        if (!StatusBarReceiver.Instance) return;

        float spRatio = 1f * Sp.Value / Sp.MaxValue;
        StatusBarReceiver.Instance.SpBarRegister.OnNext(spRatio);
    }
}
