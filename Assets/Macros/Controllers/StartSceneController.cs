using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class StartSceneController : MonoBehaviour
{
	public Button newGameButton;
	public Button modalOKButton;
	public Button modalCancelButton;
	public Button continueGameButton;
	public Button hyperNewGameButton;
	public Button settingButton;
	public Canvas confirmCanvas;
	public GameObject SelectLanguage;
	public SaveData saveData;
	public SaveManager saveManager;

	void Awake(){
		saveManager = new SaveManager(saveData);
	}

	void Start(){
		confirmCanvas.enabled = false;
		VisibleLanguageSelect(false);

		newGameButton.onClick.AddListener(OnClickNewGameButton);
		modalOKButton.onClick.AddListener(OnClickModalOKButton);
		modalCancelButton.onClick.AddListener(OnClickModalCancelButton);
		settingButton.onClick.AddListener(OnClickSettingButton);

		if(saveManager.ExistsSaveFile()){
			continueGameButton.enabled = true;
			continueGameButton.onClick.AddListener(OnClickContinueGameButton);
		}else{
			// セーブファイルが無ければ「つづきから」を表示しない
			continueGameButton.enabled = false;
			continueGameButton.gameObject.SetActive(false);
		}
		
		// TODO: いろいろ未定
		hyperNewGameButton.enabled = false;
		hyperNewGameButton.gameObject.SetActive(false);
		// hyperNewGameButton.onClick.AddListener(OnClickHyperNewGameButton);
	}


	private void VisibleLanguageSelect(bool visible){
		SelectLanguage.transform.parent.gameObject.GetComponent<Canvas>().enabled = visible;
		SelectLanguage.GetComponent<LocaleSelector>().enabled = visible;
	}


	private void OnClickNewGameButton(){
		if(saveManager.ExistsSaveFile()){
			confirmCanvas.enabled = true;
		}else{
			SceneManager.LoadSceneAsync("InitialScene", LoadSceneMode.Single);
		}
	}

	private void OnClickContinueGameButton(){
		SceneManager.LoadSceneAsync("FieldScene", LoadSceneMode.Single);
	}

	private void OnClickSettingButton(){
		PlayerLanguageSetting();
	}

	private async UniTask<string> PlayerLanguageSetting(){
		VisibleLanguageSelect(true);
		LocaleSelector localeSelector = SelectLanguage.GetComponent<LocaleSelector>();
		string language = await localeSelector.selectAsync();
		VisibleLanguageSelect(false);
		Debug.Log(language);
		return language;
	}

	private void OnClickModalOKButton(){
		SceneManager.LoadSceneAsync("InitialScene", LoadSceneMode.Single);
	}

	private void OnClickModalCancelButton(){
		confirmCanvas.enabled = false;
	}
}