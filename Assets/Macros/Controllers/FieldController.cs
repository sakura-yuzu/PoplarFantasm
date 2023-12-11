using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class FieldController : BaseController
{
	public Canvas MainMenu;
	public Button saveButton;
	public Button exitButton;

	void Start(){
		saveButton.onClick.AddListener(base.Save);
		exitButton.onClick.AddListener(Exit);
	}

	void Update(){
		if(Input.GetKeyDown("q")){
			Debug.Log('Q');
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