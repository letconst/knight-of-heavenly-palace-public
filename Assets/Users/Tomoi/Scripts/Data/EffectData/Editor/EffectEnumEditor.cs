using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EffectEnumEditor : EditorWindow
{
    private Editor _editor;
    public EffectDataBase effectDataBase;

    private string EnumSavePath = "Assets/Users/Tomoi/Scripts/Data/EffectData/EffectType.cs";


    private Vector2 _scrollPosition = Vector2.zero;

    [MenuItem("Effect/EnumCreate")]
    static void Open()
    {
        var window = GetWindow<EffectEnumEditor>();
        window.titleContent = new GUIContent("Enumの生成");
    }

    /// <summary>
    /// enumを生成
    /// </summary>
    private void CreateEnum()
    {
        //enum化する文字列のみListで抽出
        List<string> enumDateList = new List<string>();
        foreach (var effectData in new List<EffectData>(effectDataBase.EffectDataList))
        {
            enumDateList.Add(effectData.EffectTypeString);
        }

        var s = Path.GetFullPath(EnumSavePath);
        EnumCreator.Create("EffectType", enumDateList, s);
        //enum生成後にEffectDataにenumをセットしたい

        for (var i = 0; i < effectDataBase.EffectDataList.Count; i++)
        {
            EffectType type = (EffectType)Enum.ToObject(typeof(EffectType), i);
            effectDataBase.EffectDataList[i].EffectType = type;
        }

        EditorUtility.SetDirty(effectDataBase);
    }


    /// <Summary>
    /// ウィンドウのパーツを表示します。
    /// </Summary>
    void OnGUI()
    {
        //enumを生成する処理
        if (GUILayout.Button("Enumを生成"))
        {
            if (effectDataBase != null)
            {
                CreateEnum();
            }
        }

        //EffectDataBaseの取得
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Enumを生成したいEffectDataBaseを追加");
        effectDataBase = (EffectDataBase)EditorGUILayout.ObjectField("", effectDataBase,
            typeof(EffectDataBase), true);

        //Editorの要素が更新されたかチェックする
        if (EditorGUI.EndChangeCheck())
        {
            if (effectDataBase != null)
            {
                //要素を更新
                _editor = Editor.CreateEditor(effectDataBase);
            }
        }

        EditorGUILayout.Space();
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));

        if (_editor != null)
        {
            //EffectDataBaseの要素の表示・変更処理
            var database = (EffectDataBase)_editor.target;
            //スクロールの処理
            EditorGUILayout.Space();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var effectData in database.EffectDataList)
            {
                effectData.EffectTypeString =
                    EditorGUILayout.TextField("Effect Type String", effectData.EffectTypeString);
                effectData.ParticlePlayer =
                    EditorGUILayout.ObjectField("Particle Player", effectData.ParticlePlayer, typeof(ParticlePlayer),
                            true)
                        as ParticlePlayer;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Effect Type");
                EditorGUILayout.LabelField(effectData.EffectType.ToString());
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            }

            Undo.RecordObject(database, "");

            //スクロールの処理
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();
            //

            //要素の追加と削除
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (effectDataBase != null)
                {
                    database.EffectDataList.Add(new EffectData());
                }
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("-", GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (effectDataBase != null)
                {
                    database.EffectDataList.RemoveAt(database.EffectDataList.Count - 1);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //
        }
    }
}