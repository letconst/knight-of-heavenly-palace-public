using UnityEditor;

namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static Editor       _playerEditor;
        private static MasterPlayer _playerData;

        private static void PlayerGUI()
        {
            EditorUtility.SideMenuGUI(Window, _pageManager.CurrentPage);
            EditorUtility.PanelGUI(Window, LeftContent);

            void LeftContent()
            {
                if (!_playerData) return;

                _playerEditor = Editor.CreateEditor(_playerData);
                _playerEditor.OnInspectorGUI();

            }
        }

        private static void OnToggledPlayer(bool isOpened)
        {
            if (isOpened)
            {
                _playerData = EditorUtility.GetMasterDataFile<MasterPlayer>(EditorConstants.Path.PlayerDataPath);
            }
            else
            {
                _playerData = null;
            }
        }
    }
}
