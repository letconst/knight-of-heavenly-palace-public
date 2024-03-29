using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public sealed partial class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private float swingAccelThreshold;

    private SwitchInputManager _inputManager;

    private void Start()
    {
        _inputManager = SwitchInputManager.Instance;

        UniTask.Create(async () =>
        {
            await PlayerStatus.WaitForLoadMasterData();

            PlayerInputEventEmitter.Instance.Broker.Receive<PlayerEvent.Input.OnAttackWeapon>()
                                   .ThrottleFirst(
                                       System.TimeSpan.FromSeconds(
                                           PlayerStatus.playerMasterData.AttackInterval)) // 一定間隔で攻撃させる (連打防止)
                                   .Subscribe(OnAttackInput)
                                   .AddTo(this);
        });

        StartDebug();

        attackCol.OnTriggerEnterAsObservable()
                 .Where(other => !other.isTrigger)
                 .Subscribe(collider =>
                 {
                     var target = collider.GetComponentInParent<EnemyBase>();

                     if (target == null) return;

                     SoundManager.Instance.PlaySe(SoundDef.SwordAttack);
                     EnemyManeger.Instance.Broker.Publish(OnAttackEnemy.GetEvent(target.GetInstanceID(), new AttackPower(10)));
                 })
                 .AddTo(this);
    }

    private void Update()
    {
        if (swingAccelThreshold <= GetFloat3Magnitude(_inputManager.LeftAcceleration) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL))
        {
            JudgeAttackDirection(nn.hid.NpadJoyDeviceType.Left);
        }

        if (swingAccelThreshold <= GetFloat3Magnitude(_inputManager.RightAcceleration) &&
            !PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR))
        {
            JudgeAttackDirection(nn.hid.NpadJoyDeviceType.Right);
        }
    }

    private async void OnAttackInput(PlayerEvent.Input.OnAttackWeapon data)
    {
        // 張り付き状態なら、最寄りの攻撃対象に攻撃
        if (PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingL) ||
            PlayerStateManager.HasFlag(PlayerStatus.PlayerState.HangingR))
        {
            Collider[] inRangeEnemies = new Collider[8];

            Physics.OverlapSphereNonAlloc(transform.position, PlayerStatus.playerMasterData.AttackTargetFindRadius,
                                          inRangeEnemies, LayerConstants.Enemy);

            EnemyBase nearestEnemy = null;
            float     minDistance  = Mathf.Infinity;

            // 最寄りの敵を取得
            // TODO: 張り付き中のオブジェクトを保持して参照させたい
            foreach (Collider enemy in inRangeEnemies)
            {
                if (!enemy) continue;

                float distanceFromSelf = (transform.position - enemy.transform.position).sqrMagnitude;

                if (distanceFromSelf < minDistance)
                {
                    minDistance  = distanceFromSelf;
                    nearestEnemy = enemy.GetComponentInParent<EnemyBase>();
                }
            }

            if (!nearestEnemy) return;

            // モーション再生
            switch (data.ActionInfo.actHand)
            {
                case PlayerInputEvent.PlayerHand.Right:
                    PlayerAnimationManager.Instance.RightHangingAttack((float) data.AttackDirection);

                    break;

                case PlayerInputEvent.PlayerHand.Left:
                    PlayerAnimationManager.Instance.LeftHangingAttack((float) data.AttackDirection);

                    break;
            }
            //PlayerMotionController.Instance.PlayAttackTriggerMotion(data.ActionInfo.actHand, data.AttackDirection);

            // TODO: タイミング調整はモーションイベントで行いたい
            await UniTask.Delay(System.TimeSpan.FromSeconds(.5f));

            SoundManager.Instance.PlaySe(SoundDef.SwordAttack);

            // ダメージ付与
            // TODO: ダメージ量はマスターデータから引っ張ってくる
            // damageable.OnDamage(new AttackPower(10));
            EnemyManeger.Instance.Broker.Publish(OnAttackEnemy.GetEvent(nearestEnemy.GetInstanceID(), new AttackPower(10)));
        }
        // 通常攻撃
        else
        {
            // TODO: 剣のイベント化
            switch (data.ActionInfo.actHand)
            {
                case PlayerInputEvent.PlayerHand.Right:
                    PlayerAnimationManager.Instance.AttackR((float) data.AttackDirection);

                    break;

                case PlayerInputEvent.PlayerHand.Left:
                    PlayerAnimationManager.Instance.AttackL((float) data.AttackDirection);

                    break;
            }
            //PlayerMotionController.Instance.PlayAttackTriggerMotion(data.ActionInfo.actHand, data.AttackDirection);

            await UniTask.Delay(System.TimeSpan.FromSeconds(.5f));

            attackCol.enabled = true;

            await UniTask.Delay(System.TimeSpan.FromSeconds(1));

            attackCol.enabled = false;
        }
    }

    private async void JudgeAttackDirection(nn.hid.NpadJoyDeviceType joyType)
    {
        JoyConAngleCheck.Instance.PositionResetForSimulate(joyType);
        JoyConToScreenPointer.Instance.AngleResetForSimulate();

        await UniTask.DelayFrame(3);

        // 攻撃方向取得
        JoyConAngleCheck.Position attackDir = JoyConAngleCheck.Instance.GetJoyConAnglePositionForSimulate(joyType);

        var actionInfo = new PlayerActionInfo
        {
            actHand = (PlayerInputEvent.PlayerHand) joyType
        };

        // 攻撃イベント発行
        PlayerInputEventEmitter.Instance.Broker.Publish(PlayerEvent.Input.OnAttackWeapon.GetEvent(actionInfo, attackDir));
    }

    private float GetFloat3Magnitude(nn.util.Float3 float3)
    {
        float result = Mathf.Sqrt(float3.x * float3.x + float3.y * float3.y + float3.z * float3.z);

        return result;
    }
}
