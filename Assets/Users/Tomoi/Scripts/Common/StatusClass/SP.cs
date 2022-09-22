public class SP
{
    public float Value { get;private set; }
    public float MaxValue { get;private set; }

    /// <summary>初期化</summary>
    /// <param name="initValue">SPの初期値</param>
    /// <param name="initMaxValue">SPの最大値</param>
    public SP(float initValue,float initMaxValue = float.MaxValue)
    {
        Value = 0 <= initValue ? initValue : 0;
        MaxValue = initMaxValue;
    }
    /// <summary>SPを使用したときの処理
    /// 正しく消費されればtrue引数が正しくないか引数分消費できなければfalseを返す
    /// </summary>
    /// <param name="_sp">SPクラスのValueを参照</param>
    public bool Consumption(SP _sp)
    {
        //引数が正しい値かチェック
        if (CheckPositiveNumber(_sp.Value))
        {
            //引数を引くことができるかチェック
            if (_sp.Value <= Value)
            {
                //引数分消費してtrueを返す
                Value -= _sp.Value;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    /// <summary>回復の処理</summary>
    /// <param name="_sp">SPクラスのValueを参照</param>
    public void Recovery(SP _sp)
    {
        if (CheckPositiveNumber(_sp.Value))
        {
            Value +=_sp.Value;
            if (MaxValue <= Value)
            {
                Value = MaxValue;
            }
        }
    }

    /// <summary>正数のチェック</summary>
    /// <param name="_value"></param>
    private bool CheckPositiveNumber(float _value)
    {
        return 0 <= _value;
    }
}