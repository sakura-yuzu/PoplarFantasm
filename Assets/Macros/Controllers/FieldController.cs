using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class FieldController : BaseController
{
	public Canvas MainMenu;
	public Button saveButton;

	void Start(){
		saveButton.onClick.AddListener(base.Save);
	}

	void Update(){
		if(Input.GetKeyDown("q")){
			Debug.Log('Q');
			MainMenu.enabled = !MainMenu.enabled;
		}
	}

}