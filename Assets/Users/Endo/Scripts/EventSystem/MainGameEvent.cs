public static class MainGameEvent
{
    public sealed class OnMissionEnded : EventMessage<OnMissionEnded, bool>
    {
        /// <summary>依頼をクリアしたか</summary>
        public bool IsClear => param1;
    }
}
