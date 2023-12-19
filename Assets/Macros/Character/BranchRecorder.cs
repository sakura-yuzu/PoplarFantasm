using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using ScenarioFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;
using UnityEngine;

public class BranchRecorder : IReflectable
{
	//フラグ書き込み
	private readonly IChannelSender channelSender;
	//選択を求める
	private readonly IBranchSelector branchSelector;

	public BranchRecorder(IChannelSender channelSender, IBranchSelector branchSelector)
	{
		this.channelSender = channelSender ?? throw new ArgumentNullException(nameof(channelSender));
		this.branchSelector = branchSelector ?? throw new ArgumentNullException(nameof(branchSelector));
	}

	[ScenarioMethod("branch.multi", "選択肢をプレイヤーに尋ねる\nLet the player select the choices.")]
	public async UniTask AskChoiceAsync(string channelName, CancellationToken cancellationToken, params string[] choices)
	{
		//選択肢から一つ選ばせる
		var branchNumber = await branchSelector.SelectBranchAsync(choices, cancellationToken);
		//フラグを書き込む
		channelSender.Send(channelName, branchNumber);
	}

	[ScenarioMethod("branch.single", "一つの決められた選択肢をプレイヤーに選ばせる\nLet the player select the predefined choice.")]
	public async UniTask AskPredefinedChoiceAsync(string choice, CancellationToken cancellationToken)
	{
		await branchSelector.SelectBranchAsync(new string[] { choice }, cancellationToken);
	}

}