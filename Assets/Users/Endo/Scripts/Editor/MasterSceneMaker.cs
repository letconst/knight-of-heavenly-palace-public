#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterSceneMaker : EditorWindow
{
    private Vector2 _scrollPos;
    private bool    _isToggledCopyScene;
    private string  _inputSourceScenePath;
    private string  _inputSceneName;
    private int     _inputNewSceneMode;

    private const           string SceneSavePathName   = "/Scenes";
    private static readonly string SceneSavePathFormat = $"Assets{SceneSavePathName}/{{0}}.unity";
    private const           string SceneEnumName       = "GameScene";
    private static readonly string SceneEnumSavePath   = $"Assets/Users/Endo/Scripts/Scene/{SceneEnumName}.cs";

    [MenuItem("KOHP Tools/MasterSceneMaker")]
    private static void ShowWindow()
    {
        CreateWindow<MasterSceneMaker>();
    }

    private void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUI.skin.scrollView);
        EditorGUILayout.BeginVertical(GUI.skin.box);

        _inputSceneName = EditorGUILayout.TextField("シーン名", _inputSceneName);
        _inputNewSceneMode = EditorGUILayout.Popup("作成後の動作", _inputNewSceneMode,
                                                   new[] { "シーンを開く", "シーンを追加で開く" });

        _isToggledCopyScene = EditorGUILayout.BeginToggleGroup("他のシーンからコピーする", _isToggledCopyScene);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("シーンを選択", GUILayout.Width(80)))
        {
            OnClickSourceSceneSelect(out _inputSourceScenePath);
        }

        _inputSourceScenePath = EditorGUILayout.TextField(_inputSourceScenePath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("作成", GUILayout.Width(80), GUILayout.Height(25)))
        {
            OnClickSceneCreate(_inputSceneName, _isToggledCopyScene, _inputSourceScenePath,
                               (NewSceneMode) _inputNewSceneMode);
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private static void OnClickSourceSceneSelect(out string inputSourceScenePath)
    {
        inputSourceScenePath = EditorUtility.OpenFilePanelWithFilters("シーンを選択", Application.dataPath,
                                                                      new[] { "unity", "unity" });

        // シーンパス入力欄にフォーカスがあると更新されないので、強制的に外す
        GUI.FocusControl("");
    }

    private static void OnClickSceneCreate(string       newSceneName, bool isToggledCopyScene, string sourceScenePath,
                                           NewSceneMode newSceneMode)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogError("シーン名が入力されていません");
        }
        else
        {
            if (isToggledCopyScene)
            {
                bool copySuccessfully = CopyScene(newSceneName, sourceScenePath, newSceneMode);

                // シーンがコピーできたら登録処理
                if (copySuccessfully)
                {
                    SetupScene(newSceneName);
                }
                else
                {
                    Debug.Log($"シーン「{newSceneName}」はコピーされませんでした");
                }
            }
            else
            {
                bool createSuccessfully = CreateScene(newSceneName, newSceneMode);

                // シーンが作成できたら登録処理
                if (createSuccessfully)
                {
                    SetupScene(newSceneName);
                }
                else
                {
                    Debug.Log($"シーン「{newSceneName}」は作成されませんでした");
                }
            }
        }
    }

    /// <summary>
    /// 入力した名前のシーンを作成する
    /// </summary>
    /// <param name="sceneName">作成するシーン名</param>
    /// <param name="newSceneMode">シーンの作成モード</param>
    /// <returns>作成できたか</returns>
    private static bool CreateScene(string sceneName, NewSceneMode newSceneMode)
    {
        // 開いていたシーン名を記憶
        string prevSceneName = SceneManager.GetActiveScene().name;

        // シーンを作成
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, newSceneMode);
        newScene.name = sceneName;

        // 保存先が存在しなければ作成
        if (!Directory.Exists(SceneSavePathName))
        {
            Directory.CreateDirectory(SceneSavePathName);
        }

        string savePath = string.Format(SceneSavePathFormat, sceneName);

        // シーンがすでに存在していたら警告
        if (File.Exists(savePath))
        {
            bool isOverride = EditorUtility.DisplayDialog("警告", $"シーン「{sceneName}」はすでに存在します。上書きしますか？",
                                                          "はい", "いいえ");

            // キャンセルなら直前に開いていたシーンに戻して終了
            if (!isOverride)
            {
                EditorSceneManager.OpenScene(string.Format(SceneSavePathFormat, prevSceneName));

                return false;
            }
        }

        // シーンを保存
        return EditorSceneManager.SaveScene(newScene, savePath);
    }

    /// <summary>
    /// 入力した名前でシーンをコピーする
    /// </summary>
    /// <param name="newSceneName">作成するシーン名</param>
    /// <param name="sourcePath">コピー元のシーンのパス</param>
    /// <param name="newSceneMode">シーンの作成モード</param>
    /// <returns>コピーできたか</returns>
    private static bool CopyScene(string newSceneName, string sourcePath, NewSceneMode newSceneMode)
    {
        if (!File.Exists(sourcePath))
        {
            Debug.LogError($"以下のパスは存在しないため、コピーシーンを作成できません\n{sourcePath}");

            return false;
        }

        if (!Directory.Exists(SceneSavePathName))
        {
            Directory.CreateDirectory(SceneSavePathName);
        }

        string savePath = string.Format(SceneSavePathFormat, newSceneName);

        if (File.Exists(savePath))
        {
            bool isOverride = EditorUtility.DisplayDialog("警告", $"シーン「{newSceneName}」はすでに存在します。上書きしますか？",
                                                          "はい", "いいえ");

            if (!isOverride)
                return false;
        }

        // シーンを複製して開く
        FileUtil.ReplaceFile(sourcePath, savePath);
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(savePath, (OpenSceneMode) newSceneMode);

        return true;
    }

    /// <summary>
    /// 作成されたシーンの登録処理を行う
    /// </summary>
    /// <param name="newSceneName">作成したシーン名</param>
    private static void SetupScene(string newSceneName)
    {
        // ビルド対象に設定
        RegisterSceneToBuildTarget(newSceneName);

        List<string> allBuildScenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();

        // ↑はパスで取得されるので、ファイル名のみに削る
        for (int i = 0; i < allBuildScenes.Count; i++)
        {
            string[] scenePath = allBuildScenes[i].Split('/');
            string   sceneName = Path.GetFileNameWithoutExtension(scenePath[^1]);
            allBuildScenes[i] = sceneName;
        }

        bool generateSuccessfully = EnumGenerator.Generate(SceneEnumName, allBuildScenes, SceneEnumSavePath);

        if (!generateSuccessfully)
        {
            Debug.Log($"シーン「{newSceneName}」の作成に成功しましたが、enumファイルは生成されませんでした");
        }
    }

    /// <summary>
    /// 指定のシーンをビルド対象に設定する
    /// </summary>
    /// <param name="sceneName">登録するシーン名</param>
    private static void RegisterSceneToBuildTarget(string sceneName)
    {
        List<EditorBuildSettingsScene> resultScenes = new(EditorBuildSettings.scenes);
        string                         newScenePath = string.Format(SceneSavePathFormat, sceneName);

        // すでにビルド対象なら終了
        if (resultScenes.Any(scene => scene.path.Equals(newScenePath)))
            return;

        resultScenes.Add(new EditorBuildSettingsScene(newScenePath, true));
        EditorBuildSettings.scenes = resultScenes.ToArray();
    }
}
#endif
