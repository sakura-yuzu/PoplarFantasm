using UnityEngine;
using UnityEngine.SceneManagement;
class ButtleController : BaseController
{
	void Update(){
		if(Input.GetKeyDown("q")){
			Debug.Log('Q');
			SceneManager.UnloadSceneAsync("ButtleScene");
		}
	}

}