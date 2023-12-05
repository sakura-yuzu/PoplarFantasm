using Cysharp.Threading.Tasks;

using System.Linq;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

class LocaleSelector : ToggleGroupBase
{

	public async UniTask<string> selectAsync(){

		await UniTask.WhenAny(waitKeyDown(), waitMouseClick());

		Toggle selected = this.gameObject.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
		string value = selected.GetComponent<ButtonWithValue>().value;

		// はじめからとか続きからとか表示したいのでここで変更しておく
		Locale locale = LocalizationSettings.AvailableLocales.GetLocale(value);
		LocalizationSettings.SelectedLocale = locale;

		return value;
	}
}