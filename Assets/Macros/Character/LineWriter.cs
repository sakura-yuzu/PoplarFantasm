using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScenarioFlow;
using System;
using System.Linq;
using System.Threading;
using TMPro;

[ScenarioMethod("line")]
public class LineWriter : IReflectable
{
    //キャラクターの名前を表示するテキスト
    private readonly TextMeshProUGUI textActorName;
    //キャラクターのセリフを表示するテキスト
    private readonly TextMeshProUGUI textLine;
    //1文字ごとの表示間隔
    private readonly float characterInterval;
    //1文字の表示にかける時間
    private readonly float characterDuration;
    //1文字の透明度の変化のさせ方
    private readonly Ease characterEase;
    //テキストを一斉に表示/非表示させる時にかける時間
    private readonly float textDuration;
    //テキストを一斉に表示/非表示させる時の透明度を変化させる方法
    private readonly Ease textEase;

    public LineWriter(Settings settings)
    {
        this.textActorName = settings.TextActorName ?? throw new ArgumentNullException(nameof(settings.TextActorName));
        this.textLine = settings.TextLine ?? throw new ArgumentNullException(nameof(settings.TextLine));
        this.characterInterval = settings.CharacterInterval;
        this.characterDuration = settings.CharacterDuration;
        this.characterEase = settings.CharacterEase;
        this.textDuration = settings.TextDuration;
        this.textEase = settings.TextEase;
    }

    [ScenarioMethod("write", "キャラクターのセリフを表示する")]
    public async UniTask WriteLineAsync(string actorName, string line, CancellationToken cancellationToken)
    {
        try
        {
            //前のキャラクターと異なるか
            var isDifferentActor = textActorName.text != actorName;

            //セリフを消す
            //前のセリフと異なるキャラクターなら名前も消す
            await UniTask.WhenAll(
                isDifferentActor ? SetTextAlphaAsync(textActorName, 0.0f, cancellationToken) : UniTask.CompletedTask,
                SetTextAlphaAsync(textLine, 0.0f, cancellationToken));

            //セリフとキャラクターの名前を設定
            textActorName.text = actorName;
            textLine.text = line;

            //セリフを表示
            //前のセリフと異なるキャラクターなら名前も表示
            await UniTask.WhenAll(
                isDifferentActor ? SetTextAlphaAsync(textActorName, 1.0f, cancellationToken) : UniTask.CompletedTask,
                VisualizeTextInOrderAsync(textLine, cancellationToken));
        }
        finally
        {
            //最終的なテキストの状態
            textActorName.alpha = 1.0f;
            textActorName.text = actorName;
            textLine.alpha = 1.0f;
            textLine.text = line;
        }
    }

    [ScenarioMethod("erase", "キャラクターのセリフを非表示にする")]
    public async UniTask EraseLineAsync(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.WhenAll(
                SetTextAlphaAsync(textActorName, 0.0f, cancellationToken),
                SetTextAlphaAsync(textLine, 0.0f, cancellationToken));
        }
        finally
        {
            //最終的なテキストの状態
            textActorName.alpha = 0.0f;
            textLine.alpha = 0.0f;
        }
    }

    //テキストを順に表示する
    private async UniTask VisualizeTextInOrderAsync(TextMeshProUGUI textMeshProUGUI, CancellationToken cancellationToken)
    {
        textMeshProUGUI.ForceMeshUpdate();

        var textInfo = textMeshProUGUI.textInfo;

        foreach(var charIndex in Enumerable.Range(0, textInfo.characterCount))
        {
            //文字のColor情報を取得
            var characterInfo = textInfo.characterInfo[charIndex];
            var materialIndex = characterInfo.materialReferenceIndex;
            var vertexIndex = characterInfo.vertexIndex;
            var colors32 = textInfo.meshInfo[materialIndex].colors32;

            //透明度を徐々に変える
            await DOTween.ToAlpha(
                () => characterInfo.color,
                color =>
                {
                    colors32[vertexIndex] = color;
                    colors32[vertexIndex + 1] = color;
                    colors32[vertexIndex + 2] = color;
                    colors32[vertexIndex + 3] = color;
                    textMeshProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                },
                1.0f,
                characterDuration).SetEase(characterEase).ToUniTask(cancellationToken: cancellationToken);
            //少し待つ
            await UniTask.Delay(TimeSpan.FromSeconds(characterInterval), cancellationToken: cancellationToken);
        }
    }

    //テキストの透明度を徐々に変える
    private async UniTask SetTextAlphaAsync(TextMeshProUGUI textMeshProUGUI, float alpha, CancellationToken cancellationToken)
    {
        await textMeshProUGUI.DOFade(alpha, textDuration).SetEase(textEase).ToUniTask(cancellationToken: cancellationToken);
    }

    [Serializable]
    public class Settings
    {
        public TextMeshProUGUI TextActorName;
        public TextMeshProUGUI TextLine;
        public float CharacterInterval;
        public float CharacterDuration;
        public Ease CharacterEase;
        public float TextDuration;
        public Ease TextEase;
    }
}