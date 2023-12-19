using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class ButtonBranchSelector : IBranchSelector
{
    //分岐用ボタン
    private BranchButton[] branchButtons;
    //表示にかける時間
    private float displayDuration;
    //表示のEase
    private Ease displayEase;
    //非表示にかける時間
    private float hideDuration;
    //非表示のEase
    private Ease hideEase;

    public ButtonBranchSelector(Settings settings)
    {
        this.branchButtons = settings.BranchButtons;
        this.displayDuration = settings.DisplayDuration;
        this.displayEase = settings.DisplayEase;
        this.hideDuration = settings.HideDuration;
        this.hideEase = settings.HideEase;
        //分岐用ボタンの数をチェック
        if (branchButtons.Length == 0)
        {
            throw new ArgumentException("No branch buttons are registered.");
        }
    }

    //選択をプレイヤーに求める
    public async UniTask<int> SelectBranchAsync(IEnumerable<string> choices, CancellationToken cancellationToken)
    {
        //選択肢
        var choiceArray = choices.ToArray();
        var choiceLength = choiceArray.Length;
        //選択肢の数をチェック
        if (choices.Count() <= 0 || branchButtons.Length < choices.Count())
        {
            throw new ArgumentException("The number of choices is invalid. It must be less than or equal to the number of branch buttons and at least one choice must be passed.");
        }
        //分岐ボタンオブジェクトをアクティブに
        foreach (var branch in branchButtons.Take(choiceLength))
        {
            branch.gameObject.SetActive(true);
        }
        //分岐テキストを設定
        foreach (var index in Enumerable.Range(0, choiceArray.Length))
        {
            branchButtons[index].ChoiceText.text = choiceArray[index];
        }
        try
        {
            //分岐ボタン表示
            await FadeBranchButtonsAsync(choiceLength, 1.0f, displayDuration, displayEase, cancellationToken);
            //分岐ボタンの有効化
            foreach (var branch in branchButtons.Take(choiceLength))
            {
                branch.ChoiceButton.interactable = true;
            }
            //分岐ボタンのどれかが押されるのを待つ
            var (winIndex, _) = await UniTask.WhenAny(
                branchButtons
                .Take(choiceLength)
                .Select(branch => branch.ChoiceButton.OnClickAsAsyncEnumerable().FirstOrDefaultAsync(cancellationToken: cancellationToken)));
            //分岐ボタン非表示
            await FadeBranchButtonsAsync(choiceLength, 0.0f, hideDuration, hideEase, cancellationToken);
            //初めに押された分岐ボタンの番号を返す
            return winIndex;
        }
        finally
        {
            //分岐ボタンを無効化
            foreach (var branch in branchButtons.Take(choiceLength))
            {
                branch.Initialize();
            }
        }
    }

    //分岐ボタンの透明度を変更
    private UniTask FadeBranchButtonsAsync(int count, float alpha, float duration, Ease ease, CancellationToken cancellationToken)
    {
        return UniTask.WhenAll(
            branchButtons
            .Take(count)
            .Select(branch => UniTask.WhenAll(
                branch.ChoiceImage.DOFade(alpha, duration).SetEase(ease).ToUniTask(cancellationToken: cancellationToken),
                branch.ChoiceText.DOFade(alpha, duration).SetEase(ease).ToUniTask(cancellationToken: cancellationToken))));
    }

    [Serializable]
    public class Settings
    {
        public BranchButton[] BranchButtons;
        public float DisplayDuration;
        public Ease DisplayEase;
        public float HideDuration;
        public Ease HideEase;
    }
}