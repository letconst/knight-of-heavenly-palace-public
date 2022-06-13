using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "マスターデータ/魔物")]
public sealed class MasterEnemy : MasterDataBaseWithId
{
    /// <summary>
    /// 魔物の体型
    /// </summary>
    public enum BodySizeType
    {
        /// <summary>小型</summary>
        Small,

        /// <summary>大型</summary>
        Big,
    }

    [SerializeField, Header(MasterDataModelConstants.Enemy.NameLabel)]
    private string enemyName;

    /// <summary>名前</summary>
    public string Name => enemyName;

    [SerializeField, Header(MasterDataModelConstants.Enemy.BodySizeLabel)]
    private BodySizeType bodySize;

    /// <summary>体型</summary>
    public BodySizeType BodySize => bodySize;

    [SerializeField, Header(MasterDataModelConstants.Enemy.MaxHitPointLabel)]
    private int maxHitPoint;

    /// <summary>最大体力</summary>
    public int MaxHitPoint => maxHitPoint;

    [SerializeField, Header(MasterDataModelConstants.Enemy.BaseAttackPowerLabel)]
    private int baseAttackPower;

    /// <summary>ベースとなる攻撃力 (ダメージ)</summary>
    public int BaseAttackPower => baseAttackPower;
}
