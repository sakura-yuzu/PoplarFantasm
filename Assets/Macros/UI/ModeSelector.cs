using Cysharp.Threading.Tasks;

using System.Linq;

using UnityEngine;
using UnityEngine.UI;

class ModeSelector : ToggleGroupBase
{
	public async UniTask<string> selectAsync(){

		await UniTask.WhenAny(waitKeyDown(), waitMouseClick());

		Toggle selected = this.gameObject.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
		string value = selected.GetComponent<ButtonWithValue>().value;

		return value;
	}
}