/// <summary>
/// ゲーム全体のイベントをまとめたクラス
/// </summary>
public sealed class GameEvent
{
    public sealed class UI
    {
        public sealed class Input
        {
            /// <summary>
            /// UIの戻るを入力した際のメッセージ
            /// </summary>
            public sealed class OnBack : EventMessage<OnBack>
            {
            }
        }
    }
}
