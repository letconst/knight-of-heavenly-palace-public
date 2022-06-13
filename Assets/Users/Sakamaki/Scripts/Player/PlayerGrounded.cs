using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    // プレイヤーの接地判定を習得するクラス
    public static bool isGrounded = false;

    private void OnCollisionEnter(Collision collision)
    {
        // tagが "Ground" の時フラグをtrueにする 
        // 文字列の比較はEqualsを使うと丁寧○ !
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Ground"))
        {
            isGrounded = false;
        }
    }
}
