namespace KOHP.MasterData
{
    /// <summary>
    /// MasterDataEditorの各ページで使用されるボタンのデータクラス
    /// </summary>
    public abstract class GridButtonBase
    {
        public UnityEngine.Vector2 buttonSize;
        public string              label;
        public int                 fontSize;
        public UnityEngine.Color   fontColor = EditorConstants.DefaultTextColor;
    }

    /// <summary>
    /// MasterDataEditorのホームページで使用されるボタンのデータクラス
    /// </summary>
    public sealed class HomeGridButton : GridButtonBase
    {
        public DataType dataType;

        public HomeGridButton()
        {
            buttonSize = EditorConstants.Home.GridButtonSize;
            fontSize   = EditorConstants.Home.GridButtonFontSize;
        }
    }
}
