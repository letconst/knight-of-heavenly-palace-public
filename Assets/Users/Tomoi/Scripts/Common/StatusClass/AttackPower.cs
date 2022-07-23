public class AttackPower
{
    public int Value { get;private set; }
    public int MaxValue { get;private set; }

    
    /// <summary>初期化</summary>
    /// <param name="initValue">HPの初期値</param>
    /// <param name="initMaxValue">HPの最大値</param>
    public AttackPower(int initValue,int initMaxValue = int.MaxValue)
    {
        Value = 0 <= initValue ? initValue : 0;
        MaxValue = initMaxValue;
    }
    /// <summary>
    /// 攻撃の倍率を上昇させる計算をする
    /// 小数点切り捨て
    /// </summary>
    /// <param name="attackMagnification"></param>
    public void Magnification(AttackMagnification attackMagnification)
    {
        Value = (int)(Value * attackMagnification.Value);
    }
}