using UniRx;
using UnityEngine;

public class EnemyManegerTest : MonoBehaviour
{
    [SerializeField]private HP _hp = new HP(50);
    [SerializeField]private int _InstanceID;
    private MessageBroker _broker = new MessageBroker();
    public IMessageBroker Broker => _broker;
    [SerializeField] private int _TestInstanceID;
    void Start()
    {
        _InstanceID = this.gameObject.GetInstanceID();
        _TestInstanceID = _InstanceID; 
        EnemyManeger.Instance.Broker.Publish(SetEnemyInstanceID.GetEvent(_InstanceID));
        
        EnemyManeger.Instance.Broker.Receive<OnEnemyDamage>().Where(x => x.InstanceID == _InstanceID)
            .Subscribe(x =>
            {
                _hp.Damage(x.AttackPower);
                Debug.Log(_InstanceID + " : " + _hp.Value);
            }).AddTo(this);
    }

    private void OnGUI()
    {
        if(GUILayout.Button("ダメージテスト"))
        {
            EnemyManeger.Instance.Broker.Publish(OnAttackEnemy.GetEvent(_TestInstanceID,new AttackPower(10)));
        }
    }
}
