using System;
using UnityEngine;
using UniRx;
using UnityEngine.Animations;

/// <summary>
/// イベントで受信したメッセージに応じてアクションを行うクラス
/// </summary>
/// TODO: singletonではなくす (暫定措置)
public class PlayerActionManager : SingletonMonoBehaviour<PlayerActionManager>
{
    [SerializeField, Tooltip("剣を投擲する用のクラス")]
    private WeaponThrowing _weaponThrowing;

    [SerializeField, Tooltip("プレイヤーの位置情報")]
    private Transform _playerTransform;
    
    // 追従させる親オブジェクト
    private Transform _parentObject;
    
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
        
        // プレイヤーの固定(親関係の追加)
        /*_broker.Receive<PlayerEvent.OnStateChanged>()
            .Subscribe(_ =>
            {
                
            });*/
        
        _broker.Receive<PlayerEvent.OnParentChangeToObject>()
            .Subscribe(x =>
            {
                _parentObject = x.HitObject;
            }).AddTo(this);
        
        _broker.Receive<PlayerEvent.OnStateChangeRequest>()
            .Where(request => request.StateChangeOptions == PlayerStateChangeOptions.Delete)
            .Where(request => request.State is PlayerStatus.PlayerState.TransferringL or PlayerStatus.PlayerState.TransferringR)
            .Subscribe(_ =>
            {
                ParentChangeToPlayer(_parentObject);
            }).AddTo(this);
        
        _broker.Receive<PlayerEvent.OnStateChangeRequest>()
            .Where(request => request.StateChangeOptions == PlayerStateChangeOptions.Delete)
            .Where(request => request.State is PlayerStatus.PlayerState.HangingL or PlayerStatus.PlayerState.HangingR)
            .Subscribe(_ =>
            {
                ParentChangeToPlayer(null);
            }).AddTo(this);
    }

    /// <summary>
    /// 追従する親オブジェクトを変更する処理
    /// </summary>
    private void ParentChangeToPlayer(Transform hitObject)
    {
        // nullで値が渡された場合か前に値が入っていた場合初期化
        if (hitObject == null)
        {
            _playerTransform.parent = null;
            
            return;
        }
        
        _playerTransform.parent = hitObject;
    }

    /*/// <summary>
    /// 追従する親オブジェクトを変更する処理 (プレイヤー)
    /// </summary>
    /// <param name="parentObject"> 追従する先 </param>
    /// <param name="actionInfo"> 右手か左手か </param>
    public void ParentChange(Transform parentObject, ParentConstraint parentConstraint)
    {
        // 値を入れるためにConstraintSourceを作る
        ConstraintSource constraintSource = new ConstraintSource();
        int weightMax = 1;

        // nullで値が渡された場合か前に値が入っていた場合初期化
        if (parentObject == null || parentConstraint.sourceCount >= 1)
        {
            parentConstraint.RemoveSource(0);

            return;
        }

        // なにかしらのデータが入っていたら代入を行う
        constraintSource.sourceTransform = parentObject;
        constraintSource.weight = weightMax;

        // 各処理が終わったら値の代入を行う
        parentConstraint.AddSource(constraintSource);
    }
    
    /// <summary>
    /// 追従する親オブジェクトを変更する処理 (剣の投擲処理)
    /// </summary>
    /// <param name="parentObject"> 追従する先 </param>
    /// <param name="actionInfo"> 右手か左手か </param>
    public void ParentChange(Transform parentObject, PlayerActionInfo actionInfo,
        ParentConstraint parentConstraint, PlayerInputEvent.PlayerHand hand)
    {
        // 引数で渡されたどちらの手で投げている情報と自身の剣の情報を見て違ったら処理終了
        if (actionInfo.actHand != hand)
            return;
        
        // 値を入れるためにConstraintSourceを作る
        ConstraintSource constraintSource = new ConstraintSource();
        int weightMax = 1;

        // nullで値が渡された場合か前に値が入っていた場合初期化
        if (parentObject == null || parentConstraint.sourceCount >= 1)
        {
            parentConstraint.RemoveSource(0);

            return;
        }

        // なにかしらのデータが入っていたら代入を行う
        constraintSource.sourceTransform = parentObject;
        constraintSource.weight = weightMax;

        // 各処理が終わったら値の代入を行う
        parentConstraint.AddSource(constraintSource);
    }*/
}
