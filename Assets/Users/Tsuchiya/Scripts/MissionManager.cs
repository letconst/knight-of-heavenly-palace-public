using UnityEngine;
using UnityEngine.UI;

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

    private void OnClickMissionButton()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        acceptedMission = new Mission("依頼1",1);

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(acceptedMission));
    }

    private void OnClickMission2Button()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        acceptedMission = new Mission("依頼2", 2);

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(acceptedMission));
    }

    private void OnClickMissionTutorialButton()
    {
        if (LobbyController.Instance.StateManager.HasState(LobbyStateManager.LobbyState.MissionSelected))
            return;

        acceptedMission = new Mission("チュートリアル", 3);

        LobbyController.Instance.Broker.Publish(LobbyEvent.OnMissionSelected.GetEvent(acceptedMission));
    }
    // Start is called before the first frame update

    async void Start()
    {
        MasterMission data = await MasterDataManager.Instance.GetMasterDataAsync<MasterMission>("Mission");

        // foreach (MasterMission.MissionData item in data.MissionDatas)
        // {
        //     GameObject newButton = Instantiate(PrefabButton, ButtonParent.transform);
        //     newButton.GetComponentInChildren<Text>().text = item.missionName;
        //     Debug.Log(newButton.GetComponentInChildren<Text>());
        // }

        // Mission1Button.onClick.AddListener(OnClickMissionButton);

        // Mission2Button.onClick.AddListener(OnClickMission2Button);

        TutorialButton.onClick.AddListener(OnClickMissionTutorialButton);
    }
}
