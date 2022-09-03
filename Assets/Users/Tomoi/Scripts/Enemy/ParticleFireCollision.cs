using UnityEngine;

public class ParticleFireCollision : MonoBehaviour
{
    /// <summary>
    /// パーティクルが他のGameObject(Collider)に当たると呼び出される
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if (1 << other.layer == LayerConstants.Player)
        {
            var damageable = other.GetComponent<IDamageable>();

            // TODO: マスターデータからダメージ値持ってくる
            damageable?.OnDamage(new AttackPower(10));
        }
    }
}
