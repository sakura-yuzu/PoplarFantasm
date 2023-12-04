using System.Threading.Tasks;
using UnityEngine;
class InitialSaveData : MonoBehaviour
{
	private SaveData saveData;

	public InitialSaveData(SaveData _saveData){
		saveData = _saveData;
	}

	public async Task<bool> createAsync(string mode){
		switch(mode){
			case "storymode":
				await createStoryModeData();
				break;
			case "peacemode":
			  await createPeaceModeData();
				break;
			case "strongmode":
			  await createStrongModeData();
				break;
			default:
			  // ERROR!!
				break;
		}
		return true;
	}

  // TODO: 特に非同期処理になっていない
	// TODO: テンプレート用のScriptableObjectとか作って値コピーできればその方がよい
	private async Task<bool> createStoryModeData(){
		Debug.Log("createStoryModeData");
		saveData.selectedMode = "storymode";
		return true;
	}
	private async Task<bool> createPeaceModeData(){
		Debug.Log("createPeaceModeData");
		saveData.selectedMode = "peacemode";
		return true;
	}
	private async Task<bool> createStrongModeData(){
		Debug.Log("createStrongModeData");
		saveData.selectedMode = "strongmode";
		saveData.attackPower = 999;
		saveData.defensePower = 999;
		return true;
	}

}