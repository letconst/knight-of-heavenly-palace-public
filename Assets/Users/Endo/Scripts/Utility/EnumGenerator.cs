#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class EnumGenerator
{
    private static readonly string Tab = new(' ', 4);

    public static string GenerateSyntax(string enumName, IEnumerable<string> members)
    {
        var sb = new StringBuilder();

        sb.Append("public enum ")
          .Append(enumName)
          .Append("\n{\n");

        foreach (string member in members)
        {
            sb.Append(Tab)
              .Append(member)
              .Append(",\n");
        }

        sb.Append("}\n");

        return sb.ToString();
    }

    public static bool Generate(string enumName, IEnumerable<string> members, string savePath, bool ignoreOverrideWarning = false)
    {
        // 保存先に同じ名前のenumがある場合は上書きするか確認
        if (!ignoreOverrideWarning && File.Exists(savePath))
        {
            bool isOverride = EditorUtility.DisplayDialog("警告", "指定したディレクトリに同じ名前のenumが存在します。上書きしますか？",
                                                          "はい", "いいえ");

            if (!isOverride)
                return false;
        }

        DirectoryInfo saveDir = Directory.GetParent(savePath);

        if (saveDir == null)
        {
            Debug.LogError("保存先の情報を取得できませんでした");

            return false;
        }

        // 保存先が存在しなければ作成
        if (!saveDir.Exists)
        {
            Directory.CreateDirectory(saveDir.FullName);
        }

        string syntax = GenerateSyntax(enumName, members);

        File.WriteAllText(savePath, syntax);
        AssetDatabase.Refresh();

        return true;
    }
}
#endif
