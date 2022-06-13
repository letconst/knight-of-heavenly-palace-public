namespace KOHP.MasterData
{
    public sealed class EditorPage
    {
        public readonly System.Action       onGUI;
        public readonly System.Action<bool> onToggled;

        public EditorPage(System.Action onGUI, System.Action<bool> onToggled)
        {
            this.onGUI     = onGUI;
            this.onToggled = onToggled;
        }
    }
}
