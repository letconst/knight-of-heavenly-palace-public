using UnityEngine;

/// <summary>
/// 回避処理が記述してあるMovementのpartialクラス
/// </summary>
public partial class PlayerMovement
{
    /// <summary>
    /// プレイヤーの回避処理
    /// </summary>
    /// <param name="dashSpeed"> 緊急回避する速度 </param>>
    /// <param name="moveObject"> 緊急回避するオブジェクト </param>>
    /// <param name="staminaReduceNum">　緊急回避に使うスタミナ減少値　</param>
    private void Dodge(float dashSpeed, Rigidbody moveObject, float staminaReduceNum)
    {
        // 消費スタミナ分スタミナを減らす
        PlayerInputEventEmitter.Instance.Broker.
            Publish(PlayerEvent.ReduceStamina.GetEvent(staminaReduceNum));
        
        /*
        Debug.Log("消費スタミナ：" + staminaReduceNum);
        Debug.Log("最大値スタミナ：" + PlayerHealth.Instance.Sp.MaxValue);
        Debug.Log("現在のスタミナ：" + PlayerHealth.Instance.Sp.Value);
        */
        
        // 回避用のvector3変数を用意
        Vector3 dodgeVelocity = _moveForward * dashSpeed;

        // 現在倒されているvelocityと回避用のvelocityを加算する
        rigidbody.AddForce(moveObject.mass * dodgeVelocity / Time.deltaTime, ForceMode.Impulse);
    }
}
