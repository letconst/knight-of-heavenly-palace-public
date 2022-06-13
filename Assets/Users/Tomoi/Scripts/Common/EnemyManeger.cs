using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyManeger : SingletonMonoBehaviour<EnemyManeger>
{
    [SerializeField] private List<int> _EnemyInstanceIDs = new List<int>();

    
    private readonly MessageBroker _broker = new();
    public IMessageBroker Broker => _broker;
    
    private void Awake()
    {
        //Enemyの生成時に受け取り、_EnemyInstanceIDsにInstanceIDをセットする
        Broker.Receive<SetEnemyInstanceID>()
            .Subscribe(x =>
            {
                _EnemyInstanceIDs.Add(x.InstanceID);
            }).AddTo(this);
        
        //Enemyを攻撃したときに受け取り、OnEnemyDamageを呼ぶ
        Broker.Receive<OnAttackEnemy>()
            .Subscribe(x =>
            {
                Broker.Publish(OnEnemyDamage.GetEvent(x.InstanceID,x.AttackPower));
            }).AddTo(this);
    }
    //TODO : 魔物の生成

    //TODO : 魔物の破棄
    /// <summary>
    /// _InstanceIDのオブジェクトのOnStatusDestroyを呼ぶ
    /// </summary>
    /// <param name="_InstanceID">InstanceID</param>
    private void Destroy(int _InstanceID)
    {
        Broker.Publish(OnStatusDestroy.GetEvent(_InstanceID));
    }
}
