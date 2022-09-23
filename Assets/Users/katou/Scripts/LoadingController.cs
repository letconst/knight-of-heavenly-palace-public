using System.Collections;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    [SerializeField]
    private float MinWaitTime = 0f;
    // Start is called before the first frame update
    IEnumerator  Start()
    {
        yield return null;

        // 開放処理実行
        yield return Release();

        var DataPack = (ToLoadingSceneDataPack)FadeContllor2.PreviousSceneData;
        AsyncOperation operation = FadeContllor2.Instance.LoadSceneAsync(DataPack.NextScene.ToString());

        // 読み込み優先に
        Application.backgroundLoadingPriority = ThreadPriority.High;
#if UNITY_SWITCH && !UNITY_EDITOR
        // UnityEngine.Switch.Performance.SetCpuBoostMode(UnityEngine.Switch.Performance.CpuBoostMode.FastLoad);
#endif

        float StartTime = Time.time;
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 優先解除
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
#if UNITY_SWITCH && !UNITY_EDITOR
        // UnityEngine.Switch.Performance.SetCpuBoostMode(UnityEngine.Switch.Performance.CpuBoostMode.Normal);
#endif

        float EndTime =Time.time;
        while(EndTime - StartTime < MinWaitTime)
        {
            yield return null;
            EndTime = Time.time;
        }

        FadeContllor2.PreviousSceneData = DataPack.NextSceneDataPack;

        yield return FadeContllor2.Instance.FadeOutAsync(1f);
        FadeContllor2.Instance.FadeIn(1f);
        operation.allowSceneActivation = true;

    }

    private IEnumerator Release()
    {
        System.GC.Collect();
        yield return Resources.UnloadUnusedAssets();
    }
}
