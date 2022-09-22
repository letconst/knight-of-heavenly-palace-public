using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Button Mission1Button;

    [SerializeField]
    UnityEngine.UI.Button Mission2Button;

    [SerializeField]
    UnityEngine.UI.Button TutorialButton;

    [SerializeField]
    GameObject PrefabButton;

    [SerializeField]
    GameObject ButtonParent;

    private Mission acceptedMission;

    [SerializeField]
    UnityEngine.UI.Button testButton;

    private MasterMission _tutorialData;
    private MasterMission _beginnerData;
    private MasterMission _advancedData;

    private void OnClickMissionButton()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(_beginnerData));
    }

    private void OnClickMission2Button()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(_advancedData));
    }

    private void OnClickMissionTutorialButton()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(_tutorialData));
    }
    // Start is called before the first frame update

    async void Start()
    {
        // 各種マスターデータ取得
        GetMissionMasterData().Forget();

        MasterMission data = await MasterDataManager.Instance.GetMasterDataAsync<MasterMission>("Mission");

        // foreach (MasterMission.MissionData item in data.MissionDatas)
        // {
        //     GameObject newButton = Instantiate(PrefabButton, ButtonParent.transform);
        //     newButton.GetComponentInChildren<Text>().text = item.missionName;
        //     Debug.Log(newButton.GetComponentInChildren<Text>());
        // }

        Mission1Button.onClick.AddListener(OnClickMissionButton);

        Mission2Button.onClick.AddListener(OnClickMission2Button);

        TutorialButton.onClick.AddListener(OnClickMissionTutorialButton);
    }

    private async UniTaskVoid GetMissionMasterData()
    {
        List<UniTask> getTasks = new()
        {
            UniTask.Create(async () =>
            {
                _tutorialData = await MasterDataManager.Instance.GetMasterDataAsync<MasterMission>("100");
            }),
            UniTask.Create(async () =>
            {
                _beginnerData = await MasterDataManager.Instance.GetMasterDataAsync<MasterMission>("101");
            }),
            UniTask.Create(async () =>
            {
                _advancedData = await MasterDataManager.Instance.GetMasterDataAsync<MasterMission>("102");
            })
        };

        await getTasks;
    }
}
