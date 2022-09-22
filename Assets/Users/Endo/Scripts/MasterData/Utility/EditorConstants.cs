#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace KOHP.MasterData
{
    /// <summary>
    /// MasterDataEditorで使用される定数
    /// </summary>
    public static class EditorConstants
    {
        public const string ToolName = "マスターデータエディター";

        public const string ToolVersion = "v0.3.0";

        public const int WindowSizeRatio = 64;

        public static readonly Color DefaultTextColor = new(.8f, .8f, .8f, 1f);

        public const float LeftPanelWidthRatio = 7f;

        public const float RightPanelWidthRatio = 3f;

        public static class Path
        {
            public static readonly string BaseDataDir = "Assets/MasterData";

            public static readonly string EnemyDataDir = $"{BaseDataDir}/Enemy";

            public static readonly string ItemDataDir = $"{BaseDataDir}/Item";

            public static readonly string PlayerDataPath = $"{BaseDataDir}/PlayerData.asset";

            public static readonly string WeaponDataDir = $"{BaseDataDir}/Weapon";

            public static readonly string MissionDataDir = $"{BaseDataDir}/Mission";
        }

        public static class SideMenu
        {
            public const int FontSize = 12;

            public static readonly Vector2 ButtonSize = new(40f, 40f);

            public static readonly float Width = ButtonSize.x + 10f;

            public static readonly Color NormalButtonBg = new(.4f, .4f, .4f);

            public static readonly Color SelectedButtonBg = new(.3f, .6f, 1f);
        }

        public static class Home
        {
            public static readonly int MaxHorizontalGrid = 5;

            public static readonly int MaxVerticalGrid = 2;

            public static readonly Vector2 GridButtonSize = new(100f, 100f);

            public static readonly int GridButtonFontSize = 16;

            public static readonly List<HomeGridButton> GridButtons = new()
            {
                new HomeGridButton
                {
                    dataType = DataType.Enemy,
                    label    = "魔物",
                },
                new HomeGridButton
                {
                    dataType = DataType.Item,
                    label    = "アイテム",
                },
                new HomeGridButton
                {
                    dataType = DataType.Player,
                    label    = "プレイヤー",
                },
                new HomeGridButton
                {
                    dataType = DataType.Weapon,
                    label    = "武器",
                },
                new HomeGridButton
                {
                    dataType = DataType.Mission,
                    label    = "依頼"
                }
            };
        }

        public static class Enemy
        {
            public static readonly int ListFontSize = 16;
        }
    }
}
#endif
