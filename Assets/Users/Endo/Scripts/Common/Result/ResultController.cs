﻿using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public sealed class ResultController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text clearTimeText;

    [SerializeField]
    private Animation stampAnim;

    [SerializeField]
    private Image contentImage;

    private async void Start()
    {
        var dataPack = (ToResultSceneDataPack) FadeContllor2.PreviousSceneData;

        if (dataPack != null)
        {
            clearTimeText.text = dataPack.time.ToString("0.0");
        }

        // 依頼に応じた内容を表示
        contentImage.sprite = dataPack.AcceptedMission.ContentImage;

        await UniTask.Delay(TimeSpan.FromSeconds(1));

        stampAnim.Play();

        //     canvasGroup.alpha = 0f;
        //
        //     await UniTask.Delay(TimeSpan.FromSeconds(.5));
        //
        //     while (canvasGroup.alpha < 1f)
        //     {
        //         canvasGroup.alpha += Time.deltaTime;
        //
        //         await UniTask.Yield();
        //     }

        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
            .Take(1)
            .Subscribe(_ => FadeContllor2.Instance.LoadScene(1f, GameScene.Lobby))
            .AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.B))
            .Take(1)
            .Subscribe(_ => FadeContllor2.Instance.LoadScene(1f, GameScene.Title))
            .AddTo(this);
    }
}
