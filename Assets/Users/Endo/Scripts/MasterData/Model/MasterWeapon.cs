using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "マスターデータ/武器")]
public sealed class MasterWeapon : MasterDataBaseWithId
{
    [SerializeField, Header(MasterDataModelConstants.Weapon.BaseAttackPowerLabel)]
    private int baseAttackPower;

    /// <summary>ベースとなる攻撃力 (ダメージ)</summary>
    public int BaseAttackPower => baseAttackPower;

    [SerializeField, Header(MasterDataModelConstants.Weapon.BaseComboRatioLabel)]
    private float baseComboRatio;

    /// <summary>ベースとなるコンボ倍率</summary>
    public float BaseComboRatio => baseComboRatio;

    [SerializeField, Header(MasterDataModelConstants.Weapon.BaseAttackSpeedLabel)]
    private float baseAttackSpeed;

    /// <summary>攻撃速度</summary>
    public float BaseAttackSpeed => baseAttackSpeed;

    [SerializeField, Header(MasterDataModelConstants.Weapon.BaseAttackRangeLabel)]
    private float baseAttackRange;

    /// <summary>攻撃範囲 (m)</summary>
    public float BaseAttackRange => baseAttackRange;

    // TODO: 投擲モードでも可能にするか一応選択を用意する
    [SerializeField, Header(MasterDataModelConstants.Weapon.CanThrowOnAttackModeLabel)]
    private bool canThrowOnAttackMode;

    /// <summary>攻撃モード時に投擲できるか</summary>
    public bool CanThrowOnAttackMode => canThrowOnAttackMode;

    [SerializeField, Header(MasterDataModelConstants.Weapon.IsMagicWeaponLabel)]
    private bool isMagicWeapon;

    /// <summary>魔法武器か</summary>
    public bool IsMagicWeapon => isMagicWeapon;

    [SerializeField, Header(MasterDataModelConstants.Weapon.MaxMagicPowerLabel)]
    private int maxMagicPower;

    /// <summary>最大魔力</summary>
    public int MaxMagicPower => maxMagicPower;
}
