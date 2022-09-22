using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class PlayerMovement
{
    [SerializeField, Header("リスポーンさせる位置オブジェクト")]
    private GameObject _respawnGameObject;

    /// <summary>
    /// 死んだときに行う処理関数
    /// </summary>
    private async void Death()
    {
        _isDead = true;

        // 死んだ処理に入ったのでステートをつける
        PlayerInputEventEmitter.Instance.Broker.
            Publish(PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Dead,
                                                                    PlayerStateChangeOptions.Add, null, null));

        PlayerAnimationManager.Instance.Death();

        await UniTask.Delay(1500);

        // 暗転します -ステートで死ぬ状態を作って-
        await FadeContllor2.Instance.FadeOutAsync(1);
        // プレイヤーの座標をキャンプに戻します
        _playerObject.transform.position = _respawnGameObject.transform.position;
        // HP全回復させます
        PlayerHealth.Instance.Hp.Recovery(new HP(PlayerHealth.Instance.Hp.MaxValue));
        PlayerHealth.Instance.UpdateHpBar();
        // 暗転戻します
        await FadeContllor2.Instance.FadeInAsync(1);

        // 死んだ処理がおわったのでステートを戻す
        PlayerInputEventEmitter.Instance.Broker.
            Publish(PlayerEvent.OnStateChangeRequest.GetEvent(PlayerStatus.PlayerState.Dead,
                                                                    PlayerStateChangeOptions.Delete, null, null));

        _isDead = false;
    }
}
