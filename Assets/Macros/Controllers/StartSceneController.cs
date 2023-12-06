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
	public Canvas confirmCanvas;
	public SaveData saveData;
	public SaveManager saveManager;

	void Awake(){
		saveManager = new SaveManager(saveData);
	}

	void Start(){
		confirmCanvas.enabled = false;
		newGameButton.onClick.AddListener(OnClickNewGameButton);
		modalOKButton.onClick.AddListener(OnClickModalOKButton);
		modalCancelButton.onClick.AddListener(OnClickModalCancelButton);

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

	private void OnClickModalOKButton(){
		SceneManager.LoadSceneAsync("InitialScene", LoadSceneMode.Single);
	}

	private void OnClickModalCancelButton(){
		confirmCanvas.enabled = false;
	}
}