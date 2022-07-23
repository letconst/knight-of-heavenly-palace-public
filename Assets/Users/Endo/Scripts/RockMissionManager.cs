using UnityEngine;

public class RockMissionManager : SingletonMonoBehaviour<RockMissionManager>
{
    [SerializeField]
    private int requireBreakCount;

    private int _brokeCount;

    public void CountUp()
    {
        _brokeCount++;

        if (_brokeCount >= requireBreakCount)
        {
            MainGameController.Instance.Broker.Publish(MainGameEvent.OnMissionEnded.GetEvent(true));
        }
    }
}
