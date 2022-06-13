using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StatusClassTest : MonoBehaviour
{
    private HP _hp;
    private SP _sp;
    private MP _mp;
    private readonly MessageBroker _broker = new();
    public IMessageBroker Broker => _broker;

    [SerializeField] private Text DebugText;

    private void Start()
    {
        Broker.Receive<OnStatusDestroy>()
            .Subscribe(_ =>
            {
                Debug.Log("HPが0になりました");
            });
        _hp = new HP(100,100,Broker);
        _sp = new SP(100,100);
        _mp = new MP(100,100);
    }

    public void HPDamageButton()
    {
        _hp.Damage(new AttackPower(10));
    }

    public void HPRecoveryButton()
    {
        _hp.Recovery(new HP(10));
    }

    public void MPConsumptionButton()
    {
        _mp.Consumption(new MP(10));
    }

    public void MPRecoveryButton()
    {
        _mp.Recovery(new MP(10));
    }
    public void SPConsumptionButton()
    {
        _sp.Consumption(new SP(10));
    }

    public void SPRecoveryButton()
    {
        _sp.Recovery(new SP(10));
    }
    private void Update()
    {
        DebugText.text = "HP :" + _hp.Value.ToString() + "\n" +
                         "MP :" + _mp.Value.ToString() + "\n" +
                         "SP :" + _sp.Value.ToString();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("DamageButton"))
        {
            HPDamageButton();
        }
        if (GUILayout.Button("RecoveryButton"))
        {
            HPRecoveryButton();
        }
        if (GUILayout.Button("MPConsumptionButton"))
        {
            MPConsumptionButton();
        }
        if (GUILayout.Button("MPRecoveryButton"))
        {
            MPRecoveryButton();
        }
        if (GUILayout.Button("SPConsumptionButton"))
        {
            SPConsumptionButton();
        }
        if (GUILayout.Button("SPRecoveryButton"))
        {
            SPRecoveryButton();
        }
    }

}
