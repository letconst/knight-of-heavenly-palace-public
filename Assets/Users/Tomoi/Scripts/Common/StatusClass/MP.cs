public class MP
{
    public int Value { get;private set; }
    public int MaxValue { get;private set; }

    /// <summary>初期化</summary>
    /// <param name="initValue">MPの初期値</param>
    /// <param name="initMaxValue">MPの最大値</param>
    public MP(int initValue,int initMaxValue = int.MaxValue)
    {
        Value = 0 <= initValue ? initValue : 0;
        MaxValue = initMaxValue;
    }
    /// <summary>MPを使用したときの処理
    /// 正しく消費されればtrue引数が正しくないか引数分消費できなければfalseを返す
    /// </summary>
    /// <param name="_mp">MPクラスのValueを参照</param>
    public bool Consumption(MP _mp)
    {
        //引数が正しい値かチェック
        if (CheckPositiveNumber(_mp.Value))
        {
            //引数を引くことができるかチェック
            if (_mp.Value <= Value)
            {
                //引数分消費してtrueを返す
                Value -= _mp.Value;
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
    /// <param name="_mp">MPクラスのValueを参照</param>
    public void Recovery(MP _mp)
    {
        if (CheckPositiveNumber(_mp.Value))
        {
            Value +=_mp.Value;
            if (MaxValue <= Value)
            {
                Value = MaxValue;
            }
        }
    }

    /// <summary>正数のチェック</summary>
    /// <param name="_value"></param>
    private bool CheckPositiveNumber(int _value)
    {
        return 0 <= _value;
    }
}