using System.Collections;
using UnityEngine;

/// <summary>
/// ジャンプ処理を管理するPlayerMovementのpartialクラス
/// </summary>
public partial class PlayerMovement
{
    /// <summary>
    /// プレイヤーのジャンプ処理
    /// </summary>
    private void Jump(float jumpPower, Rigidbody jumpObj)
    {
        // ジャンプのフラグがtrueだったら(着地中だったら)
        if (PlayerGrounded.isGrounded)
        {
            jumpObj.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }
    }
}