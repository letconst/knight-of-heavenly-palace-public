namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static void PlayerGUI()
        {
            EditorUtility.SideMenuGUI(_pageManager.CurrentPage);
            EditorUtility.PanelGUI(LeftContent, RightContent);

            void LeftContent()
            {
            }

            void RightContent()
            {
            }
        }
    }
}
