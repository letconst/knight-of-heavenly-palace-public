namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static void ItemGUI()
        {
            EditorUtility.SideMenuGUI(Window, _pageManager.CurrentPage);
            EditorUtility.PanelGUI(Window, LeftContent, RightContent);

            void LeftContent()
            {
            }

            void RightContent()
            {
            }
        }
    }
}
