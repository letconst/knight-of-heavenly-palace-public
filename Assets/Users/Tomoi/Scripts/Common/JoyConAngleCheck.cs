using System;
using UnityEngine;
public partial class JoyConAngleCheck:SingletonMonoBehaviour<JoyConAngleCheck>
{
    public enum Position{
        Right,
        RightUp,
        Up,
        LeftUp,
        Left,
        LeftDown,
        Down,
        RightDown,
        None
    }

    private Vector2 _PositoinResetLeftVector2 = Vector2.zero;
    private Vector2 _PositoinResetRightVector2 = Vector2.zero;
    private static (int frameCount, Position position) _leftPositionCache;
    private static (int frameCount, Position position) _rightPositionCache;

    /// <summary>
    /// Joy-Conの画面上の座標から今どの位置にいるのかを返す関数
    /// </summary>
    /// <param name="_joyStick">Joy-Conを持っている手のenumを引数で渡す</param>
    /// <returns></returns>
    public Position GetJoyConAnglePosition(nn.hid.NpadJoyDeviceType _joyStick)
    {
        //初期化
        Position _position = Position.None;
        switch (_joyStick)
        {
            case nn.hid.NpadJoyDeviceType.Left:
                LeftJoyConAnglePosition(JoyConToScreenPointer.Instance.LeftJoyConSingleCircleVector2,
                                        _PositoinResetLeftVector2, ref _leftPositionCache);
                _position = _leftPositionCache.position;
                break;
            case nn.hid.NpadJoyDeviceType.Right:
                RightJoyConAnglePosition(JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2,
                                         _PositoinResetRightVector2, ref _rightPositionCache);
                _position = _rightPositionCache.position;
                break;
        }
        return _position;
    }

    private void RightJoyConAnglePosition(in Vector2 joyConSingleCircle, in Vector2 positionReset, ref (int frameCount, Position position) posCache)
    {
        if (posCache.frameCount != Time.frameCount)
        {
            //右
            //i = 0が右に存在している時 そこから反時計回りに+1ずつされる
            for (int i = 0; i < 8; i++)
            {
                /*if (i == 0)
                {
                    if (!(Mathf.Repeat(GenerateAngle(i - 1), 360) >
                          angle(JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2 + _PositoinResetRightVector2) &&
                          angle(JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2 + _PositoinResetRightVector2) >
                          Mathf.Repeat(GenerateAngle(i), 360)))
                    {
                        _rightPositionCache.position = (Position)i;
                        _rightPositionCache.frameCount = Time.frameCount;

                        //TODO:コンボの処理
                        continue;
                    }
                }
                else*/
                {
                    if (Mathf.Repeat(GenerateAngle(i - 1), 360) <
                        angle(joyConSingleCircle + positionReset) &&
                        angle(joyConSingleCircle + positionReset) <
                        Mathf.Repeat(GenerateAngle(i), 360))
                    {
                        posCache.position = (Position)i;
                        posCache.frameCount = Time.frameCount;

                        //TODO:コンボの処理
                        continue;
                    }
                }
            }
        }

    }
    private void LeftJoyConAnglePosition(in Vector2 joyConSingleCircle, in Vector2 positionReset, ref (int frameCount, Position position) posCache)
    {
        if (posCache.frameCount != Time.frameCount)
        {
            //左
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    if (!(Mathf.Repeat(GenerateAngle(i - 1), 360) >
                          angle(joyConSingleCircle + positionReset) &&
                          angle(joyConSingleCircle + positionReset) >
                          Mathf.Repeat(GenerateAngle(i), 360)))
                    {
                        posCache.position = (Position)i;
                        posCache.frameCount = Time.frameCount;
                        //TODO:コンボの処理
                        continue;
                    }
                }
                else
                {
                    if (Mathf.Repeat(GenerateAngle(i - 1), 360) <
                        angle(joyConSingleCircle + positionReset) &&
                        angle(joyConSingleCircle + positionReset) <
                        Mathf.Repeat(GenerateAngle(i), 360))
                    {
                        posCache.position = (Position)i;
                        posCache.frameCount = Time.frameCount;
                        //TODO:コンボの処理
                        continue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 引数iから条件に合う角度を返す
    /// </summary>
    /// <param name="i">何個目の三角形か</param>
    /// <returns></returns>
    private static float GenerateAngle(int i)
    {
        return (float)(i * 45 + 22.5);
    }
    //引数の座標が中心からどの角度にあるかを計算
    private static float angle(Vector2 _Pointer)
    {
        float angle = Mathf.Atan2(_Pointer.y, _Pointer.x) * Mathf.Rad2Deg;
        if (angle <= 0)
        {
            angle += 360;
        }
        return angle;
    }

    public void PositoinReset(nn.hid.NpadJoyDeviceType _joyStick)
    {
        switch (_joyStick)
        {
            case nn.hid.NpadJoyDeviceType.Left:
                _PositoinResetLeftVector2 = -JoyConToScreenPointer.Instance.LeftJoyConSingleCircleVector2;
                break;
            case nn.hid.NpadJoyDeviceType.Right:
                _PositoinResetRightVector2 = -JoyConToScreenPointer.Instance.RightJoyConSingleCircleVector2;
                break;
        }
    }
}
