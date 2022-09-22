using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor : EditorWindow
    {
        private static EditorWindow _window;

        private static readonly Dictionary<RenderPage, EditorPage> _renderPageDict = new()
        {
            { RenderPage.Home, new EditorPage(HomeGUI, null) },
            { RenderPage.Enemy, new EditorPage(EnemyGUI, OnToggledEnemy) },
            { RenderPage.Item, new EditorPage(ItemGUI, null) },
            { RenderPage.Player, new EditorPage(PlayerGUI, OnToggledPlayer) },
            { RenderPage.Weapon, new EditorPage(WeaponGUI, null) },
            { RenderPage.Mission, new EditorPage(MissionGUI, OnToggledMission) }
        };

        private static readonly EditorPageManager _pageManager = new(_renderPageDict);

        /// <summary>
        /// <see cref="MasterDataEditor"/> のEditorWindow
        /// </summary>
        private static EditorWindow Window => _window ? _window : _window = GetWindow<MasterDataEditor>();

        [MenuItem("KOHP Tools/マスターデータエディター")]
        private static void ShowWindow()
        {
            _window              = GetWindow<MasterDataEditor>();
            _window.titleContent = new GUIContent(EditorConstants.ToolName);
            _window.minSize = new Vector2(16 * EditorConstants.WindowSizeRatio,
                                          9  * EditorConstants.WindowSizeRatio);

            _window.Show();
        }

        private void OnGUI()
        {
            // 現在のページに応じた内容を表示
            _renderPageDict[_pageManager.CurrentPage.Value]?.onGUI?.Invoke();
        }

        /// <summary>
        /// ホームページ用のGUI
        /// </summary>
        private static void HomeGUI()
        {
            // ヘッダー表示
            EditorGUILayout.LabelField("Master Data Editor", GUIStyles.HomeTitleStyle);

            List<HomeGridButton> buttons      = EditorConstants.Home.GridButtons;
            int                  passedButton = 0;

            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();

            // 各種マスターデータの編集画面に移動するボタンを表示
            for (int y = 0; y < EditorConstants.Home.MaxVerticalGrid; y++)
            {
                if (buttons.Count == 0) break;

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                for (int x = 0; x < EditorConstants.Home.MaxHorizontalGrid; x++)
                {
                    HomeGridButton button = buttons[passedButton];
                    GUIStyle       style  = GUIStyles.HomeButtonStyle(button);

                    // ボタンクリックで対応するページにセット
                    if (GUILayout.Button(button.label, style, GUILayout.Width(button.buttonSize.x),
                                         GUILayout.Height(button.buttonSize.y)))
                    {
                        _pageManager.CurrentPage.Value = (RenderPage) button.dataType;
                    }

                    passedButton++;

                    if (passedButton == buttons.Count)
                        break;
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (passedButton == buttons.Count)
                    break;
            }

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(EditorConstants.ToolVersion, GUIStyles.VersionStyle);
        }
    }
}
