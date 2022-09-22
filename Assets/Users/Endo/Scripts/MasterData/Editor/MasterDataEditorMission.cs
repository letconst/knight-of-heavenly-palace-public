using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    public sealed partial class MasterDataEditor
    {
        private static Editor           _missionEditor;
        private static ScriptableObject _missionTarget;
        private static MasterMission[]  _missionData;

        private static void MissionGUI()
        {
            EditorUtility.SideMenuGUI(Window, _pageManager.CurrentPage);
            EditorUtility.PanelGUI(Window, LeftContent, RightContent);

            void LeftContent()
            {
                foreach (MasterMission data in _missionData)
                {
                    if (!data) continue;

                    if (GUILayout.Button($"{data.MissionName} ({data.Id})", GUIStyles.EnemyListButtonStyle))
                    {
                        _missionTarget = data;
                    }
                }
            }

            void RightContent()
            {
                if (!_missionTarget) return;

                _missionEditor = Editor.CreateEditor(_missionTarget);

                _missionEditor.OnInspectorGUI();
            }
        }

        private static void OnToggledMission(bool isOpened)
        {
            if (isOpened)
            {
                _missionData = EditorUtility.GetMasterDataFiles<MasterMission>(EditorConstants.Path.MissionDataDir);
            }
            else
            {
                System.Array.Clear(_missionData, 0, _missionData.Length);
            }
        }
    }
}
