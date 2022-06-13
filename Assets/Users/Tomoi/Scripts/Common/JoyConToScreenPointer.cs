using System;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Joy-ConのAngle.z,xから任意の角度で1になる2次関数を使用し、擬似的に平面上のカーソルを作成
/// Joy-ConのAngle.x,y,zからQuaternionを生成しJoy-Conの角度を変えるとカーソルの座標が変わる
/// カーソルの座標にCanvasの縦横のピクセル数をかけるスクリプト
/// </summary>
public class JoyConToScreenPointer : SingletonMonoBehaviour<JoyConToScreenPointer>
{
    //DisplaySize
    [SerializeField] private float _ScreenWidth;
    [SerializeField] private float _ScreenHeight;

    //CursorCenter
    [SerializeField] private float _CursorCenterX;
    [SerializeField] private float _CursorCenterY;

    //MaxAngle
    [SerializeField, Range(0, 20)] private float _DisplayWidthAngleX = 10;
    [SerializeField, Range(0, 20)] private float _DisplayWidthAngleY = 10;


    [SerializeField,Header("ジャイロドリフト修正")] private bool _GyroDriftCorrection = false;
    /// <summary>画面の中央への補正値 </summary>
    private Vector3 _CenterCorrectionValue;

    /// <summary>画面の中央への補正値 </summary>
    private Vector3 _LeftAngleOffset = default;
    private Vector3 _RightAngleOffset = default;
    
    private float[] _LeftAngularVelocityOffset = new float[3]{0,0,0};
    private float[] _RightAngularVelocityOffset = new float[3]{0,0,0};

    /// <summary>左Joy-Conの角度から算出されたCanvas上の座標</summary>
    public Vector2 LeftJoyConScreenVector2 { get; private set; }
    /// <summary>右Joy-Conの角度から算出されたCanvas上の座標</summary>
    public Vector2 RightJoyConScreenVector2 { get; private set; }
    /// <summary>左Joy-Conの角度から算出された単円上の座標</summary>
    public Vector2 LeftJoyConSingleCircleVector2 { get; private set; }
    /// <summary>右Joy-Conの角度から算出された単円上の座標</summary>
    public Vector2 RightJoyConSingleCircleVector2 { get; private set; }

    private const float GyroDriftValueAngle = 0.995f;
    private const float GyroDriftValueAngleVelocity = 0.005f;

    void Start()
    {
        _ScreenWidth = Screen.width;
        _ScreenHeight = Screen.height;
        _CursorCenterX = _ScreenWidth * 0.5f;
        _CursorCenterY = _ScreenHeight * 0.5f;
    }

    private float ResetTime = 0;

    void Update()
    {
        ResetTime += Time.deltaTime;
        
        if (_GyroDriftCorrection)//ジャイロドリフト修正版
        {
            LeftJoyConScreenVector2 = RotationMatrixX(new Vector2(
                    -LeftAngleZ() * GyroDriftValueAngle + LeftAngularVelocityZ() * GyroDriftValueAngleVelocity,
                    LeftAngleX() * GyroDriftValueAngle + LeftAngularVelocityX() * GyroDriftValueAngleVelocity),
                -LeftAngleY() * 360);
        }
        else//修正なし
        {
            LeftJoyConScreenVector2 = RotationMatrixX(new Vector2(
                    -LeftAngleZ(),
                    LeftAngleX()), 
                -LeftAngleY() * 360);
        }
        
        if (_GyroDriftCorrection)//ジャイロドリフト修正版
        {
            RightJoyConScreenVector2 = RotationMatrixX(new Vector2(
                    -RightAngleZ() * GyroDriftValueAngle + RightAngularVelocityZ() * GyroDriftValueAngleVelocity,
                    RightAngleX() * GyroDriftValueAngle + RightAngularVelocityX() * GyroDriftValueAngleVelocity),
                -RightAngleY() * 360);
        }
        else//修正なし
        {
            RightJoyConScreenVector2 = RotationMatrixX(new Vector2(
                    -RightAngleZ(),
                    RightAngleX()), 
                -RightAngleY() * 360);
        }

        LeftJoyConSingleCircleVector2 = LeftJoyConScreenVector2;
        RightJoyConSingleCircleVector2 = RightJoyConScreenVector2;
        LeftJoyConScreenVector2 = PointerOnOffScreenToScreen(ScaleToCanvas(LeftJoyConScreenVector2));
        RightJoyConScreenVector2 = PointerOnOffScreenToScreen(ScaleToCanvas(RightJoyConScreenVector2));
    }

    #region JoyConInput
    
    /*左Joy-Con*/
    /*角速度*/
    private float LeftAngularVelocityY() {return SwitchInputManager.Instance.LeftAngularVelocity.y + _LeftAngularVelocityOffset[1];}
    private float LeftAngularVelocityX() {return SwitchInputManager.Instance.LeftAngularVelocity.x + _LeftAngularVelocityOffset[0];}
    private float LeftAngularVelocityZ() {return SwitchInputManager.Instance.LeftAngularVelocity.z + _LeftAngularVelocityOffset[2];}
    
    /*加速度*/
    private float LeftAccelerationX(){return SwitchInputManager.Instance.LeftAcceleration.x;}
    private float LeftAccelerationY(){return SwitchInputManager.Instance.LeftAcceleration.y;}
    private float LeftAccelerationZ(){return SwitchInputManager.Instance.LeftAcceleration.z;}
    
    /*角度*/
    private float LeftAngleX(){return SwitchInputManager.Instance.LeftAngle.x - _LeftAngleOffset.x;}
    private float LeftAngleY(){return SwitchInputManager.Instance.LeftAngle.y - _LeftAngleOffset.y;}
    private float LeftAngleZ(){return SwitchInputManager.Instance.LeftAngle.z - _LeftAngleOffset.z;}

    /*右Joy-Con*/
    /*角速度*/
    private float RightAngularVelocityX() {return SwitchInputManager.Instance.RightAngularVelocity.x + _RightAngularVelocityOffset[0];}
    private float RightAngularVelocityY() {return SwitchInputManager.Instance.RightAngularVelocity.y + _RightAngularVelocityOffset[1];}
    private float RightAngularVelocityZ() {return SwitchInputManager.Instance.RightAngularVelocity.z + _RightAngularVelocityOffset[2];}
    
    /*加速度*/
    private float RightAccelerationX(){return SwitchInputManager.Instance.RightAcceleration.x;}
    private float RightAccelerationY(){return SwitchInputManager.Instance.RightAcceleration.y;}
    private float RightAccelerationZ(){return SwitchInputManager.Instance.RightAcceleration.z;}
    
    /*角度*/
    private float RightAngleX(){return SwitchInputManager.Instance.RightAngle.x - _RightAngleOffset.x;}
    private float RightAngleY(){return SwitchInputManager.Instance.RightAngle.y - _RightAngleOffset.y;}
    private float RightAngleZ(){return SwitchInputManager.Instance.RightAngle.z - _RightAngleOffset.z;}

    #endregion

    /// <summary>
    /// 画面外のポインタを画面内へ修正
    /// </summary>
    /// <returns></returns>
    private Vector2 PointerOnOffScreenToScreen(Vector2 _vector2)
    {
        //x
        if (_ScreenWidth <= _vector2.x)
        {
            _vector2.x = _ScreenWidth;
        }else if (_vector2.x <= 0)
        {
            _vector2.x = 0;
        }
        
        //y
        if (_ScreenHeight <= _vector2.y)
        {
            _vector2.y = _ScreenHeight;
        }else if (_vector2.y <= 0)
        {
            _vector2.y = 0;
        }
        
        return _vector2;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_v2">回転元のVector2</param>
    /// <param name="_angle">回転する角度</param>
    /// <returns></returns>
    private Vector2 RotationMatrixX(Vector2 _v2,float _angle)
    {
        //ラジアンに変換
        _angle *= Mathf.Deg2Rad;
        
        Vector2 _rv;//return vector2
        _rv.x = _v2.x * Mathf.Cos(_angle) + _v2.y * -Mathf.Sin(_angle);
        _rv.y = _v2.x * Mathf.Sin(_angle) + _v2.y * Mathf.Cos(_angle);
        return _rv;
    }

    private Vector2 ScaleToCanvas(Vector2 _vector2)
    {
        /*拡大*/
        _vector2.x *= _DisplayWidthAngleX;
        _vector2.y *= _DisplayWidthAngleY;
        /*画面サイズに拡大*/
        _vector2.x *= _ScreenWidth;
        _vector2.y *= _ScreenHeight;
            
        /*画面の中心に移動*/
        _vector2.x += _CursorCenterX;
        _vector2.y += _CursorCenterY;

        return _vector2;
    }
    public enum JoyCon
    {
        Left,
        Right,
        Both
    }

    public void AngleReset(JoyCon _JoyCon = JoyCon.Both)
    {
        if (1 <= ResetTime)
        {
            ResetTime = 0;
            if (_JoyCon == JoyCon.Left)
            {
                _LeftAngleOffset.x = SwitchInputManager.Instance.LeftAngle.x;
                _LeftAngleOffset.y = SwitchInputManager.Instance.LeftAngle.y;
                _LeftAngleOffset.z = SwitchInputManager.Instance.LeftAngle.z;

                _LeftAngularVelocityOffset[0] = -SwitchInputManager.Instance.LeftAngularVelocity.x;
                _LeftAngularVelocityOffset[1] = -SwitchInputManager.Instance.LeftAngularVelocity.y;
                _LeftAngularVelocityOffset[2] = -SwitchInputManager.Instance.LeftAngularVelocity.z;
            }
            else if (_JoyCon == JoyCon.Right)
            {
                _RightAngleOffset.x = SwitchInputManager.Instance.RightAngle.x;
                _RightAngleOffset.y = SwitchInputManager.Instance.RightAngle.y;
                _RightAngleOffset.z = SwitchInputManager.Instance.RightAngle.z;

                _RightAngularVelocityOffset[0] = -SwitchInputManager.Instance.RightAngularVelocity.x;
                _RightAngularVelocityOffset[1] = -SwitchInputManager.Instance.RightAngularVelocity.y;
                _RightAngularVelocityOffset[2] = -SwitchInputManager.Instance.RightAngularVelocity.z;
            }
            else if (_JoyCon == JoyCon.Both)
            {
                _LeftAngleOffset.x = SwitchInputManager.Instance.LeftAngle.x;
                _LeftAngleOffset.y = SwitchInputManager.Instance.LeftAngle.y;
                _LeftAngleOffset.z = SwitchInputManager.Instance.LeftAngle.z;

                _RightAngleOffset.x = SwitchInputManager.Instance.RightAngle.x;
                _RightAngleOffset.y = SwitchInputManager.Instance.RightAngle.y;
                _RightAngleOffset.z = SwitchInputManager.Instance.RightAngle.z;

                _LeftAngularVelocityOffset[0] = -SwitchInputManager.Instance.LeftAngularVelocity.x;
                _LeftAngularVelocityOffset[1] = -SwitchInputManager.Instance.LeftAngularVelocity.y;
                _LeftAngularVelocityOffset[2] = -SwitchInputManager.Instance.LeftAngularVelocity.z;

                _RightAngularVelocityOffset[0] = -SwitchInputManager.Instance.RightAngularVelocity.x;
                _RightAngularVelocityOffset[1] = -SwitchInputManager.Instance.RightAngularVelocity.y;
                _RightAngularVelocityOffset[2] = -SwitchInputManager.Instance.RightAngularVelocity.z;
            }
        }
    }
}
