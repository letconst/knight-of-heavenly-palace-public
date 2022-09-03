#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CustomEditorExtension
{
    /// <summary>
    /// <see cref="EditorGUILayout.IntField(int,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void IntField(ref this int value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.IntField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label)); // ツールチップ表示用
    }

    /// <summary>
    /// <see cref="EditorGUILayout.IntField(int,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void IntField(ref this int value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.IntField(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.LongField(long,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void LongField(ref this long value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.LongField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.LongField(long,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void LongField(ref this long value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.LongField(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.FloatField(float,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void FloatField(ref this float value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.FloatField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.FloatField(float,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void FloatField(ref this float value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.FloatField(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.DoubleField(double,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void DoubleField(ref this double value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.DoubleField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.DoubleField(double,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void DoubleField(ref this double value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.DoubleField(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.IntSlider(int,int,int,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void IntSlider(ref this int value, string label, int leftValue, int rightValue,
                                 params GUILayoutOption[] options)
    {
        value = EditorGUILayout.IntSlider(label, value, leftValue, rightValue, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Slider(float,float,float,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Slider(ref this float value, string label, int leftValue, int rightValue,
                              params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Slider(label, value, leftValue, rightValue, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Popup(int,string[],UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Popup(ref this int value, string label, string[] displayedOptions, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Popup(label, value, displayedOptions, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Popup(int,string[],UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Popup(ref this int value, string label, string[] displayedOptions, GUIStyle style,
                             params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Popup(label, value, displayedOptions, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.IntPopup(int,string[],int[],UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void IntPopup(ref this int value, string label, string[] displayedOptions, int[] optionValues,
                                params GUILayoutOption[] options)
    {
        value = EditorGUILayout.IntPopup(label, value, displayedOptions, optionValues, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.IntPopup(int,string[],int[],UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void IntPopup(ref this int value, string label, string[] displayedOptions, int[] optionValues, GUIStyle style,
                                params GUILayoutOption[] options)
    {
        value = EditorGUILayout.IntPopup(label, value, displayedOptions, optionValues, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.MaskField(UnityEngine.GUIContent,int,string[],UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void MaskField(ref this int value, string label, string[] displayedOptions, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.MaskField(label, value, displayedOptions, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.MaskField(UnityEngine.GUIContent,int,string[],UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void MaskField(ref this int value, string label, string[] displayedOptions, GUIStyle style,
                                 params GUILayoutOption[] options)
    {
        value = EditorGUILayout.MaskField(label, value, displayedOptions, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.LayerField(int,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void LayerField(ref this int value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.LayerField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.LayerField(int,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void LayerField(ref this int value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.LayerField(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Vector2Field(string,UnityEngine.Vector2,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Vector2Field(ref this Vector2 value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Vector2Field(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Vector2IntField(string,UnityEngine.Vector2Int,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Vector2IntField(ref this Vector2Int value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Vector2IntField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Vector3Field(string,UnityEngine.Vector3,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Vector3Field(ref this Vector3 value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Vector3Field(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Vector3IntField(string,UnityEngine.Vector3Int,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Vector3IntField(ref this Vector3Int value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Vector3IntField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Vector4Field(string,UnityEngine.Vector4,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Vector4Field(ref this Vector4 value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Vector4Field(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.BoundsField(UnityEngine.Bounds,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void BoundsField(ref this Bounds value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.BoundsField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.BoundsIntField(UnityEngine.BoundsInt,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void BoundsIntField(ref this BoundsInt value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.BoundsIntField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.ColorField(UnityEngine.Color,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void ColorField(ref this Color value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.ColorField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.RectField(UnityEngine.Rect,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void RectField(ref this Rect value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.RectField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.RectIntField(UnityEngine.RectInt,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void RectIntField(ref this RectInt value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.RectIntField(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Toggle(bool,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Toggle(ref this bool value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Toggle(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.Toggle(bool,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void Toggle(ref this bool value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.Toggle(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.ToggleLeft(string,bool,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void ToggleLeft(ref this bool value, string label, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.ToggleLeft(label, value, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }

    /// <summary>
    /// <see cref="EditorGUILayout.ToggleLeft(string,bool,UnityEngine.GUIStyle,UnityEngine.GUILayoutOption[])"/>
    /// の拡張メソッド版
    /// </summary>
    public static void ToggleLeft(ref this bool value, string label, GUIStyle style, params GUILayoutOption[] options)
    {
        value = EditorGUILayout.ToggleLeft(label, value, style, options);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", label));
    }
}
#endif
