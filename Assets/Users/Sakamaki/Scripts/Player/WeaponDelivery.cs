using UnityEngine;

// 武器が納刀されている時の処理クラス
public class WeaponDelivery : MonoBehaviour
{
    /// <summary>
    /// 武器が納刀されているときの処理関数
    /// </summary>
    public void Delivery()
    {
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.X) || Input.GetKeyDown(KeyCode.X))
        {
            PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordPulling;
            Debug.Log("抜刀");
        }
    }
}