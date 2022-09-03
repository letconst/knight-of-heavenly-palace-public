#if UNITY_EDITOR
using UnityEditor;
using KOHP.MasterData;
using EditorUtility = KOHP.MasterData.EditorUtility;

public partial class MasterPlayer
{
    [CustomEditor(typeof(MasterPlayer))]
    private class Editor : UnityEditor.Editor
    {
        private MasterPlayer _instance;

        private SerializedProperty _normalDodgeInfo;
        private SerializedProperty _inAirDodgeInfo;
        private SerializedProperty _escapingDodgeInfo;

        private void OnEnable()
        {
            _instance = (MasterPlayer) target;

            // 各SerializedPropertyを取得
            _normalDodgeInfo   = serializedObject.FindProperty(nameof(_instance.normalDodgeInfo));
            _inAirDodgeInfo    = serializedObject.FindProperty(nameof(_instance.inAirDodgeInfo));
            _escapingDodgeInfo = serializedObject.FindProperty(nameof(_instance.escapingDodgeInfo));
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(_instance, "");
            EditorGUI.BeginChangeCheck();

            // ステータス表示
            DrawStatusInformation();

            EditorUtility.Separator();

            // 攻撃情報表示
            DrawAttackInformation();

            EditorUtility.Separator();

            // 回避情報表示
            DrawDodgeInformation();

            if (EditorGUI.EndChangeCheck())
            {
                UnityEditor.EditorUtility.SetDirty(_instance);
            }
        }

        private void DrawStatusInformation()
        {
            EditorGUILayout.LabelField("ステータス", GUIStyles.PropertyHeaderStyle());

            EditorUtility.MultipleColumnArea(() =>
            {
                _instance.maxHitPoint.IntField(MasterDataModelConstants.Player.MaxHitPointLabel);
                _instance.maxStaminaPoint.IntField(MasterDataModelConstants.Player.MaxStaminaPointLabel);
                _instance.baseJumpPower.FloatField(MasterDataModelConstants.Player.BaseJumpPowerLabel);
            }, () =>
            {
                _instance.baseMoveSpeed.FloatField(MasterDataModelConstants.Player.BaseMoveSpeedLabel);

                // ラベルがフィールドに隠れるので、ラベルの幅調整
                EditorGUIUtility.labelWidth = MasterDataModelConstants.Player.BaseAutoStaminaRecoveryQuantity.Length * 10;
                _instance.baseAutoStaminaRecoveryQuantity.IntField(
                    MasterDataModelConstants.Player.BaseAutoStaminaRecoveryQuantity);

                EditorGUIUtility.labelWidth = 0f;
            });
        }

        private void DrawAttackInformation()
        {
            EditorGUILayout.LabelField("攻撃情報", GUIStyles.PropertyHeaderStyle());

            EditorUtility.MultipleColumnArea(() =>
            {
                _instance.attackInterval.FloatField(MasterDataModelConstants.Player.AttackIntervalLabel);
            }, () =>
            {
                // ラベルがフィールドに隠れるので、ラベルの幅調整
                EditorGUIUtility.labelWidth = 240f;
                _instance.attackTargetFindRadius.FloatField(MasterDataModelConstants.Player.AttackTargetFindRadiusLabel);

                EditorGUIUtility.labelWidth = 0f;
            });
        }

        private void DrawDodgeInformation()
        {
            EditorGUILayout.LabelField("回避情報", GUIStyles.PropertyHeaderStyle());

            _instance.dodgeInterval.FloatField(MasterDataModelConstants.Player.DodgeIntervalLabel);

            EditorGUILayout.Space();

            EditorUtility.MultipleColumnArea(
                () => { DrawDodgeInformationField(MasterDataModelConstants.Player.NormalDodgeInfoLabel, _normalDodgeInfo); },
                () => { DrawDodgeInformationField(MasterDataModelConstants.Player.InAirDodgeInfoLabel, _inAirDodgeInfo); },
                () => { DrawDodgeInformationField(MasterDataModelConstants.Player.EscapingDodgeInfoLabel, _escapingDodgeInfo); });
        }

        /// <summary>
        /// <see cref="DodgeInformation"/>
        /// の各プロパティを描画する
        /// </summary>
        /// <param name="label">ラベルテキスト</param>
        /// <param name="property">対象のSerializedProperty</param>
        private static void DrawDodgeInformationField(string label, SerializedProperty property)
        {
            SerializedProperty motionSpeed         = property.FindPropertyRelative("dodgeSpeed");
            SerializedProperty requireMagicPower   = property.FindPropertyRelative("requireMagicPower");
            SerializedProperty requireStaminaPoint = property.FindPropertyRelative("requireStaminaPoint");
            SerializedProperty acquireMagicPower   = property.FindPropertyRelative("acquireMagicPower");
            SerializedProperty invincibilityFrame  = property.FindPropertyRelative("invincibilityFrame");
            SerializedProperty flinchFrame         = property.FindPropertyRelative("flinchFrame");

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            property.serializedObject.Update();

            motionSpeed.floatValue =
                EditorGUILayout.FloatField(MasterDataModelConstants.DodgeInfo.DodgeSpeedLabel, motionSpeed.floatValue);

            requireMagicPower.intValue =
                EditorGUILayout.IntField(MasterDataModelConstants.DodgeInfo.RequireMagicPowerLabel, requireMagicPower.intValue);

            requireStaminaPoint.intValue =
                EditorGUILayout.IntField(MasterDataModelConstants.DodgeInfo.RequireStaminaPointLabel,
                                         requireStaminaPoint.intValue);

            acquireMagicPower.intValue =
                EditorGUILayout.IntField(MasterDataModelConstants.DodgeInfo.AcquireMagicPowerLabel, acquireMagicPower.intValue);

            invincibilityFrame.intValue =
                EditorGUILayout.IntField(MasterDataModelConstants.DodgeInfo.InvincibilityFrameLabel, invincibilityFrame.intValue);

            flinchFrame.intValue =
                EditorGUILayout.IntField(MasterDataModelConstants.DodgeInfo.FlinchFrameLabel, flinchFrame.intValue);

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
