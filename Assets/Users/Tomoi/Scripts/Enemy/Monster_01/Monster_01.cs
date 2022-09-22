using System;
using UnityEngine;

public class Monster_01 :  EnemyBase, IDamageable
{
    public HP Hp { get; }

    // ステートコントローラー
    [SerializeField] Monster_01_StateController stateController = default;

    private Animator Animator;

    private Monster_01_StateController.StateType nextState = Monster_01_StateController.StateType.Null;
    private bool nextFrameChangeState = false;


    private static readonly int id_Flapping = Animator.StringToHash("flapping");
    private static readonly int id_Gliding = Animator.StringToHash("gliding");
    protected override void Start()
    {
        base.Start();
        stateController.Initialize((int)Monster_01_StateController.StateType.Interval);
        //アニメーション再生
        Animator = GetComponent<Animator>();
        //Animator.SetBool(id_Flapping,true);
        //Animator.SetBool(id_Gliding,true);
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
