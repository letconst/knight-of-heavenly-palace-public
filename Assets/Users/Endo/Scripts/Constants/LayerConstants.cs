using UnityEngine;

/// <summary>
/// レイヤーマスクのビット値を定義したクラス
/// </summary>
public static class LayerConstants
{
    public static readonly int Default = 1 << LayerMask.NameToLayer("Default");

    public static readonly int TransparentFX = 1 << LayerMask.NameToLayer("TransparentFX");

    public static readonly int IgnoreRaycast = 1 << LayerMask.NameToLayer("Ignore Raycast");

    public static readonly int Water = 1 << LayerMask.NameToLayer("Water");

    public static readonly int UI = 1 << LayerMask.NameToLayer("UI");

    public static readonly int Ground = 1 << LayerMask.NameToLayer("Ground");

    public static readonly int Player = 1 << LayerMask.NameToLayer("Player");

    public static readonly int Enemy = 1 << LayerMask.NameToLayer("Enemy");

    public static readonly int Weapon = 1 << LayerMask.NameToLayer("Weapon");

    public static readonly int CullingMap = 1 << LayerMask.NameToLayer("CullingMap");

    public static readonly int MostFrontParticles = 1 << LayerMask.NameToLayer("MostFrontParticles");
}
