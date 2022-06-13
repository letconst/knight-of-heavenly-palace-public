using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Enemy")
        {
            int InstanceID = other.gameObject.GetInstanceID();

            EnemyManeger.Instance.Broker.Publish(OnAttackEnemy.GetEvent(InstanceID, new AttackPower(10)));
        }
    }
}
