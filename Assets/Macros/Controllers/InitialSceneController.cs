
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InitialSceneController : MonoBehaviour
{
	public SaveData saveData;
	private SaveManager saveManager;

	public Button decideButton;
	public TMP_InputField inputName;

	string targetSceneName = "FieldScene";

	void Start(){
		saveManager = new SaveManager(saveData);
		decideButton.onClick.AddListener(DecideName);
	}

	//　ボタンが選択された時に実行
	void DecideName()
	{
		var name = inputName.GetComponent<TMP_InputField>().text;
		saveData.playerName = name;
		saveManager.Save();
		SceneManager.LoadScene(targetSceneName);
	}
}
