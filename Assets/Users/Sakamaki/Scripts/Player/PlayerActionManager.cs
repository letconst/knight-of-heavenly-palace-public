using System;
using UnityEngine;
using UniRx;

/// <summary>
/// イベントで受信したメッセージに応じてアクションを行うクラス
/// </summary>
/// TODO: singletonではなくす (暫定措置)
public class PlayerActionManager : SingletonMonoBehaviour<PlayerActionManager>
{
    [SerializeField, Tooltip("剣を投擲する用のクラス")]
    private WeaponThrowing _weaponThrowing;

    private IMessageBroker _broker;

    public WeaponThrowing WeaponThrowing => _weaponThrowing;

    private void Start()
    {
        // brokerの用意
        _broker = PlayerInputEventEmitter.Instance.Broker;

        // 剣の投擲処理
        _broker.Receive<PlayerEvent.Input.OnThrowWeapon>()
            .Subscribe(x =>
            {
                _weaponThrowing.Throwing(x.ActionInfo);
            }).AddTo(this);

        // リセットイベントを受信したら剣をリセットする
        _broker.Receive<PlayerEvent.OnWallResetSword>()
            .Subscribe(x =>
            {
                if (x.ActionInfo == null)
                {
                    _weaponThrowing.ResetSword();
                }
                else
                {
                    _weaponThrowing.ResetSword(x.ActionInfo.actHand, true);
                }
            }).AddTo(this);

        // 攻撃モードに変更
        _broker.Receive<PlayerEvent.Input.OnSwitchedAttack>()
            .Subscribe(_ =>
            {
                _broker.Publish(PlayerEvent.OnStateChangeRequest.
                    GetEvent(PlayerStatus.PlayerState.AttackMode, PlayerStateChangeOptions.Add, null, null));
            });
        
        // 投擲モードに変更
        _broker.Receive<PlayerEvent.Input.OnSwitchedThrow>()
            .Subscribe(_ =>
            {
                _broker.Publish(PlayerEvent.OnStateChangeRequest.
                    GetEvent(PlayerStatus.PlayerState.ThrowingMode, PlayerStateChangeOptions.Add, null, null));
            });
    }
}
