using UnityEngine;

public partial class JoyConAngleCheck
{
    private Vector2 _PositoinResetLeftVector2ForSimulate  = Vector2.zero;
    private Vector2 _PositoinResetRightVector2ForSimulate = Vector2.zero;

    private static (int frameCount, Position position) _leftPositionCacheForSimulate;
    private static (int frameCount, Position position) _rightPositionCacheForSimulate;

    /// <summary>
    /// <inheritdoc cref="GetJoyConAnglePosition"/> (仮想用)
    /// </summary>
    /// <param name="_joyStick"><inheritdoc cref="GetJoyConAnglePosition" path="/param[@name='_joyStick']"/></param>
    /// <returns></returns>
    public Position GetJoyConAnglePositionForSimulate(nn.hid.NpadJoyDeviceType _joyStick)
    {
        //初期化
        var _position = Position.None;

        switch (_joyStick)
        {
            case nn.hid.NpadJoyDeviceType.Left:
                LeftJoyConAnglePosition(JoyConToScreenPointer.Instance.LeftJoyConSingleCircleVector2ForSimulate,
                                        _PositoinResetLeftVector2ForSimulate, ref _leftPositionCacheForSimulate);

                _position = _leftPositionCacheForSimulate.position;

                break;

            case nn.hid.NpadJoyDeviceType.Right:
                RightJoyConAnglePosition(JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2ForSimulate,
                                         _PositoinResetRightVector2ForSimulate, ref _rightPositionCacheForSimulate);

                _position = _rightPositionCacheForSimulate.position;

                break;
        }

        return _position;
    }

    public void PositionResetForSimulate(nn.hid.NpadJoyDeviceType _joyStick)
    {
        switch (_joyStick)
        {
            case nn.hid.NpadJoyDeviceType.Left:
                _PositoinResetLeftVector2ForSimulate = -JoyConToScreenPointer.Instance.LeftJoyConSingleCircleVector2ForSimulate;

                break;

            case nn.hid.NpadJoyDeviceType.Right:
                _PositoinResetRightVector2ForSimulate = -JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2ForSimulate;

                break;
        }
    }
}
