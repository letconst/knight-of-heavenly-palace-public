using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public sealed class TutorialController : SingletonMonoBehaviour<TutorialController>
{
    [SerializeField, TextArea]
    private string[] messages;

    [SerializeField]
    private Text messageText;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CanvasGroup textCanvasGroup;

    [SerializeField]
    private Collider task1Target;

    [SerializeField]
    private Renderer task2Target;

    private int  _completedCount;
    private bool _isNavigationCompleted;

    private CanvasGroup _enemyCountCanvasGroup;

    private readonly bool[] _results = new bool[9];

    private readonly Func<bool>[] _tasks =
    {
        () => Instance._results[0],
        () =>
        {
            Instance.Task2Judge();

            return Instance._results[1];
        },
        () => Instance._results[2],
        () => Instance._results[3],
        () => Instance._results[4],
        () => Instance._results[5],
        () => Instance._results[6],
        () => Instance._results[7],
    };

    private IDisposable _task1Disposable;
    private IDisposable _task4Disposable;
    private IDisposable _task5Disposable;
    private IDisposable _task8Disposable;

    private void Start()
    {
        _enemyCountCanvasGroup       = FindObjectOfType<EnemyCount>().CanvasGroup;
        _enemyCountCanvasGroup.alpha = 0;

        IMessageReceiver broker = PlayerInputEventEmitter.Instance.Broker;

        // タスク1の
        _task1Disposable = task1Target.OnTriggerEnterAsObservable()
                                      .Subscribe(Task1Judge);

        broker.Receive<PlayerEvent.OnStateChanged>()
              .Subscribe(StateSwitching)
              .AddTo(this);

        _task4Disposable = broker.Receive<MainGameEvent.Tutorial.OnTask4Passed>()
                                 .Subscribe(_ => Task4Judge());

        _task5Disposable = broker.Receive<MainGameEvent.Tutorial.OnTask5Passed>()
                                 .Subscribe(_ => Task5Judge());

        _task8Disposable = EnemyManeger.Instance.Broker
                                       .Receive<OnAttackEnemy>()
                                       .Subscribe(_ => Task8Judge());
    }

    private async void Update()
    {
        if (_isNavigationCompleted) return;

        // 誘導達成で次の項目へ
        if (_completedCount < _tasks.Length && _tasks[_completedCount].Invoke())
        {
            _completedCount++;

            await textCanvasGroup.ToggleFade(false, 1f);

            // 表示テキスト更新
            messageText.text = messages[_completedCount];

            await textCanvasGroup.ToggleFade(true, 1f);
        }
        // 最後のタスク表示後、討伐数を切り替え表示
        else if (_completedCount == _tasks.Length)
        {
            _isNavigationCompleted = true;

            await UniTask.Delay(7000);

            await canvasGroup.ToggleFade(false, 1f);
            _enemyCountCanvasGroup.ToggleFade(true, 1f).Forget();
        }
    }

    private void StateSwitching(PlayerEvent.OnStateChanged data)
    {
        if (data.State is PlayerStatus.PlayerState.ThrowingL or PlayerStatus.PlayerState.ThrowingR)
        {
            Task3Judge();
        }
        else if (data.State is PlayerStatus.PlayerState.HangingL or PlayerStatus.PlayerState.HangingR)
        {
            Task6Judge();
        }
        else if (data.State == PlayerStatus.PlayerState.AttackMode)
        {
            Task7Judge();
        }
    }

    private void Task1Judge(Collider other)
    {
        if (_completedCount == 0)
        {
            if (other.isTrigger)
                return;

            if (1 << other.gameObject.layer == LayerConstants.Player)
            {
                _results[_completedCount] = true;
                _task1Disposable.Dispose();
            }
        }
    }

    private void Task2Judge()
    {
        if (_completedCount == 1)
        {
            _results[_completedCount] = task2Target.isVisible;
        }
    }

    private void Task3Judge()
    {
        if (_completedCount == 2)
        {
            _results[_completedCount] = true;
        }
    }

    private void Task4Judge()
    {
        if (_completedCount == 3)
        {
            _results[_completedCount] = true;
            _task4Disposable.Dispose();
        }
    }

    private void Task5Judge()
    {
        if (_completedCount == 4)
        {
            _results[_completedCount] = true;
            _task5Disposable.Dispose();
        }
    }

    private async void Task6Judge()
    {
        if (_completedCount == 5)
        {
            await UniTask.Delay(5000);

            _results[_completedCount] = true;
        }
    }

    private void Task7Judge()
    {
        if (_completedCount == 6)
        {
            _results[_completedCount] = true;
        }
    }

    private void Task8Judge()
    {
        if (_completedCount == 7)
        {
            _results[_completedCount] = true;
            _task8Disposable.Dispose();
        }
    }
}
