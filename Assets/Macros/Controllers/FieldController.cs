using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class FieldController : BaseController
{
	public GameObject MainMenu;
	public Canvas CharacterSpeakArea;
	public Button saveButton;
	public Button exitButton;

	public Transform missionTable;

	private string test="piyo";

	public MissionDatabase missionDatabase;

	private bool menuOpen = false;

	void Start(){
		Debug.Log("Start");
		Debug.Log(test);
		// マップの読み込み

		// NPCの設定
		// PCの設定
		MainMenu.SetActive(false);
		CharacterSpeakArea.enabled = false;
		saveButton.onClick.AddListener(base.Save);
		exitButton.onClick.AddListener(Exit);
		new MissionList(missionTable, missionDatabase, Mission.Status.Received);
		test = "hoge";
	}

	void Update(){
		if(Input.GetKeyDown("q")){
			Debug.Log('Q');
			menuOpen = !menuOpen;
			MainMenu.SetActive(menuOpen);
		}
		if(Input.GetKeyDown("e")){
			SceneManager.LoadSceneAsync("FieldScene");
		}
	}

	public void Exit(){
		// 条件付きコンパイル
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
	}

}