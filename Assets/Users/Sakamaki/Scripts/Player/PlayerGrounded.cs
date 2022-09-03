using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    // プレイヤーの接地判定を習得するクラス
    public static bool isGrounded = false;

    private float _elapsedTimeInAir;

    private void Update()
    {
        GroundedPublishEvent();

        // 空中にいる時間を計測
        if (!isGrounded)
        {
            _elapsedTimeInAir += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // tagが "Ground" の時フラグをtrueにする
        // 文字列の比較はEqualsを使うと丁寧○ !
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Ground"))
        {
            isGrounded = true;

            // 着地イベント発行
            PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.OnLanding.GetEvent(_elapsedTimeInAir));

            // 滞空時間をリセット
            _elapsedTimeInAir = 0f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Ground"))
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// ジャンプした後に着地を感知してイベントを発行する関数
    /// </summary>
    private void GroundedPublishEvent()
    {
        // ジャンプステート中にisGroundedがtrueになったら
        if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.Jump) && isGrounded)
        {
            // ジャンプのフラグを落とす
            PlayerInputEventEmitter.Instance.Broker.Publish(
                PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Jump, PlayerStateChangeOptions.Delete,
                    null, null));
        }
    }
}
