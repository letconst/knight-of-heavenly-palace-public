using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct EnemyStatus
{
    public int HP;
    public int AttackPower;
}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private EnemyStatus _EnemyStatus = new EnemyStatus();

    [SerializeField]
    private int _InstanceID;

    [SerializeField]
    private string masterId = "-1";

    protected HP          HP;
    protected AttackPower AttackPower;

    protected UniTask<MasterEnemy> _masterDataTask;
    protected MasterEnemy          _masterData;

    private MessageBroker  _broker = new MessageBroker();
    public  IMessageBroker Broker => _broker;

    public enum BodyPart
    {
        None,
        Head,
        Wings,
        Body,
    }

    protected virtual async void Awake()
    {
        //自身のInstanceID
        _InstanceID = GetInstanceID();

        Assert.IsFalse(masterId.Equals("-1"), "masterId is default value (-1)");
        Assert.IsNotNull(MasterDataManager.Instance, "MasterDataManager.Instance is null");

        // マスターデータ取得
        _masterDataTask = MasterDataManager.Instance.GetMasterDataAsync<MasterEnemy>(masterId);
        _masterData     = await _masterDataTask;
        OnMasterDataLoaded();
    }

    protected virtual async void Start()
    {
        //Enemy生成時にEnemyManegerに登録する処理
        EnemyManeger.Instance.Broker.Publish(SetEnemyInstanceID.GetEvent(_InstanceID));

        //ダメージの処理
        EnemyManeger.Instance.Broker.Receive<OnEnemyDamage>()
                    //攻撃対象が自分かどうかを判定
                    .Where(x => x.InstanceID == _InstanceID)
                    .Subscribe(x => { HP.Damage(x.AttackPower); })
                    .AddTo(this);

        //Destroyの処理
        Broker.Receive<OnStatusDestroy>()
              .Subscribe(_ =>
              {
                  OnDead();
              })
              .AddTo(this);
    }

    public abstract void Attack();

    protected abstract void OnDead();

    /// <summary>
    /// マスターデータ読み込み完了時の処理
    /// </summary>
    protected virtual void OnMasterDataLoaded()
    {
        _EnemyStatus = new EnemyStatus
        {
            HP          = _masterData.MaxHitPoint,
            AttackPower = _masterData.BaseAttackPower
        };

        HP          = new HP(_EnemyStatus.HP, int.MaxValue, Broker, _InstanceID);
        AttackPower = new AttackPower(_EnemyStatus.AttackPower);
    }
}
