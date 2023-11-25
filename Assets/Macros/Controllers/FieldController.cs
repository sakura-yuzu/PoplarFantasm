using UnityEngine;
using UnityEngine.SceneManagement;
class FieldController : BaseController
{
	void Update(){
		if(Input.GetKeyDown("b")){
			Debug.Log('B');
			SceneManager.LoadSceneAsync("ButtleScene", LoadSceneMode.Additive);
		}
	}

}