using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの行動でステートの分岐、実行を行うクラス
public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private WeaponThrowing _weaponThrowing;
    [SerializeField] private WeaponPulling _weaponPulling;
    [SerializeField] private WeaponDelivery _weaponDelivery;

    private void Start()
    {
        // Stateの初期化
        PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordDelivery;
    }

    private void Update()
    {
        DistributeAttackState();
    }

    /// <summary>
    /// Stateで分岐をおこなう (攻撃関連)
    /// </summary>
    private void DistributeAttackState()
    {
        // 右手のステート
        switch (PlayerStatus.playerAttackState)
        {
            case PlayerStatus.PlayerAttackState.None:
                break;
            case PlayerStatus.PlayerAttackState.SwordPulling:     // 抜刀状態
                _weaponPulling.Pulling();
                
                // ZR or ZLが押されたら投擲モードに変更
                if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.ZR) ||
                         SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.ZL) || Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.Throwing;
                    _weaponThrowing.Throwing();
                }
                break;
            case PlayerStatus.PlayerAttackState.Throwing:        // 投擲モード
                _weaponThrowing.Throwing();
                
                // Yボタンを押された時は納刀
                if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.Y) || Input.GetKeyDown(KeyCode.Z))
                {
                    PlayerStatus.playerAttackState = PlayerStatus.PlayerAttackState.SwordDelivery;
                }
                break;
            case PlayerStatus.PlayerAttackState.SwordDelivery:  // 納刀状態
                _weaponDelivery.Delivery();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Stateで分岐を行う (移動関連)
    /// </summary>
    private void DistributeMoveState()
    {
        switch (PlayerStatus.playerMoveState)
        {
            case PlayerStatus.PlayerMoveState.None:
                break;
            case PlayerStatus.PlayerMoveState.Dash:
                break;
            case PlayerStatus.PlayerMoveState.Jump:
                break;
            case PlayerStatus.PlayerMoveState.Dodge:
                break;
            default:
                break;
        }
    }
}
