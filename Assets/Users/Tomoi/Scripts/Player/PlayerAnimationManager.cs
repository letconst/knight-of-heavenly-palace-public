using UniRx;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : SingletonMonoBehaviour<PlayerAnimationManager>
{
    private Animator _animator;
    private static int _layerIndex_RightHand = 0;
    private static int _layerIndex_LeftHand = 0;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _layerIndex_RightHand = _animator.GetLayerIndex("RightHand");
        _layerIndex_LeftHand = _animator.GetLayerIndex("LeftHand");

        MotionEventReceiver();
    }
    // TODO: マスターデータなどに移す
    [SerializeField]
    private float landingMotionTimeThreshold;
    private void MotionEventReceiver()
    {
        IMessageBroker broker = PlayerInputEventEmitter.Instance.Broker;

        // 落下時
        broker.Receive<PlayerEvent.OnFalling>()
            .Subscribe(_ =>
            {
                FallingTrue();
                //Instance.SetBool(PlayerMotionController.MotionType.Falling, true);
            })
            .AddTo(this);

        // 着地時
        broker.Receive<PlayerEvent.OnLanding>()
            .Subscribe(data =>
            {
                // 滞空時間が一定以上なら再生
                if (data.InAirTime > Instance.landingMotionTimeThreshold)
                {
                    Landing();
                    //Instance.PlayTriggerMotion(PlayerMotionController.MotionType.Landing);
                    EffectManager.Instance.EffectPlay(EffectType.DustParticle, transform.position, Quaternion.identity);
                }

                FallingFalse();
                //Instance.SetBool(PlayerMotionController.MotionType.Falling, false);
            })
            .AddTo(this);
    }

    /// <summary>
    /// 現在の移動速度を引数に渡すとIdleからRunにアニメーションを推移しながら再生する
    /// </summary>
    /// <param name="f">移動速度</param>
    public void IdleAndRun(float f)
    {
        _animator.SetFloat(AnimatorHash.ID_IdleAndRun, Mathf.Clamp01(f));
    }

    /// <summary>
    /// ジャンプのアニメーションを再生する
    /// </summary>
    public void Jump()
    {
        _animator.SetTrigger(AnimatorHash.ID_Jump);
    }

    /// <summary>
    /// 地上で回転するアニメーションを再生する
    /// 注意：地上の判定処理はしていない
    /// </summary>
    public void DodgeInGround()
    {
        _animator.SetTrigger(AnimatorHash.ID_DodgeInGround);
    }

    /// <summary>
    /// 死亡アニメーションを再生する
    /// </summary>
    public void Death()
    {
        _animator.SetTrigger(AnimatorHash.ID_Death);
    }

    #region Falling

    /// <summary>
    /// 落ちるアニメーションを開始する
    /// </summary>
    public void FallingTrue()
    {
        _animator.SetBool(AnimatorHash.ID_Falling, true);
    }

    /// <summary>
    /// 落ちるアニメーションを解除する
    /// </summary>
    public void FallingFalse()
    {
        _animator.SetBool(AnimatorHash.ID_Falling, false);
    }

    /// <summary>
    /// FallingがTrueのときに呼び出すとLandingのアニメーションに推移する
    /// </summary>
    public void Landing()
    {
        if (_animator.GetBool(AnimatorHash.ID_Falling))
        {
            _animator.SetTrigger(AnimatorHash.ID_Landing);
        }
    }

    #endregion

    #region Attack

    /// <summary>
    /// 左手の地上での攻撃を再生する
    /// </summary>
    /// <param name="f">剣を振る角度 詳細はアニメーターを参照</param>
    public void AttackL(float f)
    {
        _animator.SetTrigger(AnimatorHash.ID_AttackL);
        _animator.SetFloat(AnimatorHash.ID_AttackDirectionL, f);
    }

    /// <summary>
    /// 右手の地上での攻撃を再生する
    /// </summary>
    /// <param name="f">剣を振る角度 詳細はアニメーターを参照</param>
    public void AttackR(float f)
    {
        _animator.SetTrigger(AnimatorHash.ID_AttackR);
        _animator.SetFloat(AnimatorHash.ID_AttackDirectionR, f);
    }

    /// <summary>
    /// 通常時に右手の剣を投げるアニメーション
    /// </summary>
    public void ThrowSwordR()
    {
        _animator.SetTrigger(AnimatorHash.ID_ThrowSwordR);
    }

    /// <summary>
    /// 通常時に左手の剣を投げるアニメーション
    /// </summary>
    public void ThrowSwordL()
    {
        _animator.SetTrigger(AnimatorHash.ID_ThrowSwordL);
    }

    /// <summary>
    /// 左手のぶら下がり時の攻撃アニメーション
    /// </summary>
    /// <param name="f">剣を振る角度 詳細はアニメーターを参照</param>
    public void LeftHangingAttack(float f)
    {
        _animator.SetFloat(AnimatorHash.ID_AttackDirectionL, f);
        _animator.SetTrigger(AnimatorHash.ID_LeftHangingAttack);
    }

    /// <summary>
    /// 右手のぶら下がり時の攻撃アニメーション
    /// </summary>
    /// <param name="f">剣を振る角度 詳細はアニメーターを参照</param>
    public void RightHangingAttack(float f)
    {
        _animator.SetFloat(AnimatorHash.ID_AttackDirectionR, f);
        _animator.SetTrigger(AnimatorHash.ID_RightHangingAttack);
    }

    #endregion

    #region Hands

    /// <summary>
    /// プレイヤーの右手をパーの状態にアニメーションする
    /// </summary>
    public void RightHandOpen()
    {
        HandLayerTrue(Hand.Right);
        _animator.SetBool(AnimatorHash.ID_RightHandOpen, true);
    }

    /// <summary>
    /// プレイヤーの右手をグーの状態にアニメーションする
    /// </summary>
    public void RightHandClose()
    {
        HandLayerTrue(Hand.Right);
        _animator.SetBool(AnimatorHash.ID_RightHandOpen, false);
    }

    /// <summary>
    /// プレイヤーの左手をパーの状態にアニメーションする
    /// </summary>
    public void LeftHandOpen()
    {
        HandLayerTrue(Hand.Left);
        _animator.SetBool(AnimatorHash.ID_LeftHandOpen, true);
    }

    /// <summary>
    /// プレイヤーの左手をグーの状態にアニメーションする
    /// </summary>
    public void LeftHandClose()
    {
        HandLayerTrue(Hand.Left);
        _animator.SetBool(AnimatorHash.ID_LeftHandOpen, false);
    }

    public enum Hand
    {
        Right,
        Left,
        Both
    }

    /// <summary>
    /// 両手のアニメーションを上書きする
    /// </summary>
    public void HandLayerTrue()
    {
        _animator.SetLayerWeight(_layerIndex_RightHand, 1);
        _animator.SetLayerWeight(_layerIndex_LeftHand, 1);
    }

    /// <summary>
    /// 引数の手のアニメーションを上書きする
    /// </summary>
    /// <param name="hand"></param>
    public void HandLayerTrue(Hand hand)
    {
        switch (hand)
        {
            case Hand.Right:
                _animator.SetLayerWeight(_layerIndex_RightHand, 1);
                break;
            case Hand.Left:
                _animator.SetLayerWeight(_layerIndex_LeftHand, 1);
                break;
            case Hand.Both:
                _animator.SetLayerWeight(_layerIndex_RightHand, 1);
                _animator.SetLayerWeight(_layerIndex_LeftHand, 1);
                break;
        }
    }

    /// <summary>
    /// 両手のアニメーションの上書きを削除する
    /// </summary>
    public void HandLayerFalse()
    {
        _animator.SetLayerWeight(_layerIndex_RightHand, 0);
        _animator.SetLayerWeight(_layerIndex_LeftHand, 0);
    }

    /// <summary>
    /// 引数のアニメーションの上書きを削除する
    /// </summary>
    /// <param name="hand"></param>
    public void HandLayerFalse(Hand hand)
    {
        switch (hand)
        {
            case Hand.Right:
                _animator.SetLayerWeight(_layerIndex_RightHand, 0);
                break;
            case Hand.Left:
                _animator.SetLayerWeight(_layerIndex_LeftHand, 0);
                break;
            case Hand.Both:
                _animator.SetLayerWeight(_layerIndex_RightHand, 0);
                _animator.SetLayerWeight(_layerIndex_LeftHand, 0);
                break;
        }
    }

    #endregion

    #region ぶら下がりのアニメーション

    public void RightHandHangingTrue()
    {
        _animator.SetBool(AnimatorHash.ID_RightHandHanging,true);
    }
    public void RightHandHangingFalse()
    {
        _animator.SetBool(AnimatorHash.ID_RightHandHanging,false);
    }

    public void LeftHandHangingTrue()
    {
        _animator.SetBool(AnimatorHash.ID_LeftHandHanging,true);
    }
    public void LeftHandHangingFalse()
    {
        _animator.SetBool(AnimatorHash.ID_LeftHandHanging,false);
    }

    #endregion
    /// <summary>
    /// すべてのアニメーショントリガーをリセットする
    /// </summary>
    public void ResetAllAnimationTriggers()
    {
        _animator.ResetTrigger(AnimatorHash.ID_Jump);
        _animator.ResetTrigger(AnimatorHash.ID_Landing);
        _animator.ResetTrigger(AnimatorHash.ID_DodgeInGround);
        _animator.ResetTrigger(AnimatorHash.ID_AttackL);
        _animator.ResetTrigger(AnimatorHash.ID_AttackR);
        _animator.ResetTrigger(AnimatorHash.ID_ThrowSwordR);
        _animator.ResetTrigger(AnimatorHash.ID_ThrowSwordL);
        _animator.ResetTrigger(AnimatorHash.ID_LeftHangingAttack);
        _animator.ResetTrigger(AnimatorHash.ID_RightHangingAttack);
    }
}
