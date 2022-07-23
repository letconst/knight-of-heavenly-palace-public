using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DebugPlayerAttack : MonoBehaviour
{
    [SerializeField, Range(.1f, 10f)]
    private float judgeAccel;

    [SerializeField]
    private Collider attackCol;

    private void Start()
    {
        attackCol.enabled = false;

        this.UpdateAsObservable()
            .Where(_ => HasJudgeAcceleration(nn.hid.NpadJoyDeviceType.Left) ||
                        HasJudgeAcceleration(nn.hid.NpadJoyDeviceType.Right))
            .Subscribe(_ => OnAttack())
            .AddTo(this);

        attackCol.OnTriggerEnterAsObservable()
                 .Where(other => other.GetComponent<IDamageable>() != null)
                 .Subscribe(OnHit)
                 .AddTo(this);
    }

    private async void OnAttack()
    {
        attackCol.enabled = true;

        await UniTask.Delay(System.TimeSpan.FromSeconds(1));

        attackCol.enabled = false;
    }

    private void OnHit(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>()!;

        damageable.OnDamage(new AttackPower(1, 1));
    }

    private bool HasJudgeAcceleration(nn.hid.NpadJoyDeviceType joyDeviceType)
    {
        nn.util.Float3 accel = joyDeviceType switch
        {
            nn.hid.NpadJoyDeviceType.Left  => SwitchInputManager.Instance.LeftAcceleration,
            nn.hid.NpadJoyDeviceType.Right => SwitchInputManager.Instance.RightAcceleration
        };

        bool result = joyDeviceType switch
        {
            nn.hid.NpadJoyDeviceType.Left  => judgeAccel <= GetFloat3Magnitude(accel),
            nn.hid.NpadJoyDeviceType.Right => judgeAccel <= GetFloat3Magnitude(accel)
        };

        return result;
    }

    private float GetFloat3Magnitude(nn.util.Float3 float3)
    {
        float result = Mathf.Sqrt(float3.x * float3.x + float3.y * float3.y + float3.z * float3.z);

        return result;
    }
}
