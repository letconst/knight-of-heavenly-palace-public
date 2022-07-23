using UnityEngine;

public static class GameInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 30 fps指定
        Application.targetFrameRate = 30;

        // オブジェクト自動生成
        GenerateObject<SwitchInputManager>();
        GenerateObject<MasterDataManager>();
        GenerateObject<FadeContllor2>();
        GenerateObject<SoundManager>();
    }

    private static void GenerateObject<T>() where T : Component
    {
        var obj = new GameObject(typeof(T).Name);
        obj.AddComponent<T>();
        Object.DontDestroyOnLoad(obj);
    }
}
