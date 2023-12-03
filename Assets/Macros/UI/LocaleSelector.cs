using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

using TMPro;

class LocaleSelector : MonoBehaviour, IPointerClickHandler
{
	public SaveData saveData;
	void Update(){
		if(Input.GetKeyDown(KeyCode.Return)){
			// Debug.Log("Return");
			SaveLocale();
		}
	}

	public void OnPointerClick(PointerEventData e){
		// Debug.Log("OnPointerClick");
		SaveLocale();
	}

	private void SaveLocale(){
		Toggle selected = this.gameObject.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
		string value = selected.GetComponent<ButtonWithValue>().value;
		Locale locale = LocalizationSettings.AvailableLocales.GetLocale(value);
		LocalizationSettings.SelectedLocale = locale;
		// TODO: いるのかいらないのかよくわからない
		saveData.selectedLocale = value;
	}
}