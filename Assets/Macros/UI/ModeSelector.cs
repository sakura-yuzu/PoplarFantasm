using Cysharp.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

using TMPro;

class ModeSelector : MonoBehaviour
{
	public UniTask waitKeyDown(){
		return UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
	}
	public UniTask waitMouseClick(){
		return UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
	}

	public bool OnPointerClick(PointerEventData e){
		// Debug.Log("OnPointerClick");
		return true;
	}

	public async UniTask<string> selectAsync(){

		await UniTask.WhenAny(waitKeyDown(), waitMouseClick());

		Toggle selected = this.gameObject.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
		string value = selected.GetComponent<ButtonWithValue>().value;

		return value;
	}
}