using ScenarioFlow;
using ScenarioFlow.TaskFlow;
using System;
using UnityEngine;

public class BranchMaker : IReflectable
{
    //フラグ読み込み
    private readonly IChannelReceiver channelReceiver;
    //分岐実行
    private readonly ILabelOpener labelOpener;

    public BranchMaker(IChannelReceiver channelReceiver, ILabelOpener labelOpener)
    {
        this.channelReceiver = channelReceiver ?? throw new ArgumentNullException(nameof(channelReceiver));
        this.labelOpener = labelOpener ?? throw new ArgumentNullException(nameof(labelOpener));
    }

    [ScenarioMethod("label.open", "ScenarioBookにおける指定のラベルへジャンプ\nJump to the label in the ScenarioBook.")]
    public void OpenLabel(string labelName)
    {
        labelOpener.OpenLabel(labelName);
    }

    [ScenarioMethod("label.branch", "チャンネルの値が結び付けられているラベルへジャンプ\nJump to the label bound to the channel value.")]
    public void OpenSelectedLabel(string channelName, params string[] labelNames)
    {
        Debug.Log("めじるし");
        Debug.Log(channelName);
        //分岐先のラベルが0でないか
        if (labelNames.Length == 0)
        {
            throw new ArgumentException("No label names are passed.");
        }
        //分岐先
        var channelValue = channelReceiver.Receive(channelName);
        //適切な値か確認
        if (0 <= channelValue && channelValue < labelNames.Length)
        {
            //分岐
            labelOpener.OpenLabel(labelNames[channelValue]);
        }
        else
        {
            throw new InvalidOperationException($"The channel value '{channelValue}' is invalid.");
        }
    }
}