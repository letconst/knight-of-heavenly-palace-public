using Cysharp.Threading.Tasks;
using UnityEngine;

public static class CanvasGroupExtension
{
    public static async UniTask ToggleFade(this CanvasGroup target, bool isShow, float fadeTime)
    {
        System.Func<bool> breakCond = isShow
                                          ? () => target.alpha < 1
                                          : () => target.alpha > 0;

        float fadingTime = 0f;

        while (breakCond())
        {
            await UniTask.Yield();

            fadingTime += Time.deltaTime;
            target.alpha = isShow
                               ? fadingTime / fadeTime
                               : 1f - fadingTime / fadeTime;
        }
    }
}
