using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class MissionTimer : MonoBehaviour
{
    private float time = 0f;
    private bool  goal = false;

    /// <summary>依頼遂行時間の計測を行うか</summary>
    public bool IsTimerActive { get; set; }

    private void Start()
    {
        // 依頼終了時にシーン遷移
        MainGameController.Instance.Broker.Receive<MainGameEvent.OnMissionEnded>()
                          .Subscribe(OnResult)
                          .AddTo(this);
    }

    private async void OnResult(MainGameEvent.OnMissionEnded data)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(.5));

        OnGoal();
    }

    // Update is called once per frame
    public void Update()
    {
        if (IsTimerActive == true)
        {
            time += Time.deltaTime; //毎フレームの時間を加算.
        }
    }
    public void OnGoal()
    {
        goal = true;
        var dataPack = (ToMainGameSceneDataPack) FadeContllor2.PreviousSceneData;
        ToResultSceneDataPack nextDataPack = new ToResultSceneDataPack(GameScene.MainGame, time, dataPack.MissionData);
        FadeContllor2.Instance.LoadScene(1, GameScene.Result, nextDataPack);
    }

    public void riset()
    {
        time = 0f;
    }
}
