#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    /// <summary>
    /// MasterDataEditorで使用されるGUIStyleをまとめたクラス
    /// </summary>
    public static class GUIStyles
    {
        public static readonly GUIStyle HomeTitleStyle = new()
        {
            alignment = TextAnchor.UpperCenter,
            fontSize  = 36,
            fontStyle = FontStyle.Bold,
            normal =
            {
                textColor = EditorConstants.DefaultTextColor
            }
        };

        public static readonly GUIStyle VersionStyle = new()
        {
            alignment = TextAnchor.LowerRight,
            fontSize  = 16,
            normal =
            {
                textColor = EditorConstants.DefaultTextColor
            }
        };

        public static GUIStyle PanelBoxStyle()
        {
            GUIStyle style = new(EditorStyles.helpBox);

            return style;
        }

        public static readonly GUIStyle EnemyListButtonStyle = new(EditorStyles.miniButton)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize  = EditorConstants.Enemy.ListFontSize,
            fixedHeight = 26,
            margin =
            {
                bottom = 5
            },
            padding =
            {
                left = 10
            }
        };

        public static GUIStyle HomeButtonStyle(HomeGridButton button)
        {
            GUIStyle style = new(EditorStyles.miniButton)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize  = button.fontSize,
                normal =
                {
                    textColor = button.fontColor
                },
                fixedWidth  = button.buttonSize.x,
                fixedHeight = button.buttonSize.y,
                margin =
                {
                    right  = 10,
                    bottom = 10
                }
            };

            return style;
        }

        public static GUIStyle SideMenuButtonStyle(Color textColor)
        {
            GUIStyle style = new(EditorStyles.miniButton)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize  = EditorConstants.SideMenu.FontSize,
                normal =
                {
                    textColor = textColor
                },
                fixedWidth  = EditorConstants.SideMenu.ButtonSize.x,
                fixedHeight = EditorConstants.SideMenu.ButtonSize.y,
                margin =
                {
                    bottom = 5
                }
            };

            return style;
        }

        public static GUIStyle PropertyHeaderStyle()
        {
            GUIStyle style = new(EditorStyles.label)
            {
                fontSize  = 16,
                fontStyle = FontStyle.Bold
            };

            return style;
        }
    }
}
#endif
