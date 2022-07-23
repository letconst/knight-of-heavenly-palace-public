using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class ObjectFader
{
    private static readonly List<Renderer> _fadingRenderers = new();

    /// <summary>
    /// オブジェクトをフェードアウトする
    /// </summary>
    /// <param name="renderer">対象のrenderer</param>
    /// <param name="fadeTime">フェード時間 (秒)</param>
    /// <param name="cancellationToken"></param>
    public static async UniTask FadeOut(Renderer renderer, float fadeTime, CancellationToken cancellationToken = default)
    {
        if (_fadingRenderers.Contains(renderer))
            return;

        await FadeInternal(renderer, fadeTime, cancellationToken, f => 1f - f / fadeTime);
    }

    /// <summary>
    /// オブジェクトをフェードインする
    /// </summary>
    /// <param name="renderer">対象のrenderer</param>
    /// <param name="fadeTime">フェード時間 (秒)</param>
    /// <param name="cancellationToken"></param>
    public static async UniTask FadeIn(Renderer renderer, float fadeTime, CancellationToken cancellationToken = default)
    {
        if (_fadingRenderers.Contains(renderer))
            return;

        await FadeInternal(renderer, fadeTime, cancellationToken, f => f / fadeTime);
    }

    private static async UniTask FadeInternal(Renderer renderer, float fadeTime, CancellationToken cancellationToken,
                                        System.Func<float, float> onAlphaCalc)
    {
        Material[]    materials = renderer.materials;
        List<UniTask> fadeTasks = new();

        // 全マテリアルに反映
        foreach (Material mat in materials)
        {
            SetupMaterial(mat);

            // フェードタスク作成
            UniTask fadeTask = UniTask.Defer(async () =>
            {
                float fadingTime = 0f;
                Color color      = mat.color;

                // フェード処理
                while (fadingTime < fadeTime)
                {
                    await UniTask.Yield(cancellationToken);

                    fadingTime += Time.deltaTime;
                    color.a    =  onAlphaCalc(fadingTime);
                    mat.SetColor("_BaseColor", color);
                }

                _fadingRenderers.Add(renderer);

                _fadingRenderers.Remove(renderer);
            });

            fadeTasks.Add(fadeTask);
        }

        await UniTask.WhenAll(fadeTasks);
    }

    private static void SetupMaterial(Material mat)
    {
        // レンダリングモード切り替え
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.renderQueue = (int) RenderQueue.Transparent;

        // 各種設定
        mat.SetFloat("_SrcBlend", (float) BlendMode.SrcAlpha);
        mat.SetFloat("_DstBlend", (float) BlendMode.OneMinusSrcAlpha);
        mat.SetFloat("_ZWrite", 0f);
        mat.SetFloat("_Surface", 1f);
        mat.EnableKeyword(ShaderKeywordStrings._SURFACE_TYPE_TRANSPARENT);
        mat.DisableKeyword(ShaderKeywordStrings._ALPHAPREMULTIPLY_ON);
        mat.DisableKeyword(ShaderKeywordStrings._ALPHAMODULATE_ON);
        mat.SetShaderPassEnabled("DepthOnly", false);
    }
}
