using Cysharp.Threading.Tasks;

using System.Linq;

using UnityEngine;
using UnityEngine.UI;

class ModeSelector : ToggleGroupBase
{
	public ToggleGroup toggleGroup;
	public async UniTask<string> selectAsync(){
		await UniTask.WhenAny(waitKeyDown(), waitMouseClick());

		Toggle selected = toggleGroup.ActiveToggles().First<Toggle>();
		string value = selected.GetComponent<ButtonWithValue>().value;

		return value;
	}
}