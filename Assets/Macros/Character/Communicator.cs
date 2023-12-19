using Cysharp.Threading.Tasks;
using System.Threading;
using ScenarioFlow;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;

public class Communicator : IReflectable
{
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
    private TextMeshProUGUI characterSpeakArea;
    private TextMeshProUGUI characterNameArea;
    public Communicator(Settings settings){
        // characterSpeakArea = _characterSpeakArea.transform.GetChild(0).Find("SpeakArea").GetComponent<TextMeshProUGUI>();
        // characterNameArea = _characterSpeakArea.transform.GetChild(0).Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterSpeakArea = settings.characterSpeakArea;
        characterNameArea = settings.characterNameArea;
        this.characterInterval = settings.CharacterInterval;
        this.characterDuration = settings.CharacterDuration;
        this.characterEase = settings.CharacterEase;
        this.textDuration = settings.TextDuration;
        this.textEase = settings.TextEase;
    }

    [ScenarioMethod("talk")]
    public async UniTask Talk(string characterName, string message, CancellationToken cancellationToken)
    {
        // await UniTask.WhenAll(
        //     // isDifferentActor ? SetTextAlphaAsync(textActorName, 0.0f, cancellationToken) : UniTask.CompletedTask,
        //     SetTextAlphaAsync(characterSpeakArea, 0.0f, cancellationToken));

        characterNameArea.text = characterName;
        characterSpeakArea.text = message;
        Debug.Log(message);
        await UniTask.WhenAll(
            // isDifferentActor ? SetTextAlphaAsync(characterNameArea, 1.0f, cancellationToken) : UniTask.CompletedTask,
            VisualizeTextInOrderAsync(characterSpeakArea, cancellationToken));
    }
    [ScenarioMethod("shout")]
    public void Shout(string message)
    {
        Debug.Log(message);
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

    private async UniTask SetTextAlphaAsync(TextMeshProUGUI textMeshProUGUI, float alpha, CancellationToken cancellationToken)
    {
        await textMeshProUGUI.DOFade(alpha, textDuration).SetEase(textEase).ToUniTask(cancellationToken: cancellationToken);
    }

    [Serializable]
    public class Settings
    {
        public TextMeshProUGUI characterSpeakArea;
        public TextMeshProUGUI characterNameArea;
        public float CharacterInterval;
        public float CharacterDuration;
        public Ease CharacterEase;
        public float TextDuration;
        public Ease TextEase;
    }
}