using UnityEngine;

public partial class JoyConToScreenPointer
{
    private Vector3 _LeftAngleOffsetForSimulate;
    private Vector3 _RightAngleOffsetForSimulate;

    private readonly float[] _LeftAngularVelocityOffsetForSimulate  = { 0, 0, 0 };
    private readonly float[] _RightAngularVelocityOffsetForSimulate = { 0, 0, 0 };

    /// <summary><inheritdoc cref="LeftJoyConScreenVector2"/> (仮想用)</summary>
    public Vector2 LeftJoyConScreenVector2ForSimulate { get; private set; }

    /// <summary><inheritdoc cref="RightJoyConScreenVector2"/> (仮想用)</summary>
    public Vector2 RightJoyConScreenVector2ForSimulate { get; private set; }

    /// <summary><inheritdoc cref="LeftJoyConSingleCircleVector2"/> (仮想用)</summary>
    public Vector2 LeftJoyConSingleCircleVector2ForSimulate  { get; private set; }
    /// <summary><inheritdoc cref="RightJoyConSingleCircleVector2"/> (仮想用)</summary>
    public Vector2 RightJoyConSingleCircleVector2ForSimulate { get; private set; }

    public void AngleResetForSimulate(JoyCon _JoyCon = JoyCon.Both)
    {
        SwitchInputManager inputManager = SwitchInputManager.Instance;

        if (_JoyCon is JoyCon.Left or JoyCon.Both)
        {
            _LeftAngleOffsetForSimulate =  Vector3.zero;
            _LeftAngleOffsetForSimulate += inputManager.LeftAngle;

            _LeftAngularVelocityOffsetForSimulate[0] = -inputManager.LeftAngularVelocity.x;
            _LeftAngularVelocityOffsetForSimulate[1] = -inputManager.LeftAngularVelocity.y;
            _LeftAngularVelocityOffsetForSimulate[2] = -inputManager.LeftAngularVelocity.z;
        }

        if (_JoyCon is JoyCon.Right or JoyCon.Both)
        {
            _RightAngleOffsetForSimulate =  Vector3.zero;
            _RightAngleOffsetForSimulate += inputManager.RightAngle;

            _RightAngularVelocityOffsetForSimulate[0] = -inputManager.RightAngularVelocity.x;
            _RightAngularVelocityOffsetForSimulate[1] = -inputManager.RightAngularVelocity.y;
            _RightAngularVelocityOffsetForSimulate[2] = -inputManager.RightAngularVelocity.z;
        }
    }

    private void UpdateForSimulate()
    {
        float leftX;
        float leftY;
        float leftAngle;

        if (_GyroDriftCorrection) //ジャイロドリフト修正版
        {
            leftX = -LeftAngleZForSimulate()          * GyroDriftValueAngle +
                    LeftAngularVelocityZForSimulate() * GyroDriftValueAngleVelocity;

            leftY = LeftAngleXForSimulate()           * GyroDriftValueAngle +
                    LeftAngularVelocityXForSimulate() * GyroDriftValueAngleVelocity;

            leftAngle = -LeftAngleYForSimulate() * 360;
        }
        else //修正なし
        {
            leftX     = -LeftAngleZForSimulate();
            leftY     = LeftAngleXForSimulate();
            leftAngle = -LeftAngleYForSimulate() * 360;
        }

        float rightX;
        float rightY;
        float rightAngle;

        if (_GyroDriftCorrection) //ジャイロドリフト修正版
        {
            rightX = -RightAngleZForSimulate()          * GyroDriftValueAngle +
                     RightAngularVelocityZForSimulate() * GyroDriftValueAngleVelocity;

            rightY = RightAngleXForSimulate()           * GyroDriftValueAngle +
                     RightAngularVelocityXForSimulate() * GyroDriftValueAngleVelocity;

            rightAngle = -RightAngleYForSimulate() * 360;
        }
        else //修正なし
        {
            rightX     = -RightAngleZForSimulate();
            rightY     = RightAngleXForSimulate();
            rightAngle = -RightAngleYForSimulate() * 360;
        }

        LeftJoyConScreenVector2ForSimulate = RotationMatrixX(new Vector2(leftX, leftY), leftAngle);
        RightJoyConScreenVector2ForSimulate = RotationMatrixX(new Vector2(rightX, rightY), rightAngle);

        LeftJoyConSingleCircleVector2ForSimulate  = LeftJoyConScreenVector2ForSimulate;
        RightJoyConSingleCircleVector2ForSimulate = RightJoyConScreenVector2ForSimulate;
    }

    private float LeftAngularVelocityXForSimulate() =>
        SwitchInputManager.Instance.LeftAngularVelocity.x + _LeftAngularVelocityOffsetForSimulate[0];

    private float LeftAngularVelocityYForSimulate() =>
        SwitchInputManager.Instance.LeftAngularVelocity.y + _LeftAngularVelocityOffsetForSimulate[1];

    private float LeftAngularVelocityZForSimulate() =>
        SwitchInputManager.Instance.LeftAngularVelocity.z + _LeftAngularVelocityOffsetForSimulate[2];

    private float LeftAngleXForSimulate() => SwitchInputManager.Instance.LeftAngle.x - _LeftAngleOffsetForSimulate.x;
    private float LeftAngleYForSimulate() => SwitchInputManager.Instance.LeftAngle.y - _LeftAngleOffsetForSimulate.y;
    private float LeftAngleZForSimulate() => SwitchInputManager.Instance.LeftAngle.z - _LeftAngleOffsetForSimulate.z;

    private float RightAngularVelocityXForSimulate() =>
        SwitchInputManager.Instance.RightAngularVelocity.x + _RightAngularVelocityOffsetForSimulate[0];

    private float RightAngularVelocityYForSimulate() =>
        SwitchInputManager.Instance.RightAngularVelocity.y + _RightAngularVelocityOffsetForSimulate[1];

    private float RightAngularVelocityZForSimulate() =>
        SwitchInputManager.Instance.RightAngularVelocity.z + _RightAngularVelocityOffsetForSimulate[2];

    private float RightAngleXForSimulate() => SwitchInputManager.Instance.RightAngle.x - _RightAngleOffsetForSimulate.x;
    private float RightAngleYForSimulate() => SwitchInputManager.Instance.RightAngle.y - _RightAngleOffsetForSimulate.y;
    private float RightAngleZForSimulate() => SwitchInputManager.Instance.RightAngle.z - _RightAngleOffsetForSimulate.z;
}
