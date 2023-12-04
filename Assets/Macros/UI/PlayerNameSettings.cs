using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;
class PlayerNameSettings : MonoBehaviour {

	public SaveData saveData;

	public Button decideButton;
	public TMP_InputField inputName;

	private AsyncUnityEventHandler buttonEvent;

	void Start(){
		buttonEvent = decideButton.onClick.GetAsyncEventHandler(CancellationToken.None);
	}

	//　ボタンが選択された時に実行
	public async UniTask<string> DecideNameAsync()
	{
		await buttonEvent.OnInvokeAsync();
		return inputName.GetComponent<TMP_InputField>().text;
	}
}