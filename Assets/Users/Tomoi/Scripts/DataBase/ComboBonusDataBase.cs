using System.Collections.Generic;
public class ComboBonusDataBase
{
    public List<ComboBonusData> ComboBonusPositionList 
        = new List<ComboBonusData>()
        {new ComboBonusData()
            {
                Input1 = JoyConAngleCheck.Position.Up,
                Input2 = JoyConAngleCheck.Position.Down,
                Input3 = JoyConAngleCheck.Position.Up,
                Input4 = JoyConAngleCheck.Position.Down,
            }
        };
}
