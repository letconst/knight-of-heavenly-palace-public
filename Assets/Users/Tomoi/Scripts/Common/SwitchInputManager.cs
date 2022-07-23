using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nn.hid;
using nn.util;

public class SwitchInputManager : SingletonMonoBehaviour<SwitchInputManager>
{
    //Joy-Conの本体情報
    private NpadId npadId = NpadId.Invalid;
    private NpadStyle npadStyle = NpadStyle.Invalid;
    private NpadState npadState = new NpadState();

    //Joy-Conの軸センサーの情報
    private SixAxisSensorHandle[] handle = new SixAxisSensorHandle[2];
    private SixAxisSensorState state = new SixAxisSensorState();
    private int handleCount = 0;

    private nn.util.Float4 npadQuaternion = new nn.util.Float4();
    private Quaternion quaternion = new Quaternion();

    //Joy-Conのバイブレーション情報
    private VibrationValue vibrationValue = VibrationValue.Make();
    public int vibrationDeviceCount { get;private set; } = 0;
    private const int vibrationDeviceCountMax = 2;
    public VibrationDeviceHandle[] vibrationDeviceHandles { get;private set; }= new VibrationDeviceHandle[vibrationDeviceCountMax];
    private VibrationDeviceInfo[] vibrationDeviceInfos = new VibrationDeviceInfo[vibrationDeviceCountMax];


    public enum AnalogStick
    {
        Right,
        Left
    }

    public enum JoyConButton
    {
        A = 0x1 << 0,
        B = 0x1 << 1,
        X = 0x1 << 2,
        Y = 0x1 << 3,
        StickL = 0x1 << 4,
        StickR = 0x1 << 5,
        L = 0x1 << 6,
        R = 0x1 << 7,
        ZL = 0x1 << 8,
        ZR = 0x1 << 9,
        Plus = 0x1 << 10,
        Minus = 0x1 << 11,
        Left = 0x1 << 12,
        Up = 0x1 << 13,
        Right = 0x1 << 14,
        Down = 0x1 << 15,
    }

    /// <summary>
    /// 引数のJoy-Conのボタンをホールドしているときにtrueが返る関数
    /// </summary>
    /// <param name="_joyConButton"></param>
    /// <returns></returns>
    public bool GetKey(JoyConButton _joyConButton)
    {
        NpadButton _npadButton = TransrationJoyConButtonToNpadButton(_joyConButton);

        if (npadState.GetButton(_npadButton))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 引数のJoy-Conのボタンを離したときにtrueが返る関数
    /// </summary>
    /// <param name="_joyConButton"></param>
    /// <returns></returns>
    public bool GetKeyUp(JoyConButton _joyConButton)
    {
        NpadButton _npadButton = TransrationJoyConButtonToNpadButton(_joyConButton);

        if (npadState.GetButtonUp(_npadButton))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 引数のJoy-Conのボタンを押したときにtrueが返る関数
    /// </summary>
    /// <param name="_joyConButton"></param>
    /// <returns></returns>
    public bool GetKeyDown(JoyConButton _joyConButton)
    {
        NpadButton _npadButton = TransrationJoyConButtonToNpadButton(_joyConButton);

        if (npadState.GetButtonDown(_npadButton))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// SwitchInputManagerで定義しているJoyConButtonのenumをNintendoSDKで定義しているNpadButtonに変換する関数
    /// </summary>
    /// <param name="_joyConButton"></param>
    /// <returns></returns>
    private NpadButton TransrationJoyConButtonToNpadButton(JoyConButton _joyConButton)
    {
        return (NpadButton)Enum.Parse(typeof(NpadButton), _joyConButton.ToString());
    }

    /// <summary>
    /// 引数で指定したJoystickについているアナログスティックの入力情報を-1 ~ 1のVector2で返す関数
    /// </summary>
    /// <param name="_analogStick"></param>
    /// <returns></returns>
    public Vector2 GetAxis(AnalogStick _analogStick)
    {
        AnalogStickState _state = default;
        Vector2 axis;
        switch (_analogStick)
        {
            case AnalogStick.Right:
                _state = npadState.analogStickR;
                break;
            case AnalogStick.Left:
                _state = npadState.analogStickL;
                break;
            default:
                return Vector2.zero;
        }

        axis.x = (float)_state.x / (float)AnalogStickState.Max;
        axis.y = (float)_state.y / (float)AnalogStickState.Max;
        return axis;
    }

    ///<summary>加速度センサーの各方向ごとの加速度の値です。単位は G です。</summary>
    public Float3 LeftAcceleration { get; private set; }

    ///<summary>各方向ごとの角速度の値を積算して得られる回転角の値です。360度 を 1.0 とする値です。</summary>
    public Float3 LeftAngle { get; private set; }

    ///<summary>ジャイロセンサーの各方向ごとの角速度の値です。360dps を 1.0 とする値です。</summary>
    public Float3 LeftAngularVelocity { get; private set; }

    ///<summary>加速度センサーの各方向ごとの加速度の値です。単位は G です。</summary>
    public Float3 RightAcceleration { get; private set; }

    ///<summary>各方向ごとの角速度の値を積算して得られる回転角の値です。360度 を 1.0 とする値です。</summary>
    public Float3 RightAngle { get; private set; }

    /// <summary>ジャイロセンサーの各方向ごとの角速度の値です。360dps を 1.0 とする値です。</summary>
    public Float3 RightAngularVelocity { get; private set; }

    #region Joy-Conの接続をする処理

    void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedStyleSet(NpadStyle.Handheld | NpadStyle.JoyDual | NpadStyle.FullKey);
        NpadId[] npadIds = { NpadId.Handheld, NpadId.No1 };
        Npad.SetSupportedIdType(npadIds);
    }


    void Update()
    {
        NpadId npadId = NpadId.Handheld;
        NpadStyle npadStyle = NpadStyle.None;

        npadStyle = Npad.GetStyleSet(npadId);

        if (npadStyle != NpadStyle.Handheld)
        {
            npadId = NpadId.No1;
            npadStyle = Npad.GetStyleSet(npadId);
        }

        if (UpdatePadState())
        {
            for (int i = 0; i < handleCount; i++)
            {
                SixAxisSensor.GetState(ref state, handle[i]);
                //左0
                if (i == 0)
                {
                    LeftAcceleration = state.acceleration;
                    LeftAngle = state.angle;
                    LeftAngularVelocity = state.angularVelocity;
                }
                else if (i == 1) //右
                {
                    RightAcceleration = state.acceleration;
                    RightAngle = state.angle;
                    RightAngularVelocity = state.angularVelocity;
                }

                state.GetQuaternion(ref npadQuaternion);
                quaternion.Set(npadQuaternion.x, npadQuaternion.z, npadQuaternion.y, -npadQuaternion.w);
                if (handleCount == 1)
                {
                }
            }
        }
    }

    private bool UpdatePadState()
    {
        NpadStyle handheldStyle = Npad.GetStyleSet(NpadId.Handheld);
        NpadState handheldState = npadState;
        if (handheldStyle != NpadStyle.None)
        {
            Npad.GetState(ref handheldState, NpadId.Handheld, handheldStyle);
            if (handheldState.buttons != NpadButton.None)
            {
                if ((npadId != NpadId.Handheld) || (npadStyle != handheldStyle))
                {
                    this.GetSixAxisSensor(NpadId.Handheld, handheldStyle);
                    this.GetVibrationDevice(NpadId.Handheld, handheldStyle);
                }

                npadId = NpadId.Handheld;
                npadStyle = handheldStyle;
                npadState = handheldState;
                return true;
            }
        }

        NpadStyle no1Style = Npad.GetStyleSet(NpadId.No1);
        NpadState no1State = npadState;
        if (no1Style != NpadStyle.None)
        {
            Npad.GetState(ref no1State, NpadId.No1, no1Style);
            if (no1State.buttons != NpadButton.None)
            {
                if ((npadId != NpadId.No1) || (npadStyle != no1Style))
                {
                    this.GetSixAxisSensor(NpadId.No1, no1Style);
                    this.GetVibrationDevice(NpadId.No1, no1Style);
                }

                npadId = NpadId.No1;
                npadStyle = no1Style;
                npadState = no1State;
                return true;
            }
        }

        if ((npadId == NpadId.Handheld) && (handheldStyle != NpadStyle.None))
        {
            npadId = NpadId.Handheld;
            npadStyle = handheldStyle;
            npadState = handheldState;
        }
        else if ((npadId == NpadId.No1) && (no1Style != NpadStyle.None))
        {
            npadId = NpadId.No1;
            npadStyle = no1Style;
            npadState = no1State;
        }
        else
        {
            npadId = NpadId.Invalid;
            npadStyle = NpadStyle.Invalid;
            npadState.Clear();
            return false;
        }

        return true;
    }

    private void GetVibrationDevice(NpadId id, NpadStyle style)
    {
        vibrationValue.Clear();
        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.SendValue(vibrationDeviceHandles[i], vibrationValue);
        }

        vibrationDeviceCount = Vibration.GetDeviceHandles(
            vibrationDeviceHandles, vibrationDeviceCountMax, id, style);

        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.InitializeDevice(vibrationDeviceHandles[i]);
            Vibration.GetDeviceInfo(ref vibrationDeviceInfos[i], vibrationDeviceHandles[i]);
        }
    }

    private void GetSixAxisSensor(NpadId id, NpadStyle style)
    {
        for (int i = 0; i < handleCount; i++)
        {
            SixAxisSensor.Stop(handle[i]);
        }

        handleCount = SixAxisSensor.GetHandles(handle, 2, id, style);

        for (int i = 0; i < handleCount; i++)
        {
            SixAxisSensor.Start(handle[i]);
        }
    }

    #endregion
}