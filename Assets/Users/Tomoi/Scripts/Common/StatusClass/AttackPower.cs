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
    
    /// <summary>正数のチェック</summary>
    /// <param name="_value"></param>
    private bool CheckPositiveNumber(int _value)
    {
        return 0 <= _value;
    }
}