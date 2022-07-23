using System.Collections.Generic;
public class ComboBonusChecker
{
    public static ComboBonusData Check(JoyConAngleCheck.Position[] _ComboStock)
    {
        if (_ComboStock.Length == 4)
        {
            ComboBonusDataBase _Data = new ComboBonusDataBase();
            foreach (ComboBonusData _comboBonuData in _Data.ComboBonusPositionList)
            {
                if (_comboBonuData.Input1 == _ComboStock[0])
                {
                    if (_comboBonuData.Input2 == _ComboStock[1])
                    {
                        if (_comboBonuData.Input3 == _ComboStock[2])
                        {
                            if (_comboBonuData.Input4 == _ComboStock[3])
                            {
                                return _comboBonuData;
                            }
                        }
                    }                    
                }
            }
        }
        return null;
    }
}