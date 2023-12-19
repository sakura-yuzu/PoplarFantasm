using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScenarioFlow;
using System;
using System.Threading;
using UnityEngine;

public class WindowCloser : IReflectable
{
	private GameObject characterSpeakArea;

	public WindowCloser(Settings settings){
		this.characterSpeakArea = settings.characterSpeakArea;
	}

	[ScenarioMethod("window.close")]
	public async UniTask CloseWindow(CancellationToken cancellationToken)
	{
		characterSpeakArea.GetComponent<Canvas>().enabled = false;
	}
		
	[Serializable]
	public class Settings
	{
			public GameObject characterSpeakArea;
    }
}