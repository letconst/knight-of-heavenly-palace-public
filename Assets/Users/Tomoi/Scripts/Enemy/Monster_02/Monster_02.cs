using UnityEngine;

public class Monster_02 :  EnemyBase, IDamageable
{
    public HP Hp { get; }

    // ステートコントローラー
    [SerializeField] Monster_02_StateController stateController = default;

    private Animator Animator;

    private Monster_02_StateController.StateType nextState = Monster_02_StateController.StateType.Null;
    private bool nextFrameChangeState = false;


    protected override void Start()
    {
        base.Start();
        stateController.Initialize((int)Monster_02_StateController.StateType.Interval);
    }

    private void Update()
    {
        stateController.UpdateSequence();
    }

    public override void Attack()
    {
        //攻撃なし
    }

    protected override void OnDead()
    {
        // プレイヤーの張り付き解除
        // TODO: 別の場所に移動
        PlayerActionManager.Instance.WeaponThrowing.ResetSword();

        MainGameMissionManager.Instance.CountUp();

        // TODO:別のアニメーションに変更予定
        Destroy(gameObject);
    }

    public void OnDamage(AttackPower attackPower)
    {
        HP.Damage(attackPower);
    }
}
