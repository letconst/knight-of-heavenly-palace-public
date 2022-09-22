using UnityEngine;

public sealed class MainGameMissionManager : SingletonMonoBehaviour<MainGameMissionManager>
{
    [SerializeField, Header("依頼達成に必要な回数")]
    private int requireCount;
    
    public int _RequireCount
    {
        get => requireCount;
    }
    public int _CurrentCount { get; private set;}

    public void CountUp()
    {
        _CurrentCount++;

        if (_CurrentCount >= requireCount)
        {
            MainGameController.Instance.Broker.Publish(MainGameEvent.OnMissionEnded.GetEvent(true));
        }
    }
}