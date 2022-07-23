using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using nn.hid;
using UniRx;

public class ComboBonusBase
{
    //コンボをキャンセルするまでの時間
    private const float _ConboDestruction = 5f;
    //コンボをキャンセルするまでの時間を測るための変数    
    private float _Timer = 0f;
    //加速度の判定用の変数
    private const float _AccelerationJudgmentalue = 0.8f;

    //UniTaskのキャンセルトークンを保持しておく変数
    private CancellationTokenSource _cts;
    private CancellationToken _ct;

    private bool isCoroutineFlag = false;

    public JoyConAngleCheck.Position[] _ComboStock 
    = new JoyConAngleCheck.Position[4]
    {
        JoyConAngleCheck.Position.None,
        JoyConAngleCheck.Position.None,
        JoyConAngleCheck.Position.None,
        JoyConAngleCheck.Position.None
    };
    private readonly MessageBroker _broker = new();
    public IMessageBroker Broker => _broker;
    public void Start()
    {
        _cts = new CancellationTokenSource();
        _ct = _cts.Token;
    }
    
    /// <summary>
    /// コンボの登録をするメソッド
    /// 武器を振り始めたときに呼び出し、Joy-Conの加速度が_AccelerationJudgmentalueを下回ったときにいる角度に対応してコンボを登録する
    /// </summary>
    /// <param name="_joyStick">Joy-Conを持っている手のenumを引数で渡す</param>
    public void AddComboRegister(NpadJoyDeviceType _joyStick)
    {
        switch (_joyStick)
        {
            case NpadJoyDeviceType.Left:
                //JoyConToScreenPointer.Instance.AngleReset(JoyConToScreenPointer.JoyCon.Left);
                JoyConAngleCheck.Instance.PositoinReset(NpadJoyDeviceType.Left);
                LeftJoyCon();
                break;
            case NpadJoyDeviceType.Right:
                //JoyConToScreenPointer.Instance.AngleReset(JoyConToScreenPointer.JoyCon.Right);
                JoyConAngleCheck.Instance.PositoinReset(NpadJoyDeviceType.Right);
                RightJoyCon();
                break;
        }
    }
    public void Update()
    {
        //時間の計算
        if (isCoroutineFlag)
        {
            _Timer += Time.deltaTime;
        }
        //指定時間を経過したらコルーチンを止める
        if (_ConboDestruction <= _Timer && isCoroutineFlag)
        {
            isCoroutineFlag = false;
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
            _ComboStockClear();
        }
    }

    /// <summary>
    /// コンボを_ComboStockにストックする関数
    /// </summary>
    /// <param name="_position"></param>
    private void _ComboStockPush(JoyConAngleCheck.Position _position)
    {
        //一番うしろの要素がJoyConAngleCheck.Position.Noneのときのみ処理を実行する
        if (_ComboStock[_ComboStock.Length -1] == JoyConAngleCheck.Position.None)
        {
            for (int i = 0; i <= _ComboStock.Length -1; i++)
            {
                //先頭から順番に確認して最初に要素がNoneだったときに代入する
                if (_ComboStock[i] == JoyConAngleCheck.Position.None)
                {
                    _ComboStock[i] = _position;
                    //4つ目のコンボが入力されたら
                    if (i == _ComboStock.Length -1)
                    {                        
                        //コンボが存在しているかチェック
                        ComboBonusData comboBonusData= ComboBonusChecker.Check(_ComboStock);
                        if (comboBonusData !=null)
                        {
                            Broker.Publish(OnComboBonus.GetEvent(comboBonusData));
                        }
                        _ComboStockClear();
                        return;
                    }else
                    {
                        //代入したので処理の中断
                        return;
                    }
                }
            }
        }
    }
    /// <summary>
    /// _ComboStockの要素をJoyConAngleCheck.Position.Noneで初期化する
    /// </summary>
    private void _ComboStockClear()
    {
        for (int i = 0; i < _ComboStock.Length; i++)
        {
            _ComboStock[i] = JoyConAngleCheck.Position.None;
        }
    }
    
    /// <summary>
    /// 右Joy-Conの処理
    /// </summary>
    private async void RightJoyCon()
    {
        _Timer = 0;
        isCoroutineFlag = true;
        JoyConAngleCheck.Position _position = await GetRightPosition(_ct);
        _ComboStockPush(_position);
    }
    /// <summary>
    /// 左Joy-Conの処理
    /// </summary>
    private async void LeftJoyCon()
    {
        _Timer = 0;
        isCoroutineFlag = true;
        JoyConAngleCheck.Position _position = await GetLeftPosition(_ct);
        _ComboStockPush(_position);
    }

    /// <summary>
    /// 左Joy-Conを振ったときの角度を返す
    /// </summary>
    /// <returns></returns>
    private async UniTask<JoyConAngleCheck.Position> GetLeftPosition(CancellationToken cancellationToken = default)
    {
        await UniTask.WaitWhile(() =>
        {
            //キャンセルの処理
            cancellationToken.ThrowIfCancellationRequested();
            return LeftAcceleration();
        });
        return JoyConAngleCheck.Instance.GetJoyConAnglePosition(NpadJoyDeviceType.Left);

    }
    /// <summary>
    /// 右Joy-Conを振ったときの角度を返す
    /// </summary>
    /// <returns></returns>
    private async UniTask<JoyConAngleCheck.Position> GetRightPosition(CancellationToken cancellationToken = default)
    {
        await UniTask.WaitWhile(() =>
        {
            //キャンセルの処理
            cancellationToken.ThrowIfCancellationRequested();
            return RightAcceleration();
        });

        return JoyConAngleCheck.Instance.GetJoyConAnglePosition(NpadJoyDeviceType.Right);
    }
    
    /// <summary>
    /// 右Joy-Conの加速度が_AccelerationJudgmentalue以上のときにTrueを返す
    /// </summary>
    /// <returns></returns>
    private bool RightAcceleration()
    {
        return _AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.x ||
               _AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.y ||
               _AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.z;
    }

    /// <summary>
    /// 左Joy-Conの加速度が_AccelerationJudgmentalue以上のときにTrueを返す
    /// </summary>
    /// <returns></returns>
    private bool LeftAcceleration()
    {
        return _AccelerationJudgmentalue <= SwitchInputManager.Instance.LeftAcceleration.x ||
               _AccelerationJudgmentalue <= SwitchInputManager.Instance.LeftAcceleration.y ||
               _AccelerationJudgmentalue <= SwitchInputManager.Instance.LeftAcceleration.z;
    }
}
