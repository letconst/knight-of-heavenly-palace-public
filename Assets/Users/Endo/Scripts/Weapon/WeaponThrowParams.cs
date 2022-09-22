using UnityEngine;

public struct WeaponThrowParams
{
    /// <summary>
    /// 投げた手
    /// </summary>
    public PlayerInputEvent.PlayerHand Hand { get; }

    /// <summary>
    /// カーソル先にあったオブジェクトへのヒット情報
    /// </summary>
    public RaycastHit Hit { get; }

    /// <summary>
    /// 親オブジェクト
    /// </summary>
    public Transform Parent { get; }

    /// <summary>
    /// 投擲速度
    /// </summary>
    public float Speed { get; }

    /// <summary>
    /// 追従対象
    /// </summary>
    public Transform Target { get; }

    public WeaponThrowParams(PlayerInputEvent.PlayerHand hand, in RaycastHit hit, Transform parent, Transform target,
                             float speed)
    {
        Hand   = hand;
        Hit    = hit;
        Parent = parent;
        Target = target;
        Speed  = speed;
    }
}
