public static class MainGameEvent
{
    public sealed class OnMissionEnded : EventMessage<OnMissionEnded, bool>
    {
        /// <summary>依頼をクリアしたか</summary>
        public bool IsClear => param1;
    }

    public static class Tutorial
    {
        public sealed class OnTask4Passed : EventMessage<OnTask4Passed>
        {
        }

        public sealed class OnTask5Passed : EventMessage<OnTask5Passed>
        {
        }
    }
}
