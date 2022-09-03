using UniRx;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class PlayerMotionController : SingletonMonoBehaviour<PlayerMotionController>
{
    /*
    public enum MotionType
    {
        Idle,
        Run,
        Landing,
        DodgeInGround,
        Falling,
        SwordThrow,
    }

    // TODO: マスターデータなどに移す
    [SerializeField]
    private float landingMotionTimeThreshold;

    private Animator _selfAnimator;

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();

        MotionEventReceiver();
    }

    private void MotionEventReceiver()
    {
        IMessageBroker broker = PlayerInputEventEmitter.Instance.Broker;

        // 落下時
        broker.Receive<PlayerEvent.OnFalling>()
              .Subscribe(_ =>
              {
                  Instance.SetBool(MotionType.Falling, true);
              })
              .AddTo(this);

        // 着地時
        broker.Receive<PlayerEvent.OnLanding>()
              .Subscribe(data =>
              {
                  // 滞空時間が一定以上なら再生
                  if (data.InAirTime > Instance.landingMotionTimeThreshold)
                  {
                      Instance.PlayTriggerMotion(MotionType.Landing);
                      EffectManager.Instance.EffectPlay(EffectType.DustParticle, Instance.transform.position, Quaternion.identity);
                  }

                  Instance.SetBool(MotionType.Falling, false);
              })
              .AddTo(this);
    }

    /// <summary>
    /// Animatorのboolパラメータを設定する
    /// </summary>
    /// <param name="motionType">再生するモーション</param>
    /// <param name="value"></param>
    public void SetBool(MotionType motionType, bool value)
    {
        _selfAnimator.SetBool(motionType.ToString(), value);
    }

    public void SetFloat(MotionType motionType, float value)
    {
        _selfAnimator.SetFloat(motionType.ToString(), value);
    }

    public void SetFloat(string paramName, float value)
    {
        _selfAnimator.SetFloat(paramName, value);
    }

    public void SetInteger(string paramName, int value)
    {
        _selfAnimator.SetInteger(paramName, value);
    }

    /// <summary>
    /// Animatorのintパラメータを設定する
    /// </summary>
    /// <param name="motionType">再生するモーション</param>
    /// <param name="value"></param>
    public void SetInteger(MotionType motionType, int value)
    {
        _selfAnimator.SetInteger(motionType.ToString(), value);
    }

    /// <summary>
    /// モーションを再生する (trigger用)
    /// </summary>
    /// <param name="motionType"></param>
    public void PlayTriggerMotion(MotionType motionType)
    {
        _selfAnimator.SetTrigger(motionType.ToString());
    }

    public void PlayAttackTriggerMotion(PlayerInputEvent.PlayerHand hand, JoyConAngleCheck.Position attackDir)
    {
        string triggerName = hand switch
        {
            PlayerInputEvent.PlayerHand.Left  => "AttackL",
            PlayerInputEvent.PlayerHand.Right => "AttackR"
        };

        string floatName = hand switch
        {
            PlayerInputEvent.PlayerHand.Left  => "AttackDirectionL",
            PlayerInputEvent.PlayerHand.Right => "AttackDirectionR",
        };

        _selfAnimator.SetFloat(floatName, (float) attackDir);
        _selfAnimator.SetTrigger(triggerName);
    }*/
}
