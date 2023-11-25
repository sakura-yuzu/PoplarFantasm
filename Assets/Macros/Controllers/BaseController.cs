using UnityEngine;
using UnityEngine.SceneManagement;
class BaseController : MonoBehaviour
{
	public SaveData saveData;
	protected SaveManager saveManager;

	void Awake(){
		saveManager = new SaveManager(saveData);
		saveManager.Load();
	}

	public void Save(){
		saveManager.Save();
	}

}