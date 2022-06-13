using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    public static class EditorUtility
    {
        /// <summary>
        /// ホームページへの戻るボタンを配置する
        /// </summary>
        /// <param name="page"></param>
        public static void BackGUI(UniRx.IReactiveProperty<RenderPage> page)
        {
            GUIStyle style = GUIStyles.SideMenuButtonStyle(EditorConstants.DefaultTextColor);
            GUI.skin.button.fontSize = EditorConstants.SideMenu.FontSize + 6;

            if (GUILayout.Button("←", style))
            {
                page.Value = RenderPage.Home;
            }
        }

        /// <summary>
        /// ホームページでのボタンをサイドメニューとして表示する
        /// </summary>
        /// <param name="page"></param>
        public static void SideMenuGUI(UniRx.IReactiveProperty<RenderPage> page)
        {
            List<HomeGridButton> buttons   = EditorConstants.Home.GridButtons;
            float                menuWidth = EditorConstants.SideMenu.Width;
            EditorWindow         window    = EditorWindow.GetWindow(typeof(MasterDataEditor));

            GUILayout.BeginArea(new Rect(0, 0, menuWidth, window.position.height), GUIStyles.PanelBoxStyle());
            EditorGUILayout.BeginVertical();

            // ホームページで表示するボタンをサイドメニューとして羅列
            foreach (HomeGridButton button in buttons)
            {
                GUIStyle style    = GUIStyles.SideMenuButtonStyle(button.fontColor);
                var      toRender = (RenderPage) button.dataType;

                GUI.contentColor = Color.white;

                // 表示中のページのボタン背景色を選択色に
                if (page.Value == toRender)
                {
                    GUI.backgroundColor = EditorConstants.SideMenu.SelectedButtonBg;
                }
                else
                {
                    GUI.backgroundColor = EditorConstants.SideMenu.NormalButtonBg;
                }

                if (GUILayout.Button(button.label, style))
                {
                    page.Value = toRender;
                }
            }

            GUILayout.FlexibleSpace();

            GUI.enabled         = true;
            GUI.backgroundColor = EditorConstants.SideMenu.NormalButtonBg;

            BackGUI(page);

            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// 各データ編集ページでの左右に分かれたレイアウトを表示する
        /// </summary>
        /// <param name="leftContent">左側で表示する内容</param>
        /// <param name="rightContent">右側で表示する内容</param>
        public static void PanelGUI(System.Action leftContent, System.Action rightContent)
        {
            EditorWindow window = EditorWindow.GetWindow<MasterDataEditor>();

            // TODO: サイドメニューがない場合も考慮したマージンにしたい
            float leftWidth     = window.position.width / 10f * EditorConstants.LeftPanelWidthRatio;
            float rightWidth    = window.position.width / 10f * EditorConstants.RightPanelWidthRatio;
            float sideMenuWidth = EditorConstants.SideMenu.Width;
            leftWidth -= sideMenuWidth;

            // 左側領域表示
            GUILayout.BeginArea(new Rect(sideMenuWidth, 0, leftWidth, window.position.height), GUIStyles.PanelBoxStyle());
            leftContent?.Invoke();
            GUILayout.EndArea();

            // 右側領域表示
            GUILayout.BeginArea(new Rect(leftWidth + sideMenuWidth, 0, rightWidth, window.position.height),
                                GUIStyles.PanelBoxStyle());

            rightContent?.Invoke();
            GUILayout.EndArea();
        }

        public static T[] GetMasterDataFiles<T>(string assetDir) where T : MasterDataBase
        {
            string[] files = Directory.GetFiles(assetDir, "*.asset");

            return files.Select(AssetDatabase.LoadAssetAtPath<T>)
                        .Where(masterData => masterData)
                        .ToArray();
        }
    }
}
