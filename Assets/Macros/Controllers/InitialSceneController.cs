
using Cysharp.Threading.Tasks;
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
	public GameObject SelectLanguage;
	public GameObject InputName;
	public GameObject SelectMode;
	public Canvas LoadingOverlay;
	private SaveManager saveManager;
	private string targetSceneName = "FieldScene";

	async void Start(){
		saveManager = new SaveManager(saveData);

		VisibleLanguageSelect(false);
		VisibleInputName(false);
		VisibleModeSelect(false);
		LoadingOverlay.enabled = false;

		await PlayerInitialSettings();

		LoadingOverlay.enabled = true;
		InitialSaveData initialSaveData = new InitialSaveData(saveData);

		await initialSaveData.createAsync(saveData.selectedMode);
		saveManager.Save();

		Complete();
	}

	public void VisibleLanguageSelect(bool visible){
		SelectLanguage.transform.parent.gameObject.GetComponent<Canvas>().enabled = visible;
		SelectLanguage.GetComponent<LocaleSelector>().enabled = visible;
	}

	public void VisibleInputName(bool visible){
		InputName.transform.parent.gameObject.GetComponent<Canvas>().enabled = visible;
		InputName.GetComponent<PlayerNameSettings>().enabled = visible;
	}

	public void VisibleModeSelect(bool visible){
		SelectMode.transform.parent.gameObject.GetComponent<Canvas>().enabled = visible;
		SelectMode.GetComponent<ModeSelector>().enabled = visible;
	}

	public async UniTask PlayerInitialSettings(){
		saveData.selectedLocale = await PlayerLanguageSetting();
		saveData.playerName = await PlayerNameSetting();
		saveData.selectedMode = await PlayerPlayModeSetting();
	}

	private async UniTask<string> PlayerLanguageSetting(){
		VisibleLanguageSelect(true);
		LocaleSelector localeSelector = SelectLanguage.GetComponent<LocaleSelector>();
		string language = await localeSelector.selectAsync();
		VisibleLanguageSelect(false);
		Debug.Log(language);
		return language;
	}

	private async UniTask<string> PlayerNameSetting(){
		VisibleInputName(true);
		PlayerNameSettings playerNameSettings = InputName.GetComponent<PlayerNameSettings>();
		string playerName = await playerNameSettings.DecideNameAsync();
		VisibleInputName(false);
		Debug.Log(playerName);
		return playerName;
	}

	private async UniTask<string> PlayerPlayModeSetting(){
		VisibleModeSelect(true);
		ModeSelector selector = SelectMode.GetComponent<ModeSelector>();
		string selectedMode = await selector.selectAsync();
		VisibleModeSelect(false);
		Debug.Log(selectedMode);
		return selectedMode;
	}

	//　すべての設定を終えたら遷移
	void Complete()
	{
		SceneManager.LoadScene(targetSceneName);
	}
}
