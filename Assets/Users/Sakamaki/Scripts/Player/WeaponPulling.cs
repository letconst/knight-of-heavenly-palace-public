using UnityEngine;

// 武器が抜刀されているときの処理クラス
public class WeaponPulling : MonoBehaviour
{
    /// <summary>
    /// 武器が抜刀されている時の処理関数
    /// </summary>
    public void Pulling()
    {
        // Yボタンで納刀を行う
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Y) || Input.GetKeyDown(KeyCode.Z))
        {
            /*PlayerStatus.playerAttackStateR = PlayerStatus.PlayerAttackState.SwordDelivery;*/
            Debug.Log("納刀");
        }
    }
}
