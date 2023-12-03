using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;
class PlayerNameSettings : MonoBehaviour{

	public SaveData saveData;

	public Button decideButton;
	public TMP_InputField inputName;

	void Start(){
		decideButton.onClick.AddListener(DecideName);
	}

	//　ボタンが選択された時に実行
	void DecideName()
	{
		var name = inputName.GetComponent<TMP_InputField>().text;
		saveData.playerName = name;
	}
}