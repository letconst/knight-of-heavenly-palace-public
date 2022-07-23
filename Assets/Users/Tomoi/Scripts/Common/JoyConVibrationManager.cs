using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using nn.hid;

public class JoyConVibrationManager : SingletonMonoBehaviour<JoyConVibrationManager>
{
    private VibrationValue vibrationValueR = VibrationValue.Make();
    private VibrationValue vibrationValueL = VibrationValue.Make();
    
    //UniTaskのキャンセルトークンを保持しておく変数
    private CancellationTokenSource _ctsR;
    private CancellationToken _ctR;
    private CancellationTokenSource _ctsL;
    private CancellationToken _ctL;


    private SwitchInputManager _switchInputManager;

    void Start()
    {
        //UniTaskのキャンセルトークンを発行する処理
        _ctsR = new CancellationTokenSource();
        _ctR = _ctsR.Token;
        _ctsL = new CancellationTokenSource();
        _ctL = _ctsL.Token;

        //SwitchInputManagerの取得
        _switchInputManager = SwitchInputManager.Instance;
        
    }

    public enum VibrationType
    {
        Random,
    }

    /// <summary>
    /// Joy-Conをバイブレーションさせる関数
    /// 時間やバイブレーションの種類を引数で指定可能
    /// </summary>
    /// <param name="_joyStick">バイブレーションさせるJoy-Conを指定</param>
    /// <param name="_vibrationTime">バイブレーションさせる時間</param>
    /// <param name="_vibrationType">振動の種類</param>
    public void JoyConVibration(JoyCon _joyStick, float _vibrationTime = 0.1f,
        VibrationType _vibrationType = VibrationType.Random)
    {
        switch (_joyStick)
        {
            case JoyCon.Left:
            {
                _ctsL.Cancel();
                _ctsL = new CancellationTokenSource();
                _ctL = _ctsL.Token;
                PlayVibrationL(_ctL, _vibrationTime, _vibrationType).Forget();
            }
                break;
            case JoyCon.Right:
            {
                _ctsR.Cancel();
                _ctsR = new CancellationTokenSource();
                _ctR = _ctsR.Token;
                PlayVibrationR(_ctR, _vibrationTime, _vibrationType).Forget();
            }
                break;
            case JoyCon.Both:
            {
                _ctsL.Cancel();
                _ctsL = new CancellationTokenSource();
                _ctL = _ctsL.Token;
                PlayVibrationL(_ctL, _vibrationTime, _vibrationType).Forget();
                
                _ctsR.Cancel();
                _ctsR = new CancellationTokenSource();
                _ctR = _ctsR.Token;
                PlayVibrationR(_ctR, _vibrationTime, _vibrationType).Forget();
            }
                break;
        }
    }
    private async UniTask PlayVibrationR(CancellationToken _cancellationToken = default, float _vibrationTime = 0.1f, VibrationType _vibrationType = VibrationType.Random)
    {
        float _time = 0;
        while (_time < _vibrationTime)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            SetVibrationValue(_vibrationType, ref vibrationValueR);

            onVibration();
            _time += Time.deltaTime;
            await UniTask.Yield();
        }

        //バイブレーション情報のクリア        

        vibrationValueR.Clear();

        onVibration();
    }
    private async UniTask PlayVibrationL(CancellationToken _cancellationToken = default, float _vibrationTime = 0.1f, VibrationType _vibrationType = VibrationType.Random)
    {
        float _time = 0;
        while (_time < _vibrationTime)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            SetVibrationValue(_vibrationType, ref vibrationValueL);

            onVibration();
            _time += Time.deltaTime;
            await UniTask.Yield();
        }

        //バイブレーション情報のクリア        

        vibrationValueL.Clear();

        onVibration();
    }
    private void onVibration()
    {
        if (_switchInputManager.vibrationDeviceCount != 0)
        {
            Vibration.SendValue(_switchInputManager.vibrationDeviceHandles[0], vibrationValueL);
            Vibration.SendValue(_switchInputManager.vibrationDeviceHandles[1], vibrationValueR);
        }
    }

    private void SetVibrationValue(VibrationType _vibrationType, ref VibrationValue _vibrationValue)
    {
        switch (_vibrationType)
        {
            case VibrationType.Random:
            {
                _vibrationValue.amplitudeLow = Random.Range(0f, 1f);
                _vibrationValue.amplitudeHigh = 0f;
            }
                break;
        }
    }
}