using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;
class ToggleGroupBase : MonoBehaviour, IToggleGroup
{
	bool pointerClicked = false;

	public UniTask waitKeyDown(){
		return UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
	}
	public UniTask waitMouseClick(){
		return UniTask.WaitUntil(() => pointerClicked);
	}

	public bool OnPointerClick(){
		Debug.Log("OnPointerClick");
		pointerClicked = true;
		return true;
	}
}