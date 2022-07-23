using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ComboBonusGenerator : EditorWindow {
    /*

// @""とすることで、複数行を書ける
// ただ「"」は「""」として書きます
private const string CODE = @"
using UnityEngine;
public class Sample : MonoBehaviour
{
public void Start() {
    Debug.Log(""End"");
}
}
";
private const string CODEUP = @"
using System.Collections.Generic;

public class ComboBonuData
{
    public string ComboName = ""Sample"";
    public JoyConAngleCheck.Position Input1 = JoyConAngleCheck.Position.Up;
    public JoyConAngleCheck.Position Input2 = JoyConAngleCheck.Position.Down;
    public JoyConAngleCheck.Position Input3 = JoyConAngleCheck.Position.Up;
    public JoyConAngleCheck.Position Input4 = JoyConAngleCheck.Position.Down;
}
public class ComboBonuDataBase
{
";
// 作成するアセットのパス
string filePath = "Assets/Users/Tomoi/Scripts/DataBase/ComboBonuDataBase.cs";

//時間ができたらEditor拡張作る

[SerializeField]
private List<ComboBonuData> ComboBonuPositionList ;

[MenuItem("Tools/ComboBonusを追加する")]
private static void Create()
{
    // 生成
    ComboBonusGenerator window = GetWindow<ComboBonusGenerator>("ComboBonusを追加する");
    
    // 最小サイズ設定
    window.minSize = new Vector2(320, 320);

}
//[SerializeField]
//private List<int> _someList;
private void OnGUI()
{
    // 自身のSerializedObjectを取得
    var so = new SerializedObject(this);

    so.Update();
    
    // 第二引数をtrueにしたPropertyFieldで描画
    EditorGUILayout.PropertyField(so.FindProperty("ComboBonuPositionList"), true);

    so.ApplyModifiedProperties();
    EditorGUILayout.PropertyField(so.FindProperty("ComboBonuPositionList"), true);

    so.Update();
    so.ApplyModifiedProperties();

    /*
    if (ComboBonuPositionList == null)
    {
        // 読み込み
        Import();
    }

    using (new GUILayout.HorizontalScope())
    {
        _sample.SampleIntValue = EditorGUILayout.IntField("サンプル", _sample.SampleIntValue);
    }
    if (GUILayout.Button("読み込み"))
    {
        Import();
    }
    using (new GUILayout.HorizontalScope())
    {
        // 書き込みボタン
        if (GUILayout.Button("書き込み"))
        {
            Export();
        }
    }
}


private static void Generate()
{

    
    // アセット(.cs)を作成する
    File.WriteAllText(filePath, CODE);
    
    // 変更があったアセットをインポートする(UnityEditorの更新)
    AssetDatabase.Refresh();
}

private void Export()
{
    // 読み込み
    ScriptableObjectSample sample = AssetDatabase.LoadAssetAtPath<ScriptableObjectSample>(ASSET_PATH);
    if (sample == null)
    {
        sample  = ScriptableObject.CreateInstance<ScriptableObjectSample>();
    }

    // 新規の場合は作成
    if (!AssetDatabase.Contains(sample as UnityEngine.Object))
    {
        string directory = Path.GetDirectoryName(ASSET_PATH);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        // アセット作成
        AssetDatabase.CreateAsset(sample, ASSET_PATH);
    }

    // コピー
    //sample.Copy(_sample);
    EditorUtility.CopySerialized(_sample, sample);

    // 直接編集できないようにする
    sample.hideFlags = HideFlags.NotEditable;
    // 更新通知
    EditorUtility.SetDirty(sample);
    // 保存
    AssetDatabase.SaveAssets();
    // エディタを最新の状態にする
    AssetDatabase.Refresh();
}
private void Import()
{
    if (_sample == null)
    {
        _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();
    }

    ScriptableObjectSample sample = AssetDatabase.LoadAssetAtPath<ScriptableObjectSample>(ASSET_PATH);
    if (sample == null)
        return;

    // コピーする
    //_sample.Copy(sample);
    EditorUtility.CopySerialized(sample, _sample);
}
    }
*/
}