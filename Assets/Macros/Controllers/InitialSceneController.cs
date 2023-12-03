
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

	string targetSceneName = "FieldScene";

	void Start(){
		saveManager = new SaveManager(saveData);
	}

	//　すべての設定を終えたら遷移
	void Complete()
	{
		saveManager.Save();
		SceneManager.LoadScene(targetSceneName);
	}
}
