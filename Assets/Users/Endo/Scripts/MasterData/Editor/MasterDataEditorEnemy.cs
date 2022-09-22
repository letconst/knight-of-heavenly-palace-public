using UnityEngine;
using UnityEditor;

namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static Editor           _enemyEditor;
        private static ScriptableObject _enemyTarget;
        private static MasterEnemy[]    _enemyData;

        /// <summary>
        /// 魔物データ編集ページ用のGUI
        /// </summary>
        private static void EnemyGUI()
        {
            EditorUtility.SideMenuGUI(Window, _pageManager.CurrentPage);
            EditorUtility.PanelGUI(Window, LeftContent, RightContent);

            void LeftContent()
            {
                // TODO: 各データをグリッドでも表示したい（切り替え）
                foreach (MasterEnemy data in _enemyData)
                {
                    if (!data) continue;

                    if (GUILayout.Button($"{data.Name} ({data.Id})", GUIStyles.EnemyListButtonStyle))
                    {
                        _enemyTarget = data;
                    }
                }
            }

            void RightContent()
            {
                if (!_enemyTarget) return;

                _enemyEditor = Editor.CreateEditor(_enemyTarget);

                _enemyEditor.OnInspectorGUI();
            }
        }

        private static void OnToggledEnemy(bool isOpened)
        {
            if (isOpened)
            {
                _enemyData = EditorUtility.GetMasterDataFiles<MasterEnemy>(EditorConstants.Path.EnemyDataDir);
            }
            else
            {
                System.Array.Clear(_enemyData, 0, _enemyData.Length);
            }
        }
    }
}
