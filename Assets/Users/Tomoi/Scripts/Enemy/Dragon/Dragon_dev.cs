using UnityEngine;

public class Dragon_dev : EnemyBase, IDamageable
{
    public HP Hp { get; }

    // ステートコントローラー
    [SerializeField] Dragon_StateController stateController = default;

    private Animator Animator;

    private Dragon_StateController.StateType nextState = Dragon_StateController.StateType.Null;
    private bool nextFrameChangeState = false;


    private static readonly int id_Flapping = Animator.StringToHash("flapping");
    private static readonly int id_Gliding = Animator.StringToHash("gliding");

    protected override void Start()
    {
        base.Start();
        stateController.Initialize((int)Dragon_StateController.StateType.Wait);
        //アニメーション再生
        Animator = GetComponent<Animator>();
        Animator.SetBool(id_Flapping,true);
        Animator.SetBool(id_Gliding,true);
    }

    void Update()
    {
        //ステートの割り込み
        if (nextFrameChangeState)
        {
            nextFrameChangeState = false;
            stateController.UpdateSequence((int)nextState);
        }
        else
        {
        }
        stateController.UpdateSequence();

    }


    #region コライダーに入ってきたときの処理

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TaskCancel();
            stateController.playerIsInRange = true;
            nextState = Dragon_StateController.StateType.Discover;
            nextFrameChangeState = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TaskCancel();
            stateController.playerIsInRange = false;
            nextState = Dragon_StateController.StateType.Lost;
            nextFrameChangeState = true;
        }
    }

    #endregion

    public override void Attack()
    {
        nextFrameChangeState = true;
        nextState = Dragon_StateController.StateType.AttackMove;
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
