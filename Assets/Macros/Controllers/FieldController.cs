using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class FieldController : BaseController
{
	public Canvas MainMenu;
	public Canvas CharacterSpeakArea;
	public Button saveButton;
	public Button exitButton;

	public Transform missionTable;

	public MissionDatabase missionDatabase;

	void Start(){
		MainMenu.enabled = false;
		CharacterSpeakArea.enabled = false;
		saveButton.onClick.AddListener(base.Save);
		exitButton.onClick.AddListener(Exit);
		new MissionList(missionTable, missionDatabase, Mission.Status.Received);
	}

	void Update(){
		if(Input.GetKeyDown("q")){
			// Debug.Log('Q');
			MainMenu.enabled = !MainMenu.enabled;
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