using UnityEditor;
using UnityEngine;

namespace KOHP.MasterData
{
    [System.Serializable]
    public sealed class EnemyAttackAction
    {
        [SerializeField, Header("行動名")]
        private string actionName;

        [SerializeField, Header("与えるダメージ")]
        private int damage;

        [SerializeField, Header("予備動作 (f)")]
        private int prepareDuration;

        [SerializeField, Header("モーション時間 (f)")]
        private int motionDuration;

        [SerializeField, Header("攻撃判定時間 (f)")]
        private int attackingDuration;

        public string ActionName => actionName;

        public int ResultDamage => damage;

        public int ResultPrepareDuration => prepareDuration;

        public int ResultMotionDuration => motionDuration;

        public int ResultAttackingDuration => attackingDuration;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnemyAttackAction))]
    public sealed class EnemyAttackActionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // string a = EditorGUILayout.TextField("行動名", "");
            SerializedProperty actionName = property.FindPropertyRelative("actionName");
            EditorGUI.LabelField(position, "行動名");
            actionName.stringValue =
                EditorGUI.TextField(new Rect(EditorGUIUtility.labelWidth, position.y,
                                             position.width - EditorGUIUtility.labelWidth,
                                             EditorGUIUtility.singleLineHeight), actionName.stringValue);
            // actionName.stringValue = a;
        }
    }
#endif
}
