namespace Endo.Sample.PlayerInputEvent
{
    /// <summary>
    /// ゲーム全般に関するイベントをまとめたクラス
    /// </summary>
    public sealed class GameEvent
    {
        public sealed class UI
        {
            public sealed class Input
            {
                /// <summary>
                /// UIの戻る入力をした際のメッセージ
                /// </summary>
                public sealed class OnBack : EventMessage<OnBack>
                {
                }
            }
        }
    }
}
