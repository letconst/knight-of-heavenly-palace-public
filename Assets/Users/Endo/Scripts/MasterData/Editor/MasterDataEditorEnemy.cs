﻿using UnityEngine;
using UnityEditor;

namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static Vector2 _enemyScrollPos;

        private static Editor           _enemyEditor;
        private static ScriptableObject _target;
        private static MasterEnemy[]    _enemyData;

        /// <summary>
        /// 魔物データ編集ページ用のGUI
        /// </summary>
        private static void EnemyGUI()
        {
            EditorUtility.SideMenuGUI(_pageManager.CurrentPage);
            EditorUtility.PanelGUI(LeftContent, RightContent);

            void LeftContent()
            {
                _enemyScrollPos = EditorGUILayout.BeginScrollView(_enemyScrollPos);

                // TODO: 各データをグリッドでも表示したい（切り替え）
                foreach (MasterEnemy data in _enemyData)
                {
                    if (!data) continue;

                    if (GUILayout.Button($"{data.Name} ({data.Id})", GUIStyles.EnemyListButtonStyle))
                    {
                        _target = data;
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            void RightContent()
            {
                if (!_target) return;

                _enemyEditor = Editor.CreateEditor(_target);

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