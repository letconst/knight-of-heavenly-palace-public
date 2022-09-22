using UniRx;
using UnityEngine;

public class HP
{
    public int Value { get;private set; }
    public int MaxValue { get;private set; }
    private int _InstanceID;

    private IMessageBroker Broker;

    /// <summary>初期化</summary>
    /// <param name="initValue">HPの初期値</param>
    /// <param name="initMaxValue">HPの最大値</param>
    /// <param name="broker">HPが0以下になった際に呼び出すOnStatusDestroyのIMessageBroker</param>
    /// <param name="_instanceID">生成元のオブジェクトのInstanceID</param>
    public HP(int initValue, int initMaxValue = int.MaxValue, IMessageBroker broker = null, int _instanceID = 0)
    {
        Value = 0 <= initValue ? initValue : 0;
        MaxValue = initMaxValue;
        Broker = broker;
        _InstanceID = _instanceID;
    }
    /// <summary>攻撃を受けたときのダメージの処理</summary>
    /// <param name="_hp">HPクラスのValueを参照</param>
    public void Damage(AttackPower _attackPower)
    {
        if (CheckPositiveNumber(_attackPower.Value))
        {
            Value -=_attackPower.Value;
            //HPが0以下になったときの処理
            if (Value <= 0)
            {
                //null条件演算子
                Broker?.Publish(OnStatusDestroy.GetEvent(_InstanceID));
            }
        }
    }
    /// <summary>回復の処理</summary>
    /// <param name="_hp">HPクラスのValueを参照</param>
    public void Recovery(HP _hp)
    {
        if (CheckPositiveNumber(_hp.Value))
        {
            Value +=_hp.Value;
            if (MaxValue <= Value)
            {
                Value = MaxValue;
            }
        }
    }

    /// <summary>正数のチェック</summary>
    /// <param name="_value"></param>
    private bool CheckPositiveNumber(int _value)
    {
        return 0 <= _value;
    }
}