using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScenarioFlow;
using System;
using System.Threading;
using UnityEngine;

[ScenarioMethod("bg")]
public class BackgroundAnimator : IReflectable
{
    //前面背景
    private readonly SpriteRenderer frontRenderer;
    //背面背景
    private readonly SpriteRenderer backRenderer;
    //背景の変更にかける時間
    private float changeDuration = 0.0f;
    //背景の遷移の仕方
    private Ease changeEase = Ease.Linear;

    public BackgroundAnimator(Settings settings)
    {
        this.frontRenderer = settings.FrontRenderer ?? throw new ArgumentNullException(nameof(settings.FrontRenderer));
        this.backRenderer = settings.BackRenderer ?? throw new ArgumentNullException(nameof(settings.BackRenderer));
        //frontRendererがbackRendererよりも後ろにあることを許さない
        if (this.frontRenderer.sortingOrder <= this.backRenderer.sortingOrder)
            throw new ArgumentException("The front sorting order must be more than the back sorting order.");
    }

    [ScenarioMethod("change.immed", "背景を即座に変更\nChange the background immediately.")]
    public void ChangeBackgroundImmediately(Sprite sprite)
    {
        frontRenderer.sprite = sprite;
    }

    [ScenarioMethod("change.grad", "背景を徐々に変更\nChange the background gradually.")]
    public async UniTask ChangeBackgroundGraduallyAsync(Sprite sprite, CancellationToken cancellationToken)
    {
        //背面背景を変更
        backRenderer.sprite = sprite;
        try
        {
            //前面背景を徐々に透明に
            await frontRenderer.DOFade(0.0f, changeDuration).SetEase(changeEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            //前面背景を変更
            frontRenderer.sprite = sprite;
            //前面背景を表示
            frontRenderer.color = backRenderer.color;
        }

    }

    [ScenarioMethod("change.durat", "背景の変更にかける時間を設定\nSetup the time to change the background.")]
    public void SetupChangeDuration(float duration)
    {
        if (duration < 0.0f)
        {
            throw new ArgumentException("Duration must not be negative number.");
        }

        changeDuration = duration;
    }

    [ScenarioMethod("change.ease", "背景の遷移をどのように行うかを設定\nSetup how to execute the background transition.")]
    public void SetupChangeEase(Ease ease)
    {
        changeEase = ease;
    }

    [Serializable]
    public class Settings
    {
        public SpriteRenderer FrontRenderer;
        public SpriteRenderer BackRenderer;
    }
}