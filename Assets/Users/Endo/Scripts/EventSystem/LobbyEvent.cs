/// <summary>
/// ロビー用のイベント定義クラス
/// </summary>
public static class LobbyEvent
{
    /// <summary>依頼を選択した際のイベント</summary>
    public sealed class OnMissionSelected : EventMessage<OnMissionSelected, Mission>
    {
        /// <summary>選択した依頼情報</summary>
        public Mission SelectedMission => param1;
    }

    /// <summary>依頼選択をキャンセルした際のイベント</summary>
    public sealed class OnMissionSelectCancelled : EventMessage<OnMissionSelectCancelled>
    {
    }

    /// <summary>ロビーのステートが変更された際のイベント</summary>
    public sealed class OnStateChanged : EventMessage<OnStateChanged, LobbyStateManager.LobbyState>
    {
        /// <summary>変更されたステート</summary>
        public LobbyStateManager.LobbyState NewState => param1;
    }
}