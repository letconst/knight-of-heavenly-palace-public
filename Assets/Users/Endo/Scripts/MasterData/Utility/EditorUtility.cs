#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    public static class EditorUtility
    {
        private static Vector2 _leftScrollPos;
        private static Vector2 _rightScrollPos;

        /// <summary>
        /// ホームページへの戻るボタンを配置する
        /// </summary>
        /// <param name="page"></param>
        public static void BackGUI(UniRx.IReactiveProperty<RenderPage> page)
        {
            GUIStyle style = GUIStyles.SideMenuButtonStyle(EditorConstants.DefaultTextColor);
            style.fontSize = EditorConstants.SideMenu.FontSize + 6;

            if (GUILayout.Button("←", style))
            {
                page.Value = RenderPage.Home;
            }
        }

        /// <summary>
        /// ホームページでのボタンをサイドメニューとして表示する
        /// </summary>
        /// <param name="page"></param>
        public static void SideMenuGUI(EditorWindow window, UniRx.IReactiveProperty<RenderPage> page)
        {
            List<HomeGridButton> buttons   = EditorConstants.Home.GridButtons;
            float                menuWidth = EditorConstants.SideMenu.Width;

            GUILayout.BeginArea(new Rect(0, 0, menuWidth, window.position.height), GUIStyles.PanelBoxStyle());
            EditorGUILayout.BeginVertical();

            Color prevColor = GUI.backgroundColor;

            // ホームページで表示するボタンをサイドメニューとして羅列
            foreach (HomeGridButton button in buttons)
            {
                GUIStyle style    = GUIStyles.SideMenuButtonStyle(button.fontColor);
                var      toRender = (RenderPage) button.dataType;

                style.normal.textColor = Color.white;

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

            GUI.backgroundColor = EditorConstants.SideMenu.NormalButtonBg;

            BackGUI(page);

            GUI.backgroundColor = prevColor;
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// 各データ編集ページでの左右に分かれたレイアウトを表示する
        /// </summary>
        /// <param name="leftContent">左側で表示する内容</param>
        /// <param name="rightContent">右側で表示する内容</param>
        public static void PanelGUI(EditorWindow window, System.Action leftContent, System.Action rightContent = null)
        {
            // TODO: サイドメニューがない場合も考慮したマージンにしたい
            float leftWidth     = window.position.width       / 10f;
            float rightWidth    = window.position.width / 10f * EditorConstants.RightPanelWidthRatio;
            float sideMenuWidth = EditorConstants.SideMenu.Width;

            // 右側領域表示
            if (rightContent != null)
            {
                // 右側を表示する場合、左側の幅を7割に
                leftWidth *= EditorConstants.LeftPanelWidthRatio;
                leftWidth -= sideMenuWidth;

                GUILayout.BeginArea(new Rect(leftWidth + sideMenuWidth, 0, rightWidth, window.position.height),
                                    GUIStyles.PanelBoxStyle());

                _leftScrollPos = EditorGUILayout.BeginScrollView(_leftScrollPos);

                rightContent?.Invoke();

                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();
            }
            else
            {
                // 右側を表示しない場合、左側を全幅で表示
                leftWidth *= 10f;
                leftWidth -= sideMenuWidth;
            }

            // 左側領域表示
            if (leftContent != null)
            {
                GUILayout.BeginArea(new Rect(sideMenuWidth, 0, leftWidth, window.position.height), GUIStyles.PanelBoxStyle());
                _rightScrollPos = EditorGUILayout.BeginScrollView(_rightScrollPos);

                leftContent?.Invoke();

                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();
            }
        }

        /// <summary>
        /// <see cref="EditorGUILayout.BeginHorizontal(UnityEngine.GUILayoutOption[])">BeginHorizontal()</see>
        /// と
        /// <see cref="EditorGUILayout.EndHorizontal"/>
        /// のラッパーメソッド
        /// </summary>
        /// <param name="content">表示する内容</param>
        public static void HorizontalArea(System.Action content)
        {
            EditorGUILayout.BeginHorizontal();

            content?.Invoke();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// <see cref="EditorGUILayout.BeginVertical(UnityEngine.GUILayoutOption[])">BeginVertical()</see>
        /// と
        /// <see cref="EditorGUILayout.EndVertical"/>
        /// のラッパーメソッド
        /// </summary>
        /// <param name="content">表示する内容</param>
        public static void VerticalArea(System.Action content)
        {
            EditorGUILayout.BeginVertical();

            content?.Invoke();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 複数列列で表示する領域を作成する
        /// </summary>
        /// <param name="contents">表示する内容。左から順に表示される</param>
        public static void MultipleColumnArea(params System.Action[] contents)
        {
            HorizontalArea(() =>
            {
                foreach (System.Action content in contents)
                {
                    if (content == null) continue;

                    VerticalArea(content);
                }
            });
        }

        /// <summary>
        /// 区切りを表示する
        /// </summary>
        public static void Separator()
        {
            Color prevColor = GUI.backgroundColor;

            EditorGUILayout.Space();
            GUI.backgroundColor = Color.black;
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUI.backgroundColor = prevColor;
            EditorGUILayout.Space();
        }

        public static T GetMasterDataFile<T>(string assetPath) where T : MasterDataBase
        {
            var file = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            return file;
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
#endif
