using UnityEngine;
using UniRx;

public interface IEnemy 
{
    GameObject gameObject { get; }
    Transform transform { get; }
    IMessageBroker Broker { get; }
}
